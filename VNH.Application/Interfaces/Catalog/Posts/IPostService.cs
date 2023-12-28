using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common;
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


        Task<ApiResult<NumberReponse>> AddOrUnLikePost(PostFpkDto postFpk);
        Task<ApiResult<NumberReponse>> AddOrRemoveSavePost(PostFpkDto postFpk);
        Task<ApiResult<string>> ReportPost(ReportPostDto reportPostDto);
        Task<ApiResult<List<ReportPostDto>>> GetReport();
        Task<ApiResult<NumberReponse>> GetLike(PostFpkDto postFpk);
        Task<ApiResult<NumberReponse>> GetSave(PostFpkDto postFpk);
        Task<ApiResult<List<PostResponseDto>>> GetPostByTag(string tag);
        Task<ApiResult<List<CommentPostDto>>> GetComment(string postId);
        Task<ApiResult<List<CommentPostDto>>> CreateComment(CommentPostDto comment, string userId);
        Task<ApiResult<List<CommentPostDto>>> UpdateComment(CommentPostDto comment);
        Task<ApiResult<List<CommentPostDto>>> DeteleComment(string id);
        Task<ApiResult<List<PostResponseDto>>> GetMyPostSaved(string id);
        Task<ApiResult<List<PostResponseDto>>> GetMyPost(string id);
        Task<ApiResult<List<PostResponseDto>>> SearchPosts(string keyWord);
        Task<ApiResult<List<PostResponseDto>>> GetAllMobile();
        Task<ApiResult<List<PostResponseDto>>> GetRandomPost(int quantity);
        Task<ApiResult<List<PostResponseDto>>> FindByTopic(string topicName);
    }
}