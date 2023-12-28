using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VNH.Application.Common.Contants;
using VNH.Application.DTOs.Catalog.Forum.Answer;
using VNH.Application.DTOs.Catalog.Notifications;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Implement.Catalog.NotificationServices;
using VNH.Application.Interfaces.Catalog.Forum;
using VNH.Domain;
using VNH.Infrastructure.Presenters;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.Forum
{
    public class AnswerService : IAnswerService
    {
        private readonly UserManager<User> _userManager;
        private readonly VietNamHistoryContext _dataContext;
        private readonly IHubContext<ChatSignalR> _answerHubContext;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public AnswerService(UserManager<User> userManager, IHubContext<ChatSignalR> answerHubContext,
        IMapper mapper,  VietNamHistoryContext vietNamHistoryContext, INotificationService notificationService)
        {
            _userManager = userManager;
            _dataContext = vietNamHistoryContext;
            _mapper = mapper;
            _answerHubContext = answerHubContext;
            _notificationService = notificationService;
        }

        private UserShortDto? GetUserShort(List<User> users, Guid? IdUser)
        {
            return users
                .Where(x => x.Id.Equals(IdUser) && !x.IsDeleted)
                .Select(x => new UserShortDto
                {
                    Id = x.Id,
                    FullName = x.Fullname,
                    Image = x.Image
                })
                .FirstOrDefault();
        }

        private List<SubAnswerQuestionDto>? GetSubAnswer(List<SubAnswer> subAnswer, List<User> users)
        {
            List<SubAnswerQuestionDto> result = new();
            foreach (var item in subAnswer)
            {
                var answer = _mapper.Map<SubAnswerQuestionDto>(item);
                answer.UserShort = GetUserShort(users, item.AuthorId);
                result.Add(answer);
            }
            return result.OrderBy(x=>x.CreatedAt).ToList();
        }

        public async Task<ApiResult<List<AnswerQuestionDto>>> GetAnswer(string questionId)
        {
            var question = await _dataContext.Questions.FirstAsync(x => x.Id.Equals(Guid.Parse(questionId)) && !x.IsDeleted);
            if (question == null)
            {
                return new ApiSuccessResult<List<AnswerQuestionDto>>();
            }

            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();
            var answerQuestion = await _dataContext.Answers.Where(x => x.QuestionId.Equals(question.Id) && !x.IsDeleted).OrderByDescending(x => x.CreatedAt).ToListAsync();
            var subAnswerQuestion = await _dataContext.SubAnswers.ToListAsync();
            var result = new List<AnswerQuestionDto>();
            var viewVotes = await _dataContext.AnswerVotes
                                .Where(x => x.QuestionId.ToString() == questionId)
                                .GroupBy(x => x.AnswerId)
                                .Select(group => new { AnswerId = group.Key, VoteCount = group.Count()})
                                .ToListAsync();

            // mostConfirm required > 10 selected
            var mostview = viewVotes.Where(x=>x.VoteCount > 10).OrderByDescending(x=>x.VoteCount).Take(1).FirstOrDefault();
            foreach (var item in answerQuestion)
            {
                var subAnswer = subAnswerQuestion.Where(x => x.PreAnswerId.Equals(item.Id)).OrderByDescending(x => x.CreatedAt).ToList();
                var answ = _mapper.Map<AnswerQuestionDto>(item);
                var voteNumber = viewVotes.FirstOrDefault(x => x.AnswerId == item.Id);
                answ.VoteNumber = voteNumber != null ? voteNumber.VoteCount : 0;
                if(mostview != null)
                {
                    if (mostview.AnswerId == item.Id && item.MostConfirm != true)
                    {
                        item.MostConfirm = true;
                        _dataContext.Answers.Update(item);
                    }
                    if (mostview.AnswerId != item.Id && item.MostConfirm == true)
                    {
                        item.MostConfirm = false;
                        _dataContext.Answers.Update(item);
                    }
                }
                answ.UserShort = GetUserShort(users, item.AuthorId);
                if(subAnswer.Count > 0)
                {
                    answ.SubAnswer = GetSubAnswer(subAnswer, users);
                }
                result.Add(answ);
            }
            await _dataContext.SaveChangesAsync();
            result = result.OrderByDescending(x=>x.VoteNumber).ToList();

            return new ApiSuccessResult<List<AnswerQuestionDto>>(result);
        }




        public async Task<ApiResult<List<AnswerQuestionDto>>> CreateAnswer(AnswerQuestionDto answer, string? id)
        {
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(answer.QuestionId)) && !x.IsDeleted);
            if (question == null)
            {
                return new ApiErrorResult<List<AnswerQuestionDto>>("Không tìm câu hỏi mà bạn trả lời, có thể nó đã bị xóa");
            }

            Answer answerQuestion = _mapper.Map<Answer>(answer);
            answerQuestion.QuestionId = question.Id;

            _dataContext.Answers.Add(answerQuestion);
            await _dataContext.SaveChangesAsync();

            var user = await _dataContext.User.FirstOrDefaultAsync(x => x.Id.ToString().Equals(id) && !x.IsDeleted);
            var noti = new NotificationDto()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                IdObject = question.Id,
                Content = ConstantNofication.AnswerTheQuestion(user?.Fullname ?? ""),
                Date = DateTime.Now,
                Url = ConstantUrl.UrlQuestionDetail,
                NotificationId = Guid.NewGuid()
            };
            await _notificationService.AddNotificationDetail(noti);

            var answers = await GetAnswer(answer.QuestionId);
            await _answerHubContext.Clients.All.SendAsync("ReceiveAnswer", answers);

            return answers;
       
        }


        public async Task<ApiResult<List<AnswerQuestionDto>>> UpdateAnswer(AnswerQuestionDto answer)
        {
            var questionAnswer = await _dataContext.Answers.FirstOrDefaultAsync(x => x.Id == answer.Id && !x.IsDeleted);
            if (questionAnswer == null)
            {
                return new ApiErrorResult<List<AnswerQuestionDto>>("Không tìm thấy câu hỏi bạn trả lời!");
            }
            questionAnswer.Content = answer.Content;
            questionAnswer.UpdatedAt = DateTime.Now;

            _dataContext.Answers.Update(questionAnswer);
            await _dataContext.SaveChangesAsync();

            var answers = await GetAnswer(answer.QuestionId);
            await _answerHubContext.Clients.All.SendAsync("ReceiveAnswer", answers);

            return answers;
        }


        public async Task<ApiResult<string>> DeteleAnswer(string id)
        {
            var answer = await _dataContext.Answers.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)) && !x.IsDeleted);
            if (answer == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy câu hỏi");
            }
            answer.IsDeleted = true;
            _dataContext.Answers.Update(answer);
            await _dataContext.SaveChangesAsync();

            var answers = await GetAnswer(answer.QuestionId.ToString());
            await _answerHubContext.Clients.All.SendAsync("ReceiveAnswer", answers);

            return new ApiSuccessResult<string>();
        }

        private async Task<ApiResult<List<SubAnswerQuestionDto>>> GetSubAnswer(string? answerId)
        {
            List<SubAnswerQuestionDto> result = new();
            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();
            var listSubAnswer = await _dataContext.SubAnswers.Where(x => x.PreAnswerId.ToString() == answerId).OrderBy(x => x.CreatedAt).ToListAsync();
            foreach (var item in listSubAnswer)
            {
                var answer = _mapper.Map<SubAnswerQuestionDto>(item);
                answer.UserShort = GetUserShort(users, item.AuthorId);
                result.Add(answer);
            }
            return new ApiSuccessResult<List<SubAnswerQuestionDto>>(result);
        }
        public async Task<ApiResult<string>> CreateSubAnswer(SubAnswerQuestionDto subAnswer)
        {
            var answer = await _dataContext.Answers.FirstOrDefaultAsync(x => x.Id.Equals(subAnswer.PreAnswerId) && !x.IsDeleted);
            if (answer == null)
            {
                return new ApiErrorResult<string>("Không tìm được câu trả lời");
            }
            SubAnswer sub = _mapper.Map<SubAnswer>(subAnswer);
            sub.PreAnswerId = answer.Id;

            _dataContext.SubAnswers.Add(sub);
            await _dataContext.SaveChangesAsync();
            var answers = await GetAnswer(answer.QuestionId.ToString());
            await _answerHubContext.Clients.All.SendAsync("ReceiveAnswer", answers);
            return new ApiSuccessResult<string>("Trả lời thành công");

        }

        public async Task<ApiResult<string>> UpdateSubAnswer(SubAnswerQuestionDto subAnswer)
        {

            var user = await _userManager.FindByIdAsync(subAnswer.AuthorId.ToString());
            var subAns = await _dataContext.SubAnswers.FirstOrDefaultAsync(x => x.Id == subAnswer.Id);
            if (subAns == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy câu trả lời!");
            }
            subAns.Content = subAnswer.Content;
            subAns.UpdatedAt = DateTime.Now;

            _dataContext.SubAnswers.Update(subAns);
            await _dataContext.SaveChangesAsync();

            var subAnswers = await GetSubAnswer(subAns.PreAnswerId.ToString());
            await _answerHubContext.Clients.All.SendAsync("ReceiveSubAnswer", subAnswers);

            return new ApiSuccessResult<string>("Đã cập nhập bình luận");
        }

       
        public async Task<ApiResult<string>> DeteleSubAnswer(string id)
        {
            var subAnswer = await _dataContext.SubAnswers.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)));
            if (subAnswer == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy câu trả lời");
            }
            _dataContext.SubAnswers.Remove(subAnswer);
            await _dataContext.SaveChangesAsync();

            var subAnswers = await GetSubAnswer(subAnswer.PreAnswerId.ToString());
            await _answerHubContext.Clients.All.SendAsync("ReceiveSubAnswer", subAnswers);

            return new ApiSuccessResult<string>();
        }


        public async Task<ApiResult<NumberReponse>> ConfirmedByQuestioner(string answerId)
        {
            var answer = await _dataContext.Answers.FirstOrDefaultAsync(x=> x.Id.Equals(Guid.Parse(answerId)) && !x.IsDeleted);
            if (answer == null)
            {
                return new ApiErrorResult<NumberReponse>("Không tìm thấy câu trả lời");
            }
            if (answer.Confirm == true)
            {
                answer.Confirm = false;
                _dataContext.Answers.Update(answer);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<NumberReponse>(new() { Check = false, Quantity = 1 });
            } else
            {
                var item = await _dataContext.Answers.FirstOrDefaultAsync(x => x.QuestionId.Equals(answer.QuestionId) && x.Confirm == true);
                if(item != null)
                {
                    item.Confirm = false;
                    answer.Confirm = true;
                    _dataContext.Answers.Update(item);
                    _dataContext.Answers.Update(answer);
                } else
                {
                    answer.Confirm = true;
                    _dataContext.Answers.Update(answer);
                }
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<NumberReponse>(new() { Check = true, Quantity = 1 });
            }
            

        }

        public async Task<ApiResult<NumberReponse>> VoteConfirmByUser(AnswerFpkDto answer)
        {
            var userVote = await _dataContext.AnswerVotes.FirstOrDefaultAsync(x => x.UserId.ToString().Equals(answer.UserId) && x.AnswerId.ToString().Equals(answer.AnswerId));
            if (userVote == null)
            {
                var answerVote = new AnswerVote()
                {
                    Id = Guid.NewGuid(),
                    QuestionId = Guid.Parse(answer.QuestionId),
                    AnswerId = Guid.Parse(answer.AnswerId),
                    UserId = Guid.Parse(answer.UserId)
                };

                var ans = await _dataContext.Answers.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id.ToString() == answer.AnswerId);
                var user = await _dataContext.User.FirstOrDefaultAsync(x => x.Id.ToString().Equals(ans.AuthorId) && !x.IsDeleted);
                var noti = new NotificationDto()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    IdObject = Guid.Parse(answer.QuestionId),
                    Content = ConstantNofication.CommentAnswer(user?.Fullname ?? ""),
                    Date = DateTime.Now,
                    Url = ConstantUrl.UrlQuestionDetail,
                    NotificationId = Guid.NewGuid()
                };
                await _notificationService.AddNotificationDetail(noti);

                _dataContext.AnswerVotes.Add(answerVote);
                await _dataContext.SaveChangesAsync();
                var numberVote = await _dataContext.AnswerVotes.Where(x => x.AnswerId == Guid.Parse(answer.AnswerId)).CountAsync();
                return new ApiSuccessResult<NumberReponse>(new() { Check = true, Quantity = numberVote });

            }
            else
            {
                _dataContext.AnswerVotes.Remove(userVote);
                await _dataContext.SaveChangesAsync();
                var numberVote = await _dataContext.AnswerVotes.Where(x => x.AnswerId == Guid.Parse(answer.AnswerId)).CountAsync();
                return new ApiSuccessResult<NumberReponse>(new() { Check = false, Quantity = numberVote });

            }
        }

        public async Task<ApiResult<NumberReponse>> GetMyVote(string answerId, string userId)
        {
            var myVotes = await _dataContext.AnswerVotes.Where(x => x.AnswerId.ToString().Equals(answerId) && x.UserId.ToString().Equals(userId)).FirstOrDefaultAsync();
            int numberVote = await _dataContext.AnswerVotes.Where(x => x.AnswerId.ToString().Equals(answerId)).CountAsync();
            if (myVotes != null)
            {
                return new ApiSuccessResult<NumberReponse>(new() { Check = true, Quantity = numberVote });
            }
            return new ApiSuccessResult<NumberReponse>(new() { Check = false, Quantity = numberVote });
        }
    }
}

