using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Catalog.HashTags;
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

        private readonly IMapper _mapper;

        public PostService(UserManager<User> userManager, IMapper mapper, IImageService image,
            VietNamHistoryContext vietNamHistoryContext) {
            _userManager = userManager;
            _mapper = mapper;
            _image = image;
            _dataContext = vietNamHistoryContext;
        }
        public async Task<ApiResult<PostResponsetDto>> Create(CreatePostDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var post = _mapper.Map<Post>(requestDto);
            post.Image = await _image.ConvertFormFileToByteArray(requestDto.Image);
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
                
                postReponse.Image = _image.ConvertByteArrayToString(post.Image, Encoding.UTF8);
                var useDto = new UserShortDto()
                {
                    FullName = user.Fullname,
                    Id = user.Id,
                    Image = _image.ConvertByteArrayToString(user.Image, Encoding.UTF8)
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

            updatePost.Image = await _image.ConvertFormFileToByteArray(requestDto.Image);
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
                
                postReponse.Image = _image.ConvertByteArrayToString(updatePost.Image, Encoding.UTF8);
                var useDto = new UserShortDto()
                {
                    FullName = user.Fullname,
                    Id = user.Id,
                    Image = _image.ConvertByteArrayToString(user.Image, Encoding.UTF8)
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
            postResponse.Image = _image.ConvertByteArrayToString(post.Image, Encoding.UTF8);

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
                Image = _image.ConvertByteArrayToString(user.Image, Encoding.UTF8)
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

            var result = new List<PostResponsetDto>();
            foreach (var item in posts)
            {
                var post = _mapper.Map<PostResponsetDto>(item);
                var userShort = await _dataContext.User.Where(x => x.Id == item.UserId).FirstOrDefaultAsync();
                if (userShort is not null)
                {
                    post.UserShort.FullName = userShort.Fullname;
                    post.UserShort.Id = userShort.Id;
                    post.UserShort.Image = _image.ConvertByteArrayToString(userShort.Image, Encoding.UTF8);
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

                post.Image = _image.ConvertByteArrayToString(item.Image, Encoding.UTF8);
                result.Add(post);
            }

            return new ApiSuccessResult<List<PostResponsetDto>>(result);    
        }

        public async Task<ApiResult<bool>> Delete(string id)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (post is null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy bài viết");
            }
            _dataContext.Posts.Remove(post);

            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<bool>(true);
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
