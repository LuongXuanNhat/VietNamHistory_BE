using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Forum.Answer;
using VNH.Application.DTOs.Catalog.Forum.Question;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
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


        public AnswerService(UserManager<User> userManager, IHubContext<ChatSignalR> answerHubContext,
        IMapper mapper,
          VietNamHistoryContext vietNamHistoryContext)
        {
            _userManager = userManager;
            _dataContext = vietNamHistoryContext;
            _mapper = mapper;
            _answerHubContext = answerHubContext;
        }

        private UserShortDto? GetUserShort(List<User> users, Guid? IdUser)
        {
            return users
                .Where(x => x.Id.Equals(IdUser))
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
            return result;
        }

        public async Task<ApiResult<List<AnswerQuestionDto>>> GetAnswer(string questionId)
        {
            var question = await _dataContext.Questions.FirstAsync(x => x.Id.Equals(Guid.Parse(questionId)));
            if (question == null)
            {
                return new ApiSuccessResult<List<AnswerQuestionDto>>();
            }

            var users = await _dataContext.User.ToListAsync();
            var answerQuestion = await _dataContext.Answers.Where(x => x.QuestionId.Equals(question.Id)).ToListAsync();
            var subAnswerQuestion = await _dataContext.SubAnswers.ToListAsync();
            var result = new List<AnswerQuestionDto>();
            foreach(var item in answerQuestion)
            {
                var subAnswer = subAnswerQuestion.Where(x => x.PreAnswerId.Equals(item.Id)).ToList();
                var answ = _mapper.Map<AnswerQuestionDto>(item);
                answ.UserShort = GetUserShort(users, item.AuthorId);
                if(subAnswer.Count > 0)
                {
                    answ.SubAnswer = GetSubAnswer(subAnswer, users);
                }
                result.Add(answ);
            }
            return new ApiSuccessResult<List<AnswerQuestionDto>>(result);
        }




        public async Task<ApiResult<List<AnswerQuestionDto>>> CreateAnswer(AnswerQuestionDto answer)
        {
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(answer.QuestionId)));
            if (question == null)
            {
                return new ApiErrorResult<List<AnswerQuestionDto>>("Không tìm câu hỏi mà bạn trả lời, có thể nó đã bị xóa");
            }

            Answer answerQuestion = _mapper.Map<Answer>(answer);
            answerQuestion.QuestionId = question.Id;
            _dataContext.Answers.Add(answerQuestion);
            await _dataContext.SaveChangesAsync();

            var answers = await GetAnswer(answer.QuestionId);
            await _answerHubContext.Clients.All.SendAsync("ReceiveAnswer", answers);

            return answers;
       
        }


        public async Task<ApiResult<List<AnswerQuestionDto>>> UpdateAnswer(AnswerQuestionDto answer)
        {
            var questionAnswer = await _dataContext.Answers.FirstOrDefaultAsync(x => x.Id == answer.Id);
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
            var answer = await _dataContext.Answers.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)));
            if (answer == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy câu hỏi");
            }
            _dataContext.Answers.Remove(answer);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<string>();
        }


        public async Task<ApiResult<string>> CreateSubAnswer(SubAnswerQuestionDto subAnswer)
        {
            var answer = await _dataContext.Answers.FirstOrDefaultAsync(x => x.Id.Equals(subAnswer.PreAnswerId));
            if (answer == null)
            {
                return new ApiErrorResult<string>("Không tìm được câu trả lời");
            }
            SubAnswer sub = _mapper.Map<SubAnswer>(subAnswer);
            sub.PreAnswerId = answer.Id;
            _dataContext.SubAnswers.Add(sub);
            await _dataContext.SaveChangesAsync();
            var answers = await GetAnswer( answer.QuestionId.ToString());
            await _answerHubContext.Clients.All.SendAsync("ReceiveAnswer", answers);
            return new ApiSuccessResult<string>("Trả lời thành công");
        }


    
        public async Task<ApiResult<SubAnswerQuestionDto>> UpdateSubAnswer(SubAnswerQuestionDto subAnswer)
        {

            var user = await _userManager.FindByIdAsync(subAnswer.AuthorId.ToString());
            var subAns = await _dataContext.SubAnswers.FirstOrDefaultAsync(x => x.Id == subAnswer.Id);
            var answer = await _dataContext.Answers.FirstOrDefaultAsync(x => x.Id.Equals(subAnswer.PreAnswerId));
            if (subAns == null)
            {
                return new ApiErrorResult<SubAnswerQuestionDto>("Không tìm thấy câu trả lời!");
            }
            subAns.Content = subAnswer.Content;
            subAns.UpdatedAt = DateTime.Now;

            _dataContext.SubAnswers.Update(subAns);
            await _dataContext.SaveChangesAsync();
            var subAnsResponse = _mapper.Map<SubAnswerQuestionDto>(subAns);

            var answers = await GetAnswer(answer.QuestionId.ToString());
            await _answerHubContext.Clients.All.SendAsync("ReceiveAnswer", answers);


            return new ApiSuccessResult<SubAnswerQuestionDto>(subAnsResponse);
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
            return new ApiSuccessResult<string>();
        }


        public async Task<ApiResult<int>> ConfirmOrNoConfirm(AnswerFpkDto answerFpk)
        {
            var answer = await _dataContext.Answers.FirstOrDefaultAsync(x=> x.Id.Equals(Guid.Parse(answerFpk.AnswerId)));
            if(answer == null)
            {
                return new ApiErrorResult<int>("Không tìm thấy câu trả lời");
            }
            var check = _dataContext.AnswerVotes.Where(x => x.AnswerId == answer.Id && x.UserId == Guid.Parse(answerFpk.UserId)).FirstOrDefault();
            var mess = "";
            var voteNumber = await _dataContext.AnswerVotes.Where(x=>x.AnswerId == answer.Id).CountAsync(); 
            if(check is null)
            {
                var vote = new AnswerVote()
                {
                    Id = Guid.NewGuid(),
                    AnswerId = answer.Id,
                    UserId = Guid.Parse(answerFpk.UserId),
                };
                _dataContext.AnswerVotes.Add(vote);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<int>(voteNumber + 1);
            }
            else
            {
                _dataContext.AnswerVotes.Remove(check);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<int>(voteNumber - 1);
            }
        }
    }
}

