using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.ExamHistory;
using VNH.Application.DTOs.Catalog.MultipleChoiceDto;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Catalog.ExamHistory;
using VNH.Domain;
using VNH.Domain.Entities;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.ExamHistorys
{
    public class ExamHistoryService : IExamHistoryService
    {
        private readonly UserManager<User> _userManager;
        private readonly VietNamHistoryContext _dataContext;
        private readonly IMapper _mapper;

        public ExamHistoryService(UserManager<User> userManager, VietNamHistoryContext dataContext, IMapper mapper)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ApiResult<ExamHistoryResponseDto>>  Create(CreateExamHistoryDto requestDto,string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var check = await _dataContext.ExamHistories.Where(x => x.UserId.Equals(user.Id) && x.MultipleChoiceId == requestDto.MultipleChoiceId ).FirstOrDefaultAsync();
            if (check != null)
            {
                return await Update(requestDto, name);
            }
            var examhistory = _mapper.Map<ExamHistory>(requestDto);
            var multiples = await _dataContext.MultipleChoices.FirstOrDefaultAsync(x => x.Id.Equals(requestDto.MultipleChoiceId));
            examhistory.Id = Guid.NewGuid();
            examhistory.MultipleChoiceId = requestDto.MultipleChoiceId;
            examhistory.UserId = requestDto.UserId;
            examhistory.StarDate = requestDto.StarDate;
            examhistory.Scores = float.Parse(requestDto.Scores.ToString());
            examhistory.CompletionTime = requestDto.CompletionTime;
            try
            {
                _dataContext.ExamHistories.Add(examhistory);
                await _dataContext.SaveChangesAsync();
                var response = _mapper.Map<ExamHistoryResponseDto>(examhistory);
                response.UserShortDto = new()
                {
                    FullName = user.Fullname,
                    Id = user.Id,
                    Image = user.Image,
                };

                response.multipleChoiceResponseDto = new()
                {
                    Id = multiples.Id.ToString(),
                    Title = multiples.Title,
                    Description = multiples.Description,
                    CreatedAt = multiples.CreatedAt,
                    UpdatedAt = multiples.UpdatedAt,
                    WorkTime = multiples.WorkTime,
                    UserShort = new()
                    {
                        Id = user.Id,
                        FullName = user.Fullname,
                        Image = user.Image,
                    }
                };

                return new ApiSuccessResult<ExamHistoryResponseDto>(response);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<ExamHistoryResponseDto>("Có lỗi xảy ra : " + ex.Message);
            }
        }
        

        public async Task<ApiResult<ExamHistoryResponseDto>> Update(CreateExamHistoryDto requestDto,string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var updateExamHistory = await _dataContext.ExamHistories.FirstOrDefaultAsync(x => x.MultipleChoiceId.Equals(requestDto.MultipleChoiceId) && x.UserId.Equals(requestDto.UserId));
            if (updateExamHistory is null)
            {
                return new ApiErrorResult<ExamHistoryResponseDto>("Có lỗi xảy ra khi cập nhập kết quả!");
            }
            updateExamHistory.StarDate = DateTime.Now;
            updateExamHistory.Scores = float.Parse(requestDto.Scores.ToString());
            updateExamHistory.CompletionTime = requestDto.CompletionTime;

            try
            {
                _dataContext.ExamHistories.Update(updateExamHistory);
                await _dataContext.SaveChangesAsync();
                var response = _mapper.Map<ExamHistoryResponseDto>(updateExamHistory);

                return new ApiSuccessResult<ExamHistoryResponseDto>(response);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<ExamHistoryResponseDto>("Lỗi : " + ex.Message);
            }
        }


        public async Task<ApiResult<List<ExamHistoryResponseDto>>> GetMyExamHistory(string id)
        {
            Guid userId = Guid.Parse(id);
            var user = await _dataContext.User.Where(x => !x.IsDeleted && x.Id == userId).FirstOrDefaultAsync();
            if(user == null)
            {
                return new ApiErrorResult<List<ExamHistoryResponseDto>>("Tài khoản không tồn tại");
            }
            var examHistories = await _dataContext.ExamHistories.Where(x=>x.UserId.Equals(userId)).ToListAsync();
            //var examHistories = await (
            //    from examhis in _dataContext.ExamHistories
            //    join ex in _dataContext.ExamHistories on examhis.UserId equals userId
            //    where examhis.UserId == userId
            //    select ex
            //    ).ToListAsync();

            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();
            var result = new List<ExamHistoryResponseDto>();
            foreach(var item in examHistories)
            {
                var examhistory = _mapper.Map<ExamHistoryResponseDto>(item);
                var userShort = users.FirstOrDefault(x=>x.Id == item.UserId);
                var multis = await _dataContext.MultipleChoices.FirstOrDefaultAsync(x => x.Id.Equals(item.MultipleChoiceId));

                var exam = _mapper.Map<MultipleChoiceResponseDto>(multis);
                if (userShort is not null)
                {
                    examhistory.UserShortDto.FullName = userShort.Fullname;
                    examhistory.UserShortDto.Id = userShort.Id;
                    examhistory.UserShortDto.Image = userShort.Image;
                }
                examhistory.numberQuiz = await _dataContext.Quizzes.Where(x=>x.MultipleChoiceId == multis.Id).CountAsync();
                examhistory.multipleChoiceResponseDto = exam;

                result.Add(examhistory);
            }
            return new ApiSuccessResult<List<ExamHistoryResponseDto>>(result);

        }

        public async Task<ApiResult<List<ExamHistoryResponseDto>>> GetExamHistory(string examId)
        {
            var examhistories = await _dataContext.ExamHistories.Where(x => x.MultipleChoiceId.ToString().Equals(examId)).ToListAsync();

            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();
            var result = new List<ExamHistoryResponseDto>();
            foreach (var item in examhistories)
            {
                var examhistory = _mapper.Map<ExamHistoryResponseDto>(item);
                var userShort = users.FirstOrDefault(x => x.Id == item.UserId);
                var multis = await _dataContext.MultipleChoices.FirstOrDefaultAsync(x => x.Id.Equals(item.MultipleChoiceId));

                var exam = _mapper.Map<MultipleChoiceResponseDto>(multis);
                if (userShort is not null)
                {
                    examhistory.UserShortDto.FullName = userShort.Fullname;
                    examhistory.UserShortDto.Id = userShort.Id;
                    examhistory.UserShortDto.Image = userShort.Image;
                }
                examhistory.numberQuiz = await _dataContext.Quizzes.Where(x => x.MultipleChoiceId == multis.Id).CountAsync();
                examhistory.multipleChoiceResponseDto = exam;

                result.Add(examhistory);
            }

            return new ApiSuccessResult<List<ExamHistoryResponseDto>>(result);
        }
    }
}
