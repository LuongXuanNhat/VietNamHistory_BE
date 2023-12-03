

using VNH.Application.DTOs.Catalog.Forum.Question;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common;
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
        Task<ApiResult<NumberReponse>> AddOrRemoveSaveQuestion(QuestionFpkDto questionFpk);

        Task<ApiResult<NumberReponse>> GetSave(QuestionFpkDto questionFpk);

        Task<ApiResult<List<string>>> GetAllTag(int numberTag);
        Task<ApiResult<List<QuestionResponseDto>>> GetQuestionByTag(string tag);
        Task<ApiResult<QuestionResponseDto>> SubDetail(string subId);
        Task<ApiResult<NumberReponse>> GetLike(QuestionFpkDto questionFpk);
        Task<ApiResult<NumberReponse>> AddOrUnLikeQuestion(QuestionFpkDto questionFpk);
        Task<ApiResult<List<QuestionResponseDto>>> GetMyQuestion(string id);
        Task<ApiResult<string>> ReportQuestion(ReportQuestionDto reportquestionDto);
        Task<ApiResult<List<ReportQuestionDto>>> GetReport();
        Task<ApiResult<List<QuestionResponseDto>>> SearchQuestions(string keyWord);
        Task<ApiResult<List<QuestionResponseDto>>> GetMyQuestionSaved(string id);
    }
}
