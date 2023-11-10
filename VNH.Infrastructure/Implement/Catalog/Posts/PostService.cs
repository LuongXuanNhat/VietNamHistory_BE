using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Text;
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
        public async Task<ApiResult<PostResponsetDto>> Create(CreatePostDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var post = _mapper.Map<Post>(requestDto);
            post.Image = await _image.SaveFile(requestDto.Image);
            post.CreatedAt = DateTime.Now;
            post.UserId = user.Id;
            post.TopicId = requestDto.TopicId;
            var Id = RemoveDiacritics(post.Title);
            post.Id = Id.Trim().Replace(" ","-") + "-" + Guid.NewGuid().ToString();
            try
            {
                _dataContext.Posts.Add(post);
                await _dataContext.SaveChangesAsync();

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

                var postReponse = _mapper.Map<PostResponsetDto>(post);
                
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

                return new ApiSuccessResult<PostResponsetDto>(postReponse);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<PostResponsetDto>("Lỗi lưu bài viết : " + ex.Message);
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

        public async Task<ApiResult<PostResponsetDto>> Update(CreatePostDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);

            var updatePost = _dataContext.Posts.First(x=>x.Id.Equals(requestDto.Id));
            if (updatePost is null)
            {
                return new ApiErrorResult<PostResponsetDto>("Lỗi :Bài viết không được cập nhập (không tìm thấy bài viết)");
            }
            if (updatePost.Image != string.Empty)
            {
                await _storageService.DeleteFileAsync(updatePost.Image);
            }
            updatePost.Image = await _image.SaveFile(requestDto.Image);
            updatePost.UpdatedAt = DateTime.Now;
            updatePost.TopicId = requestDto.TopicId;
            updatePost.Content = requestDto.Content;

            try
            {
                _dataContext.Posts.Update(updatePost);
                await _dataContext.SaveChangesAsync();

                var tagOfPost = await _dataContext.PostTags.Where(x => x.PostId.Equals(updatePost.Id)).ToListAsync();
                _dataContext.PostTags.RemoveRange(tagOfPost);
                await _dataContext.SaveChangesAsync();

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

                var postReponse = _mapper.Map<PostResponsetDto>(updatePost);
                
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

                return new ApiSuccessResult<PostResponsetDto>(postReponse);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<PostResponsetDto>("Lỗi lưu bài viết : " + ex.Message);
            }
        }

        public async Task<ApiResult<PostResponsetDto>> Detail(string Id)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x=>x.Id.Equals(Id));
            if (post is null)
            {
                return new ApiErrorResult<PostResponsetDto>("Không tìm thấy bài viết");
            }
            var user = await _userManager.FindByIdAsync(post.UserId.ToString());
            var postResponse = _mapper.Map<PostResponsetDto>(post);
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

            var topic = await _dataContext.Topics.FirstAsync(x => x.Id == post.TopicId);
            postResponse.TopicName = topic.Title;

            postResponse.SaveNumber = await _dataContext.PostSaves.Where(x=>x.PostId.Equals(Id)).CountAsync();
            postResponse.CommentNumber = await _dataContext.PostComments.Where(x => x.PostId.Equals(Id)).CountAsync();
            postResponse.LikeNumber = await _dataContext.PostLikes.Where(x=>x.PostId.Equals(x.PostId)).CountAsync();

            return new ApiSuccessResult<PostResponsetDto>(postResponse);
        }

        public async Task<ApiResult<List<PostResponsetDto>>> GetAll()
        {
            var posts = await _dataContext.Posts.ToListAsync();
            var topics = await _dataContext.Topics.ToListAsync();
            var users = await _dataContext.User.ToListAsync();

            var result = new List<PostResponsetDto>();
            foreach (var item in posts)
            {
                var post = _mapper.Map<PostResponsetDto>(item);
                var userShort = users.First(x => x.Id == item.UserId);
                if (userShort is not null)
                {
                    post.UserShort.FullName = userShort.Fullname;
                    post.UserShort.Id = userShort.Id;
                    post.UserShort.Image = userShort.Image;
                }
                var tags = await _dataContext.PostTags
                            .Where(x => x.PostId == item.Id)
                            .Join(
                                _dataContext.Tags,
                                postTag => postTag.TagId,
                                tag => tag.Id,
                                (postTag, tag) => tag)
                            .ToListAsync();
                foreach (var tag in tags)
                {
                    post.Tags.Add(_mapper.Map<TagDto>(tag));
                }
                post.TopicName = topics.Where(x => x.Id == item.TopicId).Select(x=>x.Title).First();
                post.Image = item.Image;
                result.Add(post);
            }

            return new ApiSuccessResult<List<PostResponsetDto>>(result);    
        }

        public async Task<ApiResult<string>> Delete(string id)
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

        public async Task<ApiResult<string>> AddOrUnLikePost(string id, string userId)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (post is null)
            {
                return new ApiErrorResult<string>("Không tìm thấy bài viết");
            }
            var check = _dataContext.PostLikes.Where(x => x.PostId == id && x.UserId == Guid.Parse(userId)).FirstOrDefault();
            var mess = "";
            if (check is null)
            {
                var like = new PostLike()
                {
                    Id     = Guid.NewGuid(),
                    PostId = id,
                    UserId = Guid.Parse(userId)
                };
                _dataContext.PostLikes.Add(like);
                mess = "Đã thích";
            } else
            {
                _dataContext.PostLikes.Remove(check);
                mess = "Đã bỏ thích";
            }
            
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<string>(mess);
        }

        public async Task<ApiResult<string>> AddOrRemoveSavePost(string postId, string userId)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id.Equals(postId));
            if (post is null)
            {
                return new ApiErrorResult<string>("Không tìm thấy bài viết");
            }
            var check = _dataContext.PostSaves.Where(x => x.PostId == postId && x.UserId == Guid.Parse(userId)).FirstOrDefault();
            var mess = "";
            if (check is null)
            {
                var save = new PostSave()
                {
                    Id = Guid.NewGuid(),
                    PostId = postId,
                    UserId = Guid.Parse(userId)
                };
                _dataContext.PostSaves.Add(save);
                mess = "Đã lưu";
            }
            else
            {
                _dataContext.PostSaves.Remove(check);
                mess = "Đã bỏ lưu";
            }

            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<string>(mess);
        }

        public async Task<ApiResult<string>> ReportPost(ReportPostDto reportPostDto)
        {
            var reportPost = _mapper.Map<PostReportDetail>(reportPostDto);
            reportPost.ReportDate = DateTime.Now;
            reportPost.Id = Guid.NewGuid();

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
    }
}
