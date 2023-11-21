using VNH.Application.DTOs.Catalog.Forum.Answer;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Domain;

namespace VNH.Application.Interfaces.Catalog.Forum
{
    public interface IAnswerService
    {
        
        Task<ApiResult<List<AnswerQuestionDto>>> GetAnswer(string questionId);
        Task<ApiResult<List<AnswerQuestionDto>>> CreateAnswer(AnswerQuestionDto answer);
        Task<ApiResult<List<AnswerQuestionDto>>> UpdateAnswer(AnswerQuestionDto answer);
        Task<ApiResult<string>> DeteleAnswer(string id);


        Task<ApiResult<string>> CreateSubAnswer(SubAnswerQuestionDto subAnswer);

        Task<ApiResult<SubAnswerQuestionDto>> UpdateSubAnswer(SubAnswerQuestionDto answer);
        Task<ApiResult<string>> DeteleSubAnswer(string id);

        Task<ApiResult<int>> ConfirmOrNoConfirm(AnswerFpkDto answerFpk);


    }
}
