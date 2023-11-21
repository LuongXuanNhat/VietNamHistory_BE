

using VNH.Application.DTOs.Catalog.Forum.Question;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Interfaces.Catalog.Forum
{
    public interface IQuestionService
    {
        Task<ApiResult<QuestionResponseDto>> Create(CreateQuestionDto requestDto, string name);
        Task<ApiResult<QuestionResponseDto>> Update(CreateQuestionDto requestDto, string name);

        Task<ApiResult<QuestionResponseDto>> Detail(string Id);
        Task<ApiResult<List<QuestionResponseDto>>> GetAll();
        Task<ApiResult<string>> Delete(string id, string email);
        Task<ApiResult<int>> AddOrRemoveSaveQuestion(QuestionFpkDto questionFpk);

        Task<ApiResult<bool>> GetSave(QuestionFpkDto questionFpk);

        Task<ApiResult<List<string>>> GetAllTag(int numberTag);
        Task<ApiResult<List<QuestionResponseDto>>> GetQuestionByTag(string tag);


    }
}
