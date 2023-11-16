using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Interfaces.Posts
{
    public interface IPostService
    {
        Task<ApiResult<PostResponseDto>> Create(CreatePostDto requestDto, string name);
        Task<ApiResult<PostResponseDto>> Update(CreatePostDto requestDto, string name);
        Task<ApiResult<PostResponseDto>> Detail(string Id);
        Task<ApiResult<List<PostResponseDto>>> GetAll();
        Task<ApiResult<string>> Delete(string id, string email);
        Task<ApiResult<string>> DeleteAdmin(string id);


        Task<ApiResult<string>> AddOrUnLikePost(string id, string userId);
        Task<ApiResult<string>> AddOrRemoveSavePost(string postId, string userId);
        Task<ApiResult<string>> ReportPost(ReportPostDto reportPostDto);
        Task<List<ReportPostDto>> GetReport();
        
    }
}