using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Posts;
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
        private readonly IHashTag _hashTag;
        private readonly IImageService _image;

        private readonly IMapper _mapper;

        public PostService(UserManager<User> userManager, IMapper mapper, IImageService image,
            VietNamHistoryContext vietNamHistoryContext, IHashTag HashTag) {
            _userManager = userManager;
            _mapper = mapper;
            _image = image;
            _dataContext = vietNamHistoryContext;
            _hashTag = HashTag;
        }
        public async Task<ApiResult<PostResponsetDto>> Create(CreatePostDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var post = _mapper.Map<Post>(requestDto);
            post.Image = await _image.ConvertFormFileToByteArray(requestDto.Image);
            post.CreatedAt = DateTime.Now;
            post.UserId = user.Id;
            post.TopicId = requestDto.TopicId;
            post.Id = post.Title.Trim().Replace(" ","-") + "-" + Guid.NewGuid().ToString();
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
                    await _hashTag.AddTag(tag);
                }
                

                var postReponse = _mapper.Map<PostResponsetDto>(post);
                
                postReponse.Image = _image.ConvertByteArrayToString(post.Image, Encoding.UTF8);
                postReponse.User = user;
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
                postReponse.Topic = topic.Title;

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

            var topic = await _dataContext.Topics.FirstAsync(x => x.Id == post.TopicId);
            postResponse.Topic = topic.Title;

            postResponse.SaveNumber = await _dataContext.PostSaves.Where(x=>x.PostId.Equals(Id)).CountAsync();
            postResponse.CommentNumber = await _dataContext.PostComments.Where(x => x.PostId.Equals(Id)).CountAsync();
            postResponse.LikeNumber = await _dataContext.PostLikes.Where(x=>x.PostId.Equals(x.PostId)).CountAsync();

            return new ApiSuccessResult<PostResponsetDto>(postResponse);
        }
    }
}
