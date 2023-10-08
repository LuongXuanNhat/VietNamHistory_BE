using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Interfaces.Posts
{
    public interface IPostService
    {
        Task<ApiResult<PostResponsetDto>> Create(CreatePostDto requestDto, string name);
        Task<ApiResult<PostResponsetDto>> Detail(string Id);
    }
}