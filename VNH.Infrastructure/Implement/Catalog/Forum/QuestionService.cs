using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using VNH.Application.DTOs.Catalog.Forum.Question;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Catalog.Forum;
using VNH.Application.Interfaces.Common;
using VNH.Domain;
using VNH.Domain.Entities;
using VNH.Infrastructure.Implement.Common;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.Forum
{
    public class QuestionService : IQuestionService
    {

        private readonly UserManager<User> _userManager;
        private readonly VietNamHistoryContext _dataContext;
        private readonly IMapper _mapper;

        public QuestionService(UserManager<User> userManager,
            IMapper mapper,
           VietNamHistoryContext vietNamHistoryContext)
        {
            _userManager = userManager;
            _dataContext = vietNamHistoryContext;
            _mapper = mapper;
        }


        public async Task<ApiResult<QuestionResponseDto>> Create(CreateQuestionDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var question = _mapper.Map<Question>(requestDto);
            question.Id = Guid.NewGuid();
            question.CreateAt = DateTime.Now;
            question.AuthorId = user.Id;
            question.ViewNumber = 0; 

            string formattedDateTime = question.CreateAt.ToString("HHmmss.fff") + HandleCommon.GenerateRandomNumber().ToString();
            var Id = HandleCommon.SanitizeString(requestDto.Title);
            question.SubId = Id + "-" + formattedDateTime;
            try
            {
                _dataContext.Questions.Add(question);
                await _dataContext.SaveChangesAsync();
                if (!requestDto.Tag.IsNullOrEmpty())
                {
                    foreach (var item in requestDto.Tag)
                    {
                        var tag = new Tag()
                        {
                            Id = Guid.NewGuid(),
                            Name = item
                        };
                        var questiontag = new QuestionTag()
                        {
                            Id = Guid.NewGuid(),
                            TagId = tag.Id,
                            QuestionId = question.Id
                        };
                        _dataContext.Tags.Add(tag);
                        _dataContext.QuestionTags.Add(questiontag);
                    }
                    await _dataContext.SaveChangesAsync();
                }

                var result = await Detail(question.Id.ToString());
                return result;

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<QuestionResponseDto>("Lỗi lưu câu hỏi : " + ex.Message);
            }


        }

        public async Task<ApiResult<QuestionResponseDto>> Update(CreateQuestionDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);

            var updateQuestion = _dataContext.Questions.First(x => x.Id.Equals(requestDto.Id));
            if (updateQuestion is null)
            {
                return new ApiErrorResult<QuestionResponseDto>("Lỗi :Câu hỏi không được cập nhập (không tìm thấy bài viết)");
            }
           
            updateQuestion.UpdateAt = DateTime.Now;
            updateQuestion.Content = requestDto.Content;
            updateQuestion.Title = requestDto.Title;

            string formattedDateTime = updateQuestion.CreateAt.ToString("HHmmss.fff") + HandleCommon.GenerateRandomNumber().ToString();
            var Id = HandleCommon.SanitizeString(requestDto.Title);
            updateQuestion.SubId = Id + "-" + formattedDateTime;
            try
            {
                _dataContext.Questions.Update(updateQuestion);
                await _dataContext.SaveChangesAsync();

                var tagOfQuestion = await _dataContext.QuestionTags.Where(x => x.QuestionId.Equals(updateQuestion.Id)).ToListAsync();
                _dataContext.QuestionTags.RemoveRange(tagOfQuestion);

                foreach (var item in requestDto.Tag)
                {
                    var tag = new Tag()
                    {
                        Id = Guid.NewGuid(),
                        Name = item
                    };
                    var questionTag = new QuestionTag()
                    {
                        QuestionId = requestDto.Id,
                        TagId = tag.Id
                    };
                    _dataContext.QuestionTags.Add(questionTag);
                    _dataContext.Tags.Add(tag);
                }
                await _dataContext.SaveChangesAsync();

                var question = await Detail(updateQuestion.Id.ToString());


                return question;
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<QuestionResponseDto>("Lỗi lưu câu h : " + ex.Message);
            }
        }

        public async Task<ApiResult<QuestionResponseDto>> Detail(string Id) 
        {
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(Id)));
            if (question is null)
            {
                return new ApiErrorResult<QuestionResponseDto>("Không tìm thấy câu hỏi");
            }
            var user = await _userManager.FindByIdAsync(question.AuthorId.ToString());
            var questionResponse = _mapper.Map<QuestionResponseDto>(question);

            var listQuestionTag = await _dataContext.QuestionTags.Where(x => x.QuestionId.Equals(Guid.Parse(questionResponse.Id))).Select(x => x.TagId).ToListAsync();
            var tags = await _dataContext.Tags
                                    .Where(x => listQuestionTag.Any(TagId => TagId == x.Id))
                                    .ToListAsync();
            foreach (var tag in tags)
            {
                questionResponse.Tags.Add(new()
                {
                    Name = tag.Name,
                    Id = tag.Id,
                });
            }
            questionResponse.UserShort = new()
            {
                FullName = user.Fullname,
                Id = user.Id,
                Image = user.Image
            };
            question.ViewNumber += 1;
            questionResponse.SaveNumber = await _dataContext.QuestionSaves.Where(x => x.QuestionId.Equals(question.Id)).CountAsync();
            questionResponse.CommentNumber = await _dataContext.Answers.Where(x => x.QuestionId.Equals(question.Id)).CountAsync();

            _dataContext.Questions.Update(question);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<QuestionResponseDto>(questionResponse);
        }

        public async Task<ApiResult<List<QuestionResponseDto>>> GetAll()
        {
            var questions = await _dataContext.Questions.ToListAsync();
            var users = await _dataContext.User.ToListAsync();

            var result = new List<QuestionResponseDto>();
            foreach (var item in questions)
            {
                var question = _mapper.Map<QuestionResponseDto>(item);
                question.CommentNumber = await _dataContext.Answers.Where(x => x.QuestionId.ToString() == question.Id).CountAsync();
                question.SaveNumber    = await _dataContext.QuestionSaves.Where(x => x.QuestionId.ToString() == question.Id).CountAsync();
                question.LikeNumber    = await _dataContext.QuestionLikes.Where(x => x.QuestionId.ToString() == question.Id).CountAsync();

                var userShort = users.First(x => x.Id == item.AuthorId);
                if (userShort is not null)
                {
                    question.UserShort.FullName = userShort.Fullname;
                    question.UserShort.Id = userShort.Id;
                    question.UserShort.Image = userShort.Image;
                }
                result.Add(question);
            }

            return new ApiSuccessResult<List<QuestionResponseDto>>(result);
        }

        public async Task<ApiResult<string>> Delete(string id, string userId)
        {
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)) && x.AuthorId.Equals(Guid.Parse(userId)));
            if (question is null)
            {
                return new ApiErrorResult<string>("Không tìm thấy câu hỏi");
            }
          
            _dataContext.Questions.Remove(question);

            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<string>("Đã xóa câu hỏi");
        }

        public async Task<ApiResult<int>> AddOrRemoveSaveQuestion(QuestionFpkDto questionFpk)
        {
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(questionFpk.QuestionId)));
            if (question is null)
            {
                return new ApiErrorResult<int>("Không tìm thấy câu hỏi");
            }
            var check = _dataContext.QuestionSaves.Where(x => x.QuestionId == question.Id && x.UserId == Guid.Parse(questionFpk.UserId)).FirstOrDefault();
            var mess = "";
            var saveNumber = await _dataContext.QuestionSaves.Where(x => x.QuestionId == question.Id).CountAsync();
            if (check is null)
            {
                var save = new QuestionSave()
                {
                    Id = Guid.NewGuid(),
                    QuestionId = question.Id,
                    UserId = Guid.Parse(questionFpk.UserId)
                };
                _dataContext.QuestionSaves.Add(save);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<int>(saveNumber + 1);
            }
            else
            {
                _dataContext.QuestionSaves.Remove(check);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<int>(saveNumber - 1);
            }


        }
        public async Task<ApiResult<bool>> GetSave(QuestionFpkDto questionFpk)
        {
            var question = _dataContext.Questions.First(x => x.Id.Equals(Guid.Parse(questionFpk.QuestionId)));
            var check = await _dataContext.QuestionSaves.Where(x => x.QuestionId.Equals(question.Id) && x.UserId == Guid.Parse(questionFpk.UserId)).FirstOrDefaultAsync();
            var reuslt = check != null;
            return new ApiSuccessResult<bool>(reuslt);
        }
        public async Task<ApiResult<List<string>>> GetAllTag(int numberTag)
        {
            var tags = await _dataContext.Tags
                    .GroupBy(x => x.Name)
                    .Select(group => new { Name = group.Key, Count = group.Sum(t => 1) })
                     .OrderByDescending(tagName => tagName.Count)
                    .Take(numberTag)
                       .Select(group => group.Name)
                    .ToListAsync();
            return new ApiSuccessResult<List<string>>(tags);
        }


        public async Task<ApiResult<List<QuestionResponseDto>>> GetQuestionByTag(string tag)
        {
            var questions = await _dataContext.Questions
                            .Where(question => _dataContext.QuestionTags
                                .Any(questionTag => _dataContext.Tags
                                .Any(tagEntity => tagEntity.Name.ToLower().Contains(tag.ToLower()) && tagEntity.Id == questionTag.TagId)
                                && questionTag.QuestionId == question.Id))
                                .ToListAsync();
            if (questions.IsNullOrEmpty())
            {
                return new ApiSuccessResult<List<QuestionResponseDto>>(new List<QuestionResponseDto>());
            }
            var users = await _dataContext.User.ToListAsync();

            var result = new List<QuestionResponseDto>();
            foreach (var item in questions)
            {
                var ques = _mapper.Map<QuestionResponseDto>(item);
                var userShort = users.First(x => x.Id == item.AuthorId);
                if (userShort is not null)
                {
                    ques.UserShort.FullName = userShort.Fullname;
                    ques.UserShort.Id = userShort.Id;
                    ques.UserShort.Image = userShort.Image;
                }
                result.Add(ques);
            }
            return new ApiSuccessResult<List<QuestionResponseDto>>(result);
        }

        public async Task<ApiResult<QuestionResponseDto>> SubDetail(string subId)
        {
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x => x.SubId.Equals(subId));
            if (question is null)
            {
                return new ApiErrorResult<QuestionResponseDto>("Không tìm thấy câu hỏi");
            }
            var user = await _userManager.FindByIdAsync(question.AuthorId.ToString());
            var questionResponse = _mapper.Map<QuestionResponseDto>(question);

            var listQuestionTag = await _dataContext.QuestionTags.Where(x => x.QuestionId.Equals(Guid.Parse(questionResponse.Id))).Select(x => x.TagId).ToListAsync();
            var tags = await _dataContext.Tags
                                    .Where(x => listQuestionTag.Any(TagId => TagId == x.Id))
                                    .ToListAsync();
            foreach (var tag in tags)
            {
                questionResponse.Tags.Add(new()
                {
                    Name = tag.Name,
                    Id = tag.Id,
                });
            }
            questionResponse.UserShort = new()
            {
                FullName = user.Fullname,
                Id = user.Id,
                Image = user.Image
            };
            question.ViewNumber += 1;
            questionResponse.SaveNumber = await _dataContext.QuestionSaves.Where(x => x.QuestionId.Equals(question.Id)).CountAsync();
            questionResponse.CommentNumber = await _dataContext.Answers.Where(x => x.QuestionId.Equals(question.Id)).CountAsync();

            _dataContext.Questions.Update(question);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<QuestionResponseDto>(questionResponse);
        }
    }


}
