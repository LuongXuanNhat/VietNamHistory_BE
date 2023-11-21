using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
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
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.Posts
{
    public class PostService : IPostService
    {
        private readonly UserManager<User> _userManager;
        private readonly VietNamHistoryContext _dataContext;
        private readonly IImageService _image;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public PostService(UserManager<User> userManager, IMapper mapper, IImageService image,
            VietNamHistoryContext vietNamHistoryContext, IStorageService storageService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _image = image;
            _dataContext = vietNamHistoryContext;
            _storageService = storageService;
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
                
                postReponse.Image = post.Image;
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
            if (updatePost.Image != string.Empty)
            {
                await _storageService.DeleteFileAsync(updatePost.Image);
            }
            updatePost.Image = await _image.SaveFile(requestDto.Image);
            updatePost.UpdatedAt = DateTime.Now;
            updatePost.TopicId = requestDto.TopicId;
            updatePost.Content = requestDto.Content;
            string formattedDateTime = DateTime.Now.ToString("HH:mm:ss.fff-dd-MM-yyyy");
            var Id = SanitizeString(updatePost.Title);
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
                
                postReponse.Image = updatePost.Image;
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
            postResponse.Image = post.Image;

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
            var topic = await _dataContext.Topics.FirstAsync(x => x.Id == post.TopicId);
            postResponse.TopicName = topic.Title;
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
            var topics = await _dataContext.Topics.ToListAsync();
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
                post.Image = item.Image;
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
            var mess = "";
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
            var mess = "";
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

        public async Task<ApiResult<List<string>>> GetTopTags(int numberTag)
        {
            var tags = await _dataContext.Tags
                            .GroupBy(x => x.Name)
                            .Select(group => new { Name = group.Key, Count = group.Sum(t => 1) })
                            .OrderByDescending(tagName => tagName.Count)
                            .Take(numberTag)
                            .Select(group => group.Name)
                            .ToListAsync();

            return new ApiSuccessResult<List<string>>(tags);
        }

        public async Task<ApiResult<List<PostResponseDto>>> GetPostByTag(string tag)
        {
            var posts = await _dataContext.Posts
                            .Where(post => _dataContext.PostTags
                                .Any(postTag => _dataContext.Tags
                                .Any(tagEntity => tagEntity.Name.ToLower().Contains(tag.ToLower()) && tagEntity.Id == postTag.TagId)
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
                post.Image = item.Image;
                result.Add(post);
            }
            return new ApiSuccessResult<List<PostResponseDto>>(result);
        }
    }
}
