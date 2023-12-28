using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VNH.Application.Common.Contants;
using VNH.Application.DTOs.Catalog.Notifications;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Implement.Catalog.NotificationServices;
using VNH.Application.Interfaces.Common;
using VNH.Application.Interfaces.Posts;
using VNH.Domain;
using VNH.Domain.Entities;
using VNH.Infrastructure.Implement.Common;
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
        private readonly INotificationService _notificationService;

        public PostService(UserManager<User> userManager, IMapper mapper, IImageService image,
            VietNamHistoryContext vietNamHistoryContext, IStorageService storageService,
            IHubContext<ChatSignalR> chatSignalR, INotificationService notificationService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _image = image;
            _dataContext = vietNamHistoryContext;
            _storageService = storageService;
            _commentHubContext = chatSignalR;
            _notificationService = notificationService;
        }
        public async Task<ApiResult<PostResponseDto>> Create(CreatePostDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var post = _mapper.Map<Post>(requestDto);
            post.Image = await _image.SaveFile(requestDto.Image);
            post.CreatedAt = DateTime.Now;
            post.UserId = user.Id;
            post.TopicId = requestDto.TopicId;
            string formattedDateTime = post.CreatedAt.ToString("HHmmss.fff") + HandleCommon.GenerateRandomNumber().ToString();
            var Id = HandleCommon.SanitizeString(post.Title);
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

                var detail = await Detail(post.SubId);
                var postReponse = detail.ResultObj;

                return new ApiSuccessResult<PostResponseDto>(postReponse);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<PostResponseDto>("Lỗi lưu bài viết : " + ex.Message);
            }
        }
        public async Task<ApiResult<PostResponseDto>> Update(CreatePostDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);

            var updatePost = _dataContext.Posts.First(x=>x.Id.Equals(requestDto.Id) && !x.IsDeleted);
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
            string formattedDateTime = DateTime.Now.ToString("HHmmss.fff") + HandleCommon.GenerateRandomNumber().ToString();
            var Id = HandleCommon.SanitizeString(requestDto.Title);
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

                var detail = await Detail(updatePost.SubId);
                var postReponse = detail.ResultObj;

                return new ApiSuccessResult<PostResponseDto>(postReponse);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<PostResponseDto>("Lỗi lưu bài viết : " + ex.Message);
            }
        }

        public async Task<ApiResult<PostResponseDto>> Detail(string Id)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x=>x.SubId.Equals(Id) && !x.IsDeleted);
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
            var posts = await _dataContext.Posts.Where(x=>!x.IsDeleted).OrderByDescending(x => x.CreatedAt).ToListAsync();
            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();
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
            var posts = await _dataContext.Posts.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedAt).ToListAsync();
            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();
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
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id.Equals(id) && x.UserId.ToString().Equals(userId) && !x.IsDeleted);
            if (post is null)
            {
                return new ApiErrorResult<string>("Không tìm thấy bài viết");
            }
            //if (post.Image != string.Empty)
            //{
            //    await _storageService.DeleteFileAsync(post.Image);
            //}
            post.IsDeleted = true;
            _dataContext.Posts.Update(post);
            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<string>("Đã xóa bài viết");
        }

        public async Task<ApiResult<string>> DeleteAdmin(string id)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsDeleted);
            if (post is null)
            {
                return new ApiErrorResult<string>("Không tìm thấy bài viết");
            }
            //if (post.Image != string.Empty)
            //{
            //    await _storageService.DeleteFileAsync(post.Image);
            //}
            post.IsDeleted = true;
            _dataContext.Posts.Update(post);

            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<string>("Đã xóa bài viết");
        }

        public async Task<ApiResult<NumberReponse>> AddOrUnLikePost(PostFpkDto postFpk)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.SubId.Equals(postFpk.PostId) && !x.IsDeleted);
            if (post is null)
            {
                return new ApiErrorResult<NumberReponse>("Không tìm thấy bài viết");
            }
            var check = _dataContext.PostLikes.Where(x => x.PostId == post.Id && x.UserId == Guid.Parse(postFpk.UserId)).FirstOrDefault();
            var likeNumber = await _dataContext.PostLikes.Where(x => x.PostId == post.Id).CountAsync();
            if (check is null)
            {
                var user = await _dataContext.User.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == Guid.Parse(postFpk.UserId));
                var like = new PostLike()
                {
                    Id     = Guid.NewGuid(),
                    PostId = post.Id,
                    UserId = Guid.Parse(postFpk.UserId)
                };
                var noti = new NotificationDto()
                {
                    Id = Guid.NewGuid(),
                    UserId = post.UserId,
                    IdObject = Guid.Parse(post.Id),
                    Content = ConstantNofication.LikePost(user?.Fullname ?? ""),
                    Date = DateTime.Now,
                    Url = ConstantUrl.UrlPostDetail,
                    NotificationId = Guid.NewGuid()
                };

                _dataContext.PostLikes.Add(like);
                await _dataContext.SaveChangesAsync();
                await _notificationService.AddNotificationDetail(noti);
                return  new ApiSuccessResult<NumberReponse>(new() { Check = true, Quantity = likeNumber + 1});
            } else
            {
                _dataContext.PostLikes.Remove(check); 
                await _dataContext.SaveChangesAsync();
                return  new ApiSuccessResult<NumberReponse>(new() { Check = false, Quantity = likeNumber - 1 });
  
            }
            
            
        }

        public async Task<ApiResult<NumberReponse>> AddOrRemoveSavePost(PostFpkDto postFpk)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.SubId.Equals(postFpk.PostId) && !x.IsDeleted);
            if (post is null)
            {
                return new ApiErrorResult<NumberReponse>("Không tìm thấy bài viết");
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
                return new ApiSuccessResult<NumberReponse>(new() { Check = true, Quantity = saveNumber + 1 });
            }
            else
            {
                _dataContext.PostSaves.Remove(check);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<NumberReponse>(new() { Check = false, Quantity = saveNumber - 1 });
            }

            
        }

        public async Task<ApiResult<string>> ReportPost(ReportPostDto reportPostDto)
        {
            var post = _dataContext.Posts.FirstOrDefault(x => x.SubId.Equals(reportPostDto.PostId) && !x.IsDeleted);
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

        public async Task<ApiResult<List<ReportPostDto>>> GetReport()
        {
            var reportPost = await _dataContext.PostReportDetails
                .OrderByDescending(x=>x.ReportDate)
                .ToListAsync();
            var results = new List<ReportPostDto>();
            foreach (var item in reportPost)
            {
                results.Add(_mapper.Map<ReportPostDto>(item));
            }
            return new ApiSuccessResult<List<ReportPostDto>>(results);
        }

        public async Task<ApiResult<NumberReponse>> GetLike(PostFpkDto postFpk)
        {
            var post = _dataContext.Posts.First(x => x.SubId.Equals(postFpk.PostId) && !x.IsDeleted);
            var check = await _dataContext.PostLikes.Where(x => x.PostId.Equals(post.Id) && x.UserId == Guid.Parse(postFpk.UserId)).FirstOrDefaultAsync();
            var number = await _dataContext.PostLikes.Where(x => x.PostId.Equals(post.Id)).CountAsync();
            return new ApiSuccessResult<NumberReponse>(new() { Check = (check != null), Quantity = number});
        }

        public async Task<ApiResult<NumberReponse>> GetSave(PostFpkDto postFpk)
        {
            var post = _dataContext.Posts.First(x => x.SubId.Equals(postFpk.PostId) && !x.IsDeleted);
            var check = await _dataContext.PostSaves.Where(x => x.PostId.Equals(post.Id) && x.UserId == Guid.Parse(postFpk.UserId)).FirstOrDefaultAsync();
            var number = await _dataContext.PostSaves.Where(x => x.PostId.Equals(post.Id)).CountAsync();
            return new ApiSuccessResult<NumberReponse>(new() { Check = (check != null), Quantity = number });
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
            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();

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
                var topic = await _dataContext.Topics.FirstOrDefaultAsync(x => x.Id == item.TopicId);
                post.TopicName = topic?.Title;

                result.Add(post);
            }
            return new ApiSuccessResult<List<PostResponseDto>>(result);
        }

        public async Task<ApiResult<List<CommentPostDto>>> GetComment(string postId)
        {
            var post = await _dataContext.Posts.FirstAsync(x => x.SubId.Equals(postId) && !x.IsDeleted);
            if (post == null)
            {
                return new ApiSuccessResult<List<CommentPostDto>>();
            }
            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();
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
                .Where(x => x.Id.Equals(IdUser) && !x.IsDeleted)
                .Select(x => new UserShortDto
                {
                    Id = x.Id,
                    FullName = x.Fullname,
                    Image = x.Image
                })
                .FirstOrDefault();
        }

        public async Task<ApiResult<List<CommentPostDto>>> CreateComment(CommentPostDto comment, string userId)
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x=>x.SubId.Equals(comment.PostId) && !x.IsDeleted);
            if (post == null)
            {
                return new ApiErrorResult<List<CommentPostDto>>("Không tìm thấy bài đọc bạn bình luận, có thể nó đã bị xóa");
            }
            PostComment postComment = _mapper.Map<PostComment>(comment);
            postComment.PostId = post.Id;
            postComment.UpdatedAt = DateTime.Now;

            if (userId != post.UserId.ToString())
            {
                var user = await _dataContext.User.FirstOrDefaultAsync(x => x.Id.ToString().Equals(userId) && !x.IsDeleted);
                var noti = new NotificationDto()
                {
                    Id = Guid.NewGuid(),
                    UserId = post.UserId,
                    IdObject = Guid.Parse(post.Id),
                    Content = ConstantNofication.CommentPost(user?.Fullname ?? ""),
                    Date = DateTime.Now,
                    Url = ConstantUrl.UrlPostDetail,
                    NotificationId = Guid.NewGuid()
                };

                await _notificationService.AddNotificationDetail(noti);
            }
            

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

        public async Task<ApiResult<List<CommentPostDto>>> DeteleComment(string id)
        {
            var comment = await _dataContext.PostComments.FirstOrDefaultAsync(x=>x.Id.Equals(Guid.Parse(id)));
            if (comment == null)
            {
                return new ApiErrorResult<List<CommentPostDto>> ("Không tìm thấy bình luận");
            }
            _dataContext.PostComments.Remove(comment);
            await _dataContext.SaveChangesAsync();

            var subPost = await _dataContext.Posts.FirstAsync(x => x.Id == comment.PostId);
            var comments = await GetComment(subPost.SubId);
            await _commentHubContext.Clients.All.SendAsync("ReceiveComment", comments);
            return comments;
        }

        public async Task<ApiResult<List<PostResponseDto>>> GetMyPostSaved(string id)
        {
            Guid userId = Guid.Parse(id);
            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();
            var topics = await _dataContext.Topics.ToListAsync();

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
                post.TopicName = topics.FirstOrDefault(x => x.Id == item.TopicId)?.Title ?? "";
                result.Add(post);
            }
            return new ApiSuccessResult<List<PostResponseDto>>(result);
        }
        public async Task<ApiResult<List<PostResponseDto>>> GetMyPost(string id)
        {
            var posts = await _dataContext.Posts.Where(x=>x.UserId.Equals(Guid.Parse(id)) && !x.IsDeleted).ToListAsync();
            var result = new List<PostResponseDto>();
            var topics = await _dataContext.Topics.ToListAsync();
            var user = await _dataContext.User.Where(x => !x.IsDeleted && x.Id.ToString().Equals(id)).FirstOrDefaultAsync();

            foreach (var item in posts)
            {
                var post = _mapper.Map<PostResponseDto>(item);
                post.SaveNumber = await _dataContext.PostSaves.Where(x => x.PostId.Equals(post.Id)).CountAsync();
                post.CommentNumber = await _dataContext.PostComments.Where(x => x.PostId.Equals(post.Id)).CountAsync();
                post.LikeNumber = await _dataContext.PostLikes.Where(x => x.PostId.Equals(post.Id)).CountAsync();
                post.TopicName = topics.FirstOrDefault(x => x.Id == item.TopicId)?.Title ?? "";
                if (user is not null)
                {
                    post.UserShort.FullName = user.Fullname;
                    post.UserShort.Id = user.Id;
                    post.UserShort.Image = user.Image;
                }
                result.Add(post);
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
            var users = await _dataContext.User.Where(x=> !x.IsDeleted).ToListAsync();
            var posts = new List<PostResponseDto>();
            string[] searchKeywords = keyWord.ToLower().Split(' ');
            var result = from post in _dataContext.Posts.Where(x => !x.IsDeleted) as IEnumerable<Post>
                         let titleWords = post.Title.ToLower().Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                         let searchPhrases = HandleCommon.GenerateSearchPhrases(searchKeywords)
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
        public async Task<ApiResult<List<PostResponseDto>>> GetRandomPost(int quantity)
        {
            if (quantity == 0)
            {
                return await GetAll();
            }
            DateTime currentDate = DateTime.Now;
            DateTime startDate = currentDate.AddDays(-30);
            var posts = await _dataContext.Posts
                .Where(post => post.CreatedAt >= startDate && post.CreatedAt <= currentDate && !post.IsDeleted)
                .Take(100)
                .ToListAsync();

            var list = new List<Post>();
            int[] numbers = {};
            Random random = new Random();
            var maxNum = posts.Count < 100 ? posts.Count : 99;
            var getNum = quantity < posts.Count ? quantity : posts.Count;
            for (int i = 0; i < getNum; i++)
            {
                var index = random.Next(0, maxNum);
                if (!numbers.Contains(index))
                {
                    Array.Resize(ref numbers, numbers.Length + 1);
                    numbers[numbers.Length - 1] = index;
                    list.Add(posts[i]);
                } else
                {
                    i-=1;
                }
            }
            var result = _mapper.Map<List<PostResponseDto>>(list);

            return new ApiSuccessResult<List<PostResponseDto>>(result);

        }

        public async Task<ApiResult<List<PostResponseDto>>> FindByTopic(string topicName)
        {
            var posts = await _dataContext.Posts
                            .Where(post => _dataContext.TopicDetails 
                                .Any(postTag => _dataContext.Topics
                                .Any(tagEntity => tagEntity.Title.ToLower().Equals(topicName.ToLower()) && tagEntity.Id == postTag.TopicId)
                                && postTag.PostId == post.Id) && !post.IsDeleted)
                                .ToListAsync();
            if (posts.IsNullOrEmpty())
            {
                return new ApiSuccessResult<List<PostResponseDto>>(new List<PostResponseDto>());
            }
            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();

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
                var topic = await _dataContext.Topics.FirstOrDefaultAsync(x => x.Id == item.TopicId);
                post.TopicName = topic?.Title;

                result.Add(post);
            }
            return new ApiSuccessResult<List<PostResponseDto>>(result);
        }
    }
}
