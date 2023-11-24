﻿using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Common;
using VNH.Application.Interfaces.Posts;
using VNH.Domain;
using VNH.Domain.Entities;
using VNH.Infrastructure.Presenters;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.Posts
{
    public class PostService : IPostService
    {
        private readonly UserManager<User> _userManager;
        private readonly VietNamHistoryContext _dataContext;
        private readonly IImageService _image;
        private readonly IStorageService _storageService;
        private readonly IHubContext<ChatSignalR> _commentHubContext;
        private readonly IMapper _mapper;

        public PostService(UserManager<User> userManager, IMapper mapper, IImageService image,
            VietNamHistoryContext vietNamHistoryContext, IStorageService storageService,
            IHubContext<ChatSignalR> chatSignalR)
        {
            _userManager = userManager;
            _mapper = mapper;
            _image = image;
            _dataContext = vietNamHistoryContext;
            _storageService = storageService;
            _commentHubContext = chatSignalR;
        }
        public async Task<ApiResult<PostResponseDto>> Create(CreatePostDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var post = _mapper.Map<Post>(requestDto);
            post.Image = await _image.SaveFile(requestDto.Image);
            post.CreatedAt = DateTime.Now;
            post.UserId = user.Id;
            post.TopicId = requestDto.TopicId;
            string formattedDateTime = post.CreatedAt.ToString("HH:mm:ss.fff-dd-MM-yyyy");
            var Id = SanitizeString(post.Title);
            post.SubId = Id + "-" + formattedDateTime;
            try
            {
                _dataContext.Posts.Add(post);
                await _dataContext.SaveChangesAsync();

                if (!requestDto.Tag.IsNullOrEmpty())
                {
                    foreach (var item in requestDto.Tag)
                    {
                        var tag = new Tag()
                        {
                            Id = Guid.NewGuid(),
                            Name = item
                        };
                        var postTag = new PostTag()
                        {
                            PostId = post.Id,
                            TagId = tag.Id
                        };
                        _dataContext.PostTags.Add(postTag);
                        _dataContext.Tags.Add(tag);
                    }
                    await _dataContext.SaveChangesAsync();
                }
                var postReponse = _mapper.Map<PostResponseDto>(post);
                
                var useDto = new UserShortDto()
                {
                    FullName = user.Fullname,
                    Id = user.Id,
                    Image = user.Image
                };
                postReponse.UserShort = useDto;
                var listPostTag = await _dataContext.PostTags.Where(x => x.PostId.Equals(postReponse.Id)).Select(x => x.TagId).ToListAsync();
                
                var tags = await _dataContext.Tags
                                    .Where(x => listPostTag.Any(TagId => TagId == x.Id))
                                    .ToListAsync();
                foreach (var tag in tags)
                {
                    postReponse.Tags.Add(new TagDto()
                    {
                        Name = tag.Name,
                        Id = tag.Id,
                    });
                }

                var topic = await _dataContext.Topics.FirstAsync(x => x.Id == post.TopicId);
                postReponse.TopicName = topic.Title;

                return new ApiSuccessResult<PostResponseDto>(postReponse);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<PostResponseDto>("Lỗi lưu bài viết : " + ex.Message);
            }
        }

        private static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        private static string SanitizeString(string input)
        {
            string withoutDiacritics = RemoveDiacritics(input).Trim().Replace(" ", "-");
            string sanitizedString = Regex.Replace(withoutDiacritics, "[^a-zA-Z0-9-]", "");

            return sanitizedString;
        }

        public async Task<ApiResult<PostResponseDto>> Update(CreatePostDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);

            var updatePost = _dataContext.Posts.First(x=>x.Id.Equals(requestDto.Id));
            if (updatePost is null)
            {
                return new ApiErrorResult<PostResponseDto>("Lỗi :Bài viết không được cập nhập (không tìm thấy bài viết)");
            }
            if (updatePost.Image != string.Empty && requestDto.Image != null)
            {
                await _storageService.DeleteFileAsync(updatePost.Image);
                updatePost.Image = await _image.SaveFile(requestDto.Image);
            }
            updatePost.Title = requestDto.Title;
            updatePost.UpdatedAt = DateTime.Now;
            updatePost.TopicId = requestDto.TopicId;
            updatePost.Content = requestDto.Content;
            string formattedDateTime = DateTime.Now.ToString("HH:mm:ss.fff-dd-MM-yy");
            var Id = SanitizeString(requestDto.Title);
            updatePost.SubId = Id.Trim().Replace(" ", "-") + "-" + formattedDateTime;
            try
            {
                _dataContext.Posts.Update(updatePost);
                await _dataContext.SaveChangesAsync();

                var tagOfPost = await _dataContext.PostTags.Where(x => x.PostId.Equals(updatePost.Id)).ToListAsync();
                _dataContext.PostTags.RemoveRange(tagOfPost);

                foreach (var item in requestDto.Tag)
                {
                    var tag = new Tag()
                    {
                        Id = Guid.NewGuid(),
                        Name = item
                    };
                    var postTag = new PostTag()
                    {
                        PostId = requestDto.Id,
                        TagId = tag.Id
                    };
                    _dataContext.PostTags.Add(postTag);
                    _dataContext.Tags.Add(tag);
                }
                await _dataContext.SaveChangesAsync();

                var postReponse = _mapper.Map<PostResponseDto>(updatePost);
                
                var useDto = new UserShortDto()
                {
                    FullName = user.Fullname,
                    Id = user.Id,
                    Image = user.Image
                };
                postReponse.UserShort = useDto;
                var listPostTag = await _dataContext.PostTags.Where(x => x.PostId.Equals(postReponse.Id)).Select(x => x.TagId).ToListAsync();
                
                var tags = await _dataContext.Tags
                                    .Where(x => listPostTag.Any(TagId => TagId == x.Id))
                                    .ToListAsync();
                foreach (var tag in tags)
                {
                    postReponse.Tags.Add(new TagDto()
                    {
                        Name = tag.Name,
                        Id = tag.Id,
                    });
                }

                var topic = await _dataContext.Topics.FirstAsync(x => x.Id == updatePost.TopicId);
                postReponse.TopicName = topic.Title;

                return new ApiSuccessResult<PostResponseDto>(postReponse);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<PostResponseDto>("Lỗi lưu bài viết : " + ex.Message);
            }
        }

        public async Task<ApiResult<PostResponseDto>> Detail(string Id)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x=>x.SubId.Equals(Id));
            if (post is null)
            {
                return new ApiErrorResult<PostResponseDto>("Không tìm thấy bài viết");
            }
            var user = await _userManager.FindByIdAsync(post.UserId.ToString());
            var postResponse = _mapper.Map<PostResponseDto>(post);

            var listPostTag = await _dataContext.PostTags.Where(x => x.PostId.Equals(postResponse.Id)).Select(x => x.TagId).ToListAsync();
            var tags = await _dataContext.Tags
                                    .Where(x => listPostTag.Any(TagId => TagId == x.Id))
                                    .ToListAsync();
            foreach (var tag in tags)
            {
                postResponse.Tags.Add(new()
                {
                    Name = tag.Name,
                    Id = tag.Id,
                });
            }
            postResponse.UserShort = new()
            {
                FullName = user.Fullname,
                Id = user.Id,
                Image = user.Image
            };
            post.ViewNumber += 1;
            var topic = await _dataContext.Topics.FirstOrDefaultAsync(x => x.Id == post.TopicId);
            postResponse.TopicName = topic?.Title;
            postResponse.SaveNumber = await _dataContext.PostSaves.Where(x=>x.PostId.Equals(post.Id)).CountAsync();
            postResponse.CommentNumber = await _dataContext.PostComments.Where(x => x.PostId.Equals(post.Id)).CountAsync();
            postResponse.LikeNumber = await _dataContext.PostLikes.Where(x=>x.PostId.Equals(post.Id)).CountAsync();

            _dataContext.Posts.Update(post);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<PostResponseDto>(postResponse);
        }

        public async Task<ApiResult<List<PostResponseDto>>> GetAll()
        {
            var posts = await _dataContext.Posts.ToListAsync();
            var users = await _dataContext.User.ToListAsync();
            var topics = await _dataContext.Topics.ToListAsync();

            var result = new List<PostResponseDto>();
            foreach (var item in posts)
            {
                var post = _mapper.Map<PostResponseDto>(item);
                var userShort = users.First(x => x.Id == item.UserId);
                if (userShort is not null)
                {
                    post.UserShort.FullName = userShort.Fullname;
                    post.UserShort.Id = userShort.Id;
                    post.UserShort.Image = userShort.Image;
                }
                post.TopicName = topics.FirstOrDefault(x => x.Id == item.TopicId)?.Title ?? "";
                result.Add(post);
            }

            return new ApiSuccessResult<List<PostResponseDto>>(result);    
        }
        public async Task<ApiResult<List<PostResponseDto>>> GetAllMobile()
        {
            var posts = await _dataContext.Posts.ToListAsync();
            var users = await _dataContext.User.ToListAsync();
            var topics = await _dataContext.Topics.ToListAsync();

            var result = new List<PostResponseDto>();
            foreach (var item in posts)
            {
                var post = _mapper.Map<PostResponseDto>(item);
                var userShort = users.First(x => x.Id == item.UserId);
                if (userShort is not null)
                {
                    post.UserShort.FullName = userShort.Fullname;
                    post.UserShort.Id = userShort.Id;
                    post.UserShort.Image = userShort.Image;
                }
                post.TopicName = topics.FirstOrDefault(x => x.Id == item.TopicId)?.Title;
                post.SaveNumber = await _dataContext.PostSaves.Where(x => x.PostId.Equals(post.Id)).CountAsync();
                post.CommentNumber = await _dataContext.PostComments.Where(x => x.PostId.Equals(post.Id)).CountAsync();
                post.LikeNumber = await _dataContext.PostLikes.Where(x => x.PostId.Equals(post.Id)).CountAsync();
                result.Add(post);
            }

            return new ApiSuccessResult<List<PostResponseDto>>(result);    
        }

        public async Task<ApiResult<string>> Delete(string id, string userId)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id.Equals(id) && x.UserId.ToString().Equals(userId));
            if (post is null)
            {
                return new ApiErrorResult<string>("Không tìm thấy bài viết");
            }
            if (post.Image != string.Empty)
            {
                await _storageService.DeleteFileAsync(post.Image);
            }
            _dataContext.Posts.Remove(post);

            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<string>("Đã xóa bài viết");
        }

        public async Task<ApiResult<string>> DeleteAdmin(string id)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (post is null)
            {
                return new ApiErrorResult<string>("Không tìm thấy bài viết");
            }
            if (post.Image != string.Empty)
            {
                await _storageService.DeleteFileAsync(post.Image);
            }
            _dataContext.Posts.Remove(post);

            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<string>("Đã xóa bài viết");
        }

        public async Task<ApiResult<int>> AddOrUnLikePost(PostFpkDto postFpk)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.SubId.Equals(postFpk.PostId));
            if (post is null)
            {
                return new ApiErrorResult<int>("Không tìm thấy bài viết");
            }
            var check = _dataContext.PostLikes.Where(x => x.PostId == post.Id && x.UserId == Guid.Parse(postFpk.UserId)).FirstOrDefault();
            var likeNumber = await _dataContext.PostLikes.Where(x => x.PostId == post.Id).CountAsync();
            if (check is null)
            {
                var like = new PostLike()
                {
                    Id     = Guid.NewGuid(),
                    PostId = post.Id,
                    UserId = Guid.Parse(postFpk.UserId)
                };
                _dataContext.PostLikes.Add(like);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<int>(likeNumber+1);
            } else
            {
                _dataContext.PostLikes.Remove(check);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<int>(likeNumber-1);
            }
            
            
        }

        public async Task<ApiResult<int>> AddOrRemoveSavePost(PostFpkDto postFpk)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.SubId.Equals(postFpk.PostId));
            if (post is null)
            {
                return new ApiErrorResult<int>("Không tìm thấy bài viết");
            }
            var check = _dataContext.PostSaves.Where(x => x.PostId == post.Id && x.UserId == Guid.Parse(postFpk.UserId)).FirstOrDefault();
            var saveNumber = await _dataContext.PostSaves.Where(x => x.PostId == post.Id).CountAsync();
            if (check is null)
            {
                var save = new PostSave()
                {
                    Id = Guid.NewGuid(),
                    PostId = post.Id,
                    UserId = Guid.Parse(postFpk.UserId)
                };
                _dataContext.PostSaves.Add(save);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<int>(saveNumber+1);
            }
            else
            {
                _dataContext.PostSaves.Remove(check);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<int>(saveNumber-1);
            }

            
        }

        public async Task<ApiResult<string>> ReportPost(ReportPostDto reportPostDto)
        {
            var post = _dataContext.Posts.FirstOrDefault(x => x.SubId.Equals(reportPostDto.PostId));
            if (post == null)
            {
                return new ApiErrorResult<string>("Bài viết không tồn tại");
            }
            var reportPost = _mapper.Map<PostReportDetail>(reportPostDto);
            reportPost.Id = Guid.NewGuid();
            reportPost.PostId = post.Id;
            _dataContext.PostReportDetails.Add(reportPost);
            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<string>("Đã gửi báo cáo đến kiểm duyệt viên! Chúng tôi sẽ phản hồi bạn sớm nhất có thể! Xin cảm ơn.");
        }

        [Authorize(Roles = "admin")]
        public async Task<List<ReportPostDto>> GetReport()
        {
            var reportPost = await _dataContext.PostReportDetails
                .OrderByDescending(x=>x.ReportDate)
                .ToListAsync();
            var results = new List<ReportPostDto>();
            foreach (var item in reportPost)
            {
                results.Add(_mapper.Map<ReportPostDto>(item));
            }
            return results;
        }

        public async Task<ApiResult<bool>> GetLike(PostFpkDto postFpk)
        {
            var post = _dataContext.Posts.First(x => x.SubId.Equals(postFpk.PostId));
            var check = await _dataContext.PostLikes.Where(x => x.PostId.Equals(post.Id) && x.UserId == Guid.Parse(postFpk.UserId)).FirstOrDefaultAsync();
            var reuslt = check != null;
            return new ApiSuccessResult<bool>(reuslt);
        }

        public async Task<ApiResult<bool>> GetSave(PostFpkDto postFpk)
        {
            var post = _dataContext.Posts.First(x => x.SubId.Equals(postFpk.PostId));
            var check = await _dataContext.PostSaves.Where(x => x.PostId.Equals(post.Id) && x.UserId == Guid.Parse(postFpk.UserId)).FirstOrDefaultAsync();
            var reuslt = check != null;
            return new ApiSuccessResult<bool>(reuslt);
        }

        public async Task<ApiResult<List<PostResponseDto>>> GetPostByTag(string tag)
        {
            var posts = await _dataContext.Posts
                            .Where(post => _dataContext.PostTags
                                .Any(postTag => _dataContext.Tags
                                .Any(tagEntity => tagEntity.Name.ToLower().Equals(tag.ToLower()) && tagEntity.Id == postTag.TagId)
                                && postTag.PostId == post.Id))
                                .ToListAsync();
            if (posts.IsNullOrEmpty())
            {
                return new ApiSuccessResult<List<PostResponseDto>>(new List<PostResponseDto>());
            }
            var users = await _dataContext.User.ToListAsync();

            var result = new List<PostResponseDto>();
            foreach (var item in posts)
            {
                var post = _mapper.Map<PostResponseDto>(item);
                var userShort = users.First(x => x.Id == item.UserId);
                if (userShort is not null)
                {
                    post.UserShort.FullName = userShort.Fullname;
                    post.UserShort.Id = userShort.Id;
                    post.UserShort.Image = userShort.Image;
                }
                result.Add(post);
            }
            return new ApiSuccessResult<List<PostResponseDto>>(result);
        }

        public async Task<ApiResult<List<CommentPostDto>>> GetComment(string postId)
        {
            var post = await _dataContext.Posts.FirstAsync(x => x.SubId.Equals(postId));
            if (post == null)
            {
                return new ApiSuccessResult<List<CommentPostDto>>();
            }
            var users = await _dataContext.User.ToListAsync();
            var postComment = await _dataContext.PostComments.Where(x=>x.PostId.Equals(post.Id)).ToListAsync();
            var postSubComment = await _dataContext.PostSubComments.ToListAsync();

            var result = new List<CommentPostDto>();
            foreach (var item in postComment)
            {
                var subComment = postSubComment.Where(x=>x.PreCommentId.Equals(item.Id)).ToList();
                var comment = _mapper.Map<CommentPostDto>(item);
                comment.UserShort = GetUserShort(users, item.UserId);
                if(subComment.Count > 0)
                {
                    comment.SubComment = GetSubComment(subComment, users);
                }
                result.Add(comment);
            }
            return new ApiSuccessResult<List<CommentPostDto>>(result);
        }

        private List<SubCommentDto>? GetSubComment(List<PostSubComment> subComment, List<User> users)
        {
            List<SubCommentDto> result = new();
            foreach (var item in subComment)
            {
                var comment = _mapper.Map<SubCommentDto>(item);
                comment.UserShort = GetUserShort(users, item.UserId);
                result.Add(comment);
            }
            return result;
        }

        private  UserShortDto? GetUserShort(List<User> users, Guid? IdUser)
        {
            return users
                .Where(x => x.Id.Equals(IdUser))
                .Select(x => new UserShortDto
                {
                    Id = x.Id,
                    FullName = x.Fullname,
                    Image = x.Image
                })
                .FirstOrDefault();
        }

        public async Task<ApiResult<List<CommentPostDto>>> CreateComment(CommentPostDto comment)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x=>x.SubId.Equals(comment.PostId));
            if (post == null)
            {
                return new ApiErrorResult<List<CommentPostDto>>("Không tìm thấy bài đọc bạn bình luận, có thể nó đã bị xóa");
            }
            PostComment postComment = _mapper.Map<PostComment>(comment);
            postComment.PostId = post.Id;
            _dataContext.PostComments.Add(postComment);
            await _dataContext.SaveChangesAsync();
            var comments = await GetComment(comment.PostId);
            await _commentHubContext.Clients.All.SendAsync("ReceiveComment", comments);

            return comments;
        }

        public async Task<ApiResult<List<CommentPostDto>>> UpdateComment(CommentPostDto comment)
        {
            var postComment = await _dataContext.PostComments.FirstOrDefaultAsync(x => x.Id == comment.Id);
            if (postComment == null)
            {
                return new ApiErrorResult<List<CommentPostDto>> ("Không tìm thấy bài đọc bạn bình luận!");
            }
            postComment.Content = comment.Content;
            postComment.UpdatedAt = DateTime.Now;
            
            _dataContext.PostComments.Update(postComment);
            await _dataContext.SaveChangesAsync();

            var subPost = await _dataContext.Posts.FirstAsync(x => x.Id == comment.PostId);
            var comments = await GetComment(subPost.SubId);
            await _commentHubContext.Clients.All.SendAsync("ReceiveComment", comments);

            return comments;
        }

        public async Task<ApiResult<string>> DeteleComment(string id)
        {
            var comment = await _dataContext.PostComments.FirstOrDefaultAsync(x=>x.Id.Equals(Guid.Parse(id)));
            if (comment == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy bình luận");
            }
            _dataContext.PostComments.Remove(comment);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<string>();
        }

        public async Task<ApiResult<List<PostResponseDto>>> GetMyPostSaved(string id)
        {
            Guid userId = Guid.Parse(id);
            var users = await _dataContext.User.ToListAsync();

            var posts = await (
                from postSave in _dataContext.PostSaves
                join post in _dataContext.Posts on postSave.PostId equals post.Id
                where postSave.UserId == userId
                select post
            ).ToListAsync();

            var result = new List<PostResponseDto>();
            foreach (var item in posts)
            {
                var post = _mapper.Map<PostResponseDto>(item);
                var userShort = users.First(x => x.Id == item.UserId);
                if (userShort is not null)
                {
                    post.UserShort.FullName = userShort.Fullname;
                    post.UserShort.Id = userShort.Id;
                    post.UserShort.Image = userShort.Image;
                }
                result.Add(post);
            }
            return new ApiSuccessResult<List<PostResponseDto>>(result);
        }
        public async Task<ApiResult<List<PostResponseDto>>> GetMyPost(string id)
        {
            var posts = await _dataContext.Posts.ToListAsync();
            var result = new List<PostResponseDto>();
            foreach (var item in posts)
            {
                var post = _mapper.Map<PostResponseDto>(item);
                result.Add(post);
                post.SaveNumber = await _dataContext.PostSaves.Where(x => x.PostId.Equals(post.Id)).CountAsync();
                post.CommentNumber = await _dataContext.PostComments.Where(x => x.PostId.Equals(post.Id)).CountAsync();
                post.LikeNumber = await _dataContext.PostLikes.Where(x => x.PostId.Equals(post.Id)).CountAsync();
            }

            return new ApiSuccessResult<List<PostResponseDto>>(result);
        }

        public async Task<ApiResult<List<PostResponseDto>>> SearchPosts(string keyWord)
        {
            if (keyWord.StartsWith("#"))
            {
                keyWord = keyWord.TrimStart('#');
                return await GetPostByTag(keyWord);
            }
            var users = await _dataContext.User.ToListAsync();
            var posts = new List<PostResponseDto>();
            string[] searchKeywords = keyWord.ToLower().Split(' ');
            var result = from post in _dataContext.Posts as IEnumerable<Post>
                         let titleWords = post.Title.ToLower().Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                         let searchPhrases = GenerateSearchPhrases(searchKeywords)
                         let matchingPhrases = searchPhrases
                            .Where(phrase => titleWords.Contains(phrase))
                         where matchingPhrases.Any()
                         let matchCount = matchingPhrases.Count()
                         orderby matchCount descending
                         select new Post()
                         {
                             Id = post.Id,
                             SubId = post.SubId,
                             Title = post.Title,
                             Image = post.Image,
                             CreatedAt = post.CreatedAt,
                             UpdatedAt = post.UpdatedAt,
                             UserId = post.UserId,
                             ViewNumber = post.ViewNumber
                         };
            
            foreach (var post in result)
            {
                var item = _mapper.Map<PostResponseDto>(post);
                var userShort = users.First(x => x.Id == post.UserId);
                if (userShort is not null)
                {
                    item.UserShort.FullName = userShort.Fullname;
                    item.UserShort.Id = userShort.Id;
                    item.UserShort.Image = userShort.Image;
                }
                posts.Add(item);
            }
            return new ApiSuccessResult<List<PostResponseDto>>(posts);
        }

        private static IEnumerable<string> GenerateSearchPhrases(string[] searchKeywords)
        {
            List<string> searchPhrases = new();
            for (int i = searchKeywords.Length; i >= 1; i--)
            {
                for (int j = 0; j <= searchKeywords.Length - i; j++)
                {
                    string phrase = string.Join(" ", searchKeywords.Skip(j).Take(i));
                    searchPhrases.Add(phrase);
                }
            }
            return searchPhrases;
        }
    }
}
