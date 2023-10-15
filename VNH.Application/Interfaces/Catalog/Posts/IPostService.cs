using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Interfaces.Posts
{
    public interface IPostService
    {
        Task<ApiResult<PostResponsetDto>> Create(CreatePostDto requestDto, string name);
        Task<ApiResult<PostResponsetDto>> Update(CreatePostDto requestDto, string name);
        Task<ApiResult<PostResponsetDto>> Detail(string Id);
        Task<ApiResult<List<PostResponsetDto>>> GetAll();
        Task<ApiResult<bool>> Delete(string id);
        Task<ApiResult<bool>> AddLikePost(string id, string userId);
    }
}