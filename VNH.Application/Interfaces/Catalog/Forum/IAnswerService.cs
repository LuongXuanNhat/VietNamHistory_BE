using VNH.Application.DTOs.Catalog.Forum.Answer;
using VNH.Application.DTOs.Common;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Domain;

namespace VNH.Application.Interfaces.Catalog.Forum
{
    public interface IAnswerService
    {
        
        Task<ApiResult<List<AnswerQuestionDto>>> GetAnswer(string questionId);
        Task<ApiResult<List<AnswerQuestionDto>>> CreateAnswer(AnswerQuestionDto answer, string? id);
        Task<ApiResult<List<AnswerQuestionDto>>> UpdateAnswer(AnswerQuestionDto answer);
        Task<ApiResult<string>> DeteleAnswer(string id);


        Task<ApiResult<string>> CreateSubAnswer(SubAnswerQuestionDto subAnswer);

        Task<ApiResult<string>> UpdateSubAnswer(SubAnswerQuestionDto answer);
        Task<ApiResult<string>> DeteleSubAnswer(string id);

        Task<ApiResult<NumberReponse>> ConfirmedByQuestioner(string answerId);
        Task<ApiResult<NumberReponse>> VoteConfirmByUser(AnswerFpkDto answer);
        Task<ApiResult<NumberReponse>> GetMyVote(string answerId, string userId);
    }
}
