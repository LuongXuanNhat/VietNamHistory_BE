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
        Task<ApiResult<string>> Delete(string id);

        Task<ApiResult<string>> AddOrUnLikePost(string id, string userId);
        Task<ApiResult<string>> AddOrRemoveSavePost(string postId, string userId);
        Task<ApiResult<string>> ReportPost(ReportPostDto reportPostDto);
        Task<List<ReportPostDto>> GetReport();
    }
}