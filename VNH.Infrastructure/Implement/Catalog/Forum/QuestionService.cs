using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using VNH.Application.Common.Contants;
using VNH.Application.DTOs.Catalog.Forum.Question;
using VNH.Application.DTOs.Catalog.Notifications;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Implement.Catalog.NotificationServices;
using VNH.Application.Interfaces.Catalog.Forum;
using VNH.Domain;
using VNH.Infrastructure.Implement.Common;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.Forum
{
    public class QuestionService : IQuestionService
    {

        private readonly UserManager<User> _userManager;
        private readonly VietNamHistoryContext _dataContext;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public QuestionService(UserManager<User> userManager,
            IMapper mapper, INotificationService notificationService,
           VietNamHistoryContext vietNamHistoryContext)
        {
            _userManager = userManager;
            _dataContext = vietNamHistoryContext;
            _mapper = mapper;
            _notificationService = notificationService;
        }


        public async Task<ApiResult<QuestionResponseDto>> Create(CreateQuestionDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var question = _mapper.Map<Question>(requestDto);
            question.Id = Guid.NewGuid();
            question.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")); ;
            question.AuthorId = user.Id;
            question.ViewNumber = 0; 

            string formattedDateTime = question.CreatedAt.ToString("HHmmss.fff") + HandleCommon.GenerateRandomNumber().ToString();
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

            var updateQuestion = _dataContext.Questions.First(x => x.Id.Equals(requestDto.Id) && !x.IsDeleted);
            if (updateQuestion is null)
            {
                return new ApiErrorResult<QuestionResponseDto>("Lỗi :Câu hỏi không được cập nhập (không tìm thấy bài viết)");
            }
           
            updateQuestion.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")); ;
            updateQuestion.Content = requestDto.Content;
            updateQuestion.Title = requestDto.Title;

            string formattedDateTime = updateQuestion.CreatedAt.ToString("HHmmss.fff") + HandleCommon.GenerateRandomNumber().ToString();
            var Id = HandleCommon.SanitizeString(requestDto.Title);
            updateQuestion.SubId = Id + "-" + formattedDateTime;
            try
            {
                _dataContext.Questions.Update(updateQuestion);
                await _dataContext.SaveChangesAsync();

                var tagOfQuestion = await _dataContext.QuestionTags.Where(x => x.QuestionId.Equals(updateQuestion.Id)).ToListAsync();
                _dataContext.QuestionTags.RemoveRange(tagOfQuestion);

                if (requestDto.Tag != null)
                {
                    foreach (var item in requestDto.Tag)
                    {
                        var tag = new Tag()
                        {
                            Id = Guid.NewGuid(),
                            Name = item
                        };
                        var questionTag = new QuestionTag()
                        {
                            Id = Guid.NewGuid(),
                            QuestionId = requestDto.Id,
                            TagId = tag.Id
                        };
                        _dataContext.QuestionTags.Add(questionTag);
                        _dataContext.Tags.Add(tag);
                    }
                    await _dataContext.SaveChangesAsync();
                }

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
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(Id)) && !x.IsDeleted);
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
            questionResponse.ViewNumber += 1;
            questionResponse.SaveNumber = await _dataContext.QuestionSaves.Where(x => x.QuestionId.Equals(question.Id)).CountAsync();
            questionResponse.CommentNumber = await _dataContext.Answers.Where(x => x.QuestionId.Equals(question.Id) && !x.IsDeleted).CountAsync();
            questionResponse.LikeNumber = await _dataContext.QuestionLikes.Where(x => x.QuestionId.Equals(question.Id)).CountAsync();

            _dataContext.Questions.Update(question);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<QuestionResponseDto>(questionResponse);
        }

        public async Task<ApiResult<List<QuestionResponseDto>>> GetAll()
        {
            var questions = await _dataContext.Questions.Where(x=> !x.IsDeleted).OrderByDescending(x => x.CreatedAt).ToListAsync();
            var users = await _dataContext.User.ToListAsync();

            var result = new List<QuestionResponseDto>();
            foreach (var item in questions)
            {
                var question = _mapper.Map<QuestionResponseDto>(item);
                question.CommentNumber = await _dataContext.Answers.Where(x => x.QuestionId.ToString() == question.Id && !x.IsDeleted).CountAsync();
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

            question.IsDeleted = true;
            _dataContext.Questions.Update(question);

            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<string>("Đã xóa câu hỏi");
        }

        public async Task<ApiResult<NumberReponse>> AddOrRemoveSaveQuestion(QuestionFpkDto questionFpk)
        {
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(questionFpk.QuestionId)) && !x.IsDeleted);
            if (question is null)
            {
                return new ApiErrorResult<NumberReponse>("Không tìm thấy câu hỏi");
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
                return new ApiSuccessResult<NumberReponse>(new() { Check = true, Quantity = saveNumber + 1 });
            }
            else
            {
                _dataContext.QuestionSaves.Remove(check);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<NumberReponse>(new() { Check = true, Quantity = saveNumber - 1 });
            }


        }
        public async Task<ApiResult<NumberReponse>> GetSave(QuestionFpkDto questionFpk) 
        {
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(questionFpk.QuestionId)) && !x.IsDeleted);
            var number = await _dataContext.QuestionSaves.Where(x => x.QuestionId.Equals(question.Id)).CountAsync();
            var check = await _dataContext.QuestionSaves.Where(x => x.QuestionId.Equals(question.Id) && x.UserId == Guid.Parse(questionFpk.UserId)).FirstOrDefaultAsync();
            return new ApiSuccessResult<NumberReponse>(new() { Check = check != null, Quantity = number }) ;
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
                                && questionTag.QuestionId == question.Id) && !question.IsDeleted)
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
            var question = await _dataContext.Questions.Where(x=> !x.IsDeleted).FirstOrDefaultAsync(x => x.SubId.Equals(subId));
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
            questionResponse.ViewNumber += 1;
            questionResponse.SaveNumber = await _dataContext.QuestionSaves.Where(x => x.QuestionId.Equals(question.Id)).CountAsync();
            questionResponse.CommentNumber = await _dataContext.Answers.Where(x => x.QuestionId.Equals(question.Id)).CountAsync();

            _dataContext.Questions.Update(question);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<QuestionResponseDto>(questionResponse);
        }

        public async Task<ApiResult<NumberReponse>> GetLike(QuestionFpkDto questionFpk)
        {
            var question = _dataContext.Questions.First(x => x.SubId.Equals(questionFpk.QuestionId) && !x.IsDeleted);
            var check = await _dataContext.QuestionLikes.Where(x => x.QuestionId.Equals(question.Id) && x.UserId == Guid.Parse(questionFpk.UserId)).FirstOrDefaultAsync();
            var number = await _dataContext.QuestionLikes.Where(x => x.QuestionId.Equals(question.Id)).CountAsync();
            return new ApiSuccessResult<NumberReponse>(new() { Check = (check != null), Quantity = number });
        }

        public async Task<ApiResult<NumberReponse>> AddOrUnLikeQuestion(QuestionFpkDto questionFpk)
        {
            var question = await _dataContext.Questions.FirstOrDefaultAsync(x => x.SubId.Equals(questionFpk.QuestionId) && !x.IsDeleted);
            if (question is null)
            {
                return new ApiErrorResult<NumberReponse>("Không tìm thấy câu hỏi");
            }
            var check = _dataContext.QuestionLikes.Where(x => x.QuestionId == question.Id && x.UserId == Guid.Parse(questionFpk.UserId)).FirstOrDefault();
            var likeNumber = await _dataContext.QuestionLikes.Where(x => x.QuestionId == question.Id).CountAsync();
            if (check is null)
            {
                var user = await _dataContext.User.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == question.AuthorId);
                var like = new QuestionLike()
                {
                    Id = Guid.NewGuid(),
                    QuestionId = question.Id,
                    UserId = Guid.Parse(questionFpk.UserId)
                };
                var noti = new NotificationDto()
                {
                    Id = Guid.NewGuid(),
                    UserId = question.AuthorId ?? Guid.NewGuid(),
                    IdObject = question.Id,
                    Content = ConstantNofication.LikeQuestion(user?.Fullname ?? ""),
                    Date = DateTime.Now,
                    Url = ConstantUrl.UrlQuestionDetail,
                    NotificationId = Guid.NewGuid()
                };
                await _notificationService.AddNotificationDetail(noti);

                _dataContext.QuestionLikes.Add(like);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<NumberReponse>(new() { Check = true, Quantity = likeNumber + 1 });
            }
            else
            {
                _dataContext.QuestionLikes.Remove(check);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<NumberReponse>(new() { Check = false, Quantity = likeNumber - 1 });

            }
        }

        public async Task<ApiResult<List<QuestionResponseDto>>> GetMyQuestion(string id)
        {
            var questions = await _dataContext.Questions.Where(x => x.AuthorId.Equals(Guid.Parse(id)) && !x.IsDeleted).ToListAsync();
            var user = await _dataContext.User.Where(x => !x.IsDeleted && x.Id.ToString().Equals(id)).FirstOrDefaultAsync();
            var result = new List<QuestionResponseDto>();
            foreach (var item in questions)
            {
                var question = _mapper.Map<QuestionResponseDto>(item);
                question.SaveNumber = await _dataContext.QuestionSaves.Where(x => x.QuestionId.Equals(item.Id)).CountAsync();
                question.CommentNumber = await _dataContext.Answers.Where(x => x.QuestionId.Equals(item.Id)).CountAsync();
                question.LikeNumber = await _dataContext.QuestionLikes.Where(x => x.QuestionId.Equals(item.Id)).CountAsync();
                if (user is not null)
                {
                    question.UserShort.FullName = user.Fullname;
                    question.UserShort.Id = user.Id;
                    question.UserShort.Image = user.Image;
                }
                result.Add(question);
            }

            return new ApiSuccessResult<List<QuestionResponseDto>>(result);
        }

        public async Task<ApiResult<string>> ReportQuestion(ReportQuestionDto reportquestionDto)
        {
            var question = _dataContext.Questions.FirstOrDefault(x => x.SubId.Equals(reportquestionDto.QuestionId) && !x.IsDeleted);
            if (question == null)
            {
                return new ApiErrorResult<string>("Bài viết không tồn tại");
            }
            var reportQuestion = _mapper.Map<QuestionReportDetail>(reportquestionDto);
            reportQuestion.Id = Guid.NewGuid();
            reportQuestion.QuestionId = question.Id;
            _dataContext.QuestionReportDetails.Add(reportQuestion);
            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<string>("Đã gửi báo cáo đến kiểm duyệt viên! Chúng tôi sẽ phản hồi bạn sớm nhất có thể! Xin cảm ơn.");
        }

        public async Task<ApiResult<List<ReportQuestionDto>>> GetReport()
        {
            var reportPost = await _dataContext.QuestionReportDetails
                .OrderByDescending(x => x.ReportDate)
                .ToListAsync();
            var results = new List<ReportQuestionDto>();
            foreach (var item in reportPost)
            {
                results.Add(_mapper.Map<ReportQuestionDto>(item));
            }
            return new ApiSuccessResult<List<ReportQuestionDto>>(results);
        }

        public async Task<ApiResult<List<QuestionResponseDto>>> SearchQuestions(string keyWord)
        {
            if (keyWord.StartsWith("#"))
            {
                keyWord = keyWord.TrimStart('#');
                return await GetQuestionByTag(keyWord);
            }
            var users = await _dataContext.User.ToListAsync();
            var questions = new List<QuestionResponseDto>();
            string[] searchKeywords = keyWord.ToLower().Split(' ');
            var result = from question in _dataContext.Questions as IEnumerable<Question>
                         let titleWords = question.Title.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                         let searchPhrases = HandleCommon.GenerateSearchPhrases(searchKeywords)
                         let matchingPhrases = searchPhrases
                            .Where(phrase => titleWords.Contains(phrase))
                         where matchingPhrases.Any()
                         let matchCount = matchingPhrases.Count()
                         orderby matchCount descending
                         select new Question()
                         {
                             Id = question.Id,
                             SubId = question.SubId,
                             Title = question.Title,
                             CreatedAt = question.CreatedAt,
                             UpdatedAt = question.UpdatedAt,
                             AuthorId = question.AuthorId,
                             ViewNumber = question.ViewNumber,
                         };

            foreach (var question in result)
            {
                var item = _mapper.Map<QuestionResponseDto>(question);
                var userShort = users.First(x => x.Id == question.AuthorId);
                if (userShort is not null)
                {
                    item.UserShort.FullName = userShort.Fullname;
                    item.UserShort.Id = userShort.Id;
                    item.UserShort.Image = userShort.Image;
                }
                questions.Add(item);
            } 
            return new ApiSuccessResult<List<QuestionResponseDto>>(questions);
        }

        public async Task<ApiResult<List<QuestionResponseDto>>> GetMyQuestionSaved(string id)
        {
            Guid userId = Guid.Parse(id);
            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();

            var questions = await(
                from questionSave in _dataContext.QuestionSaves
                join question in _dataContext.Questions.Where(x=>!x.IsDeleted) on questionSave.QuestionId equals question.Id
                where questionSave.UserId == userId
                select question
            ).ToListAsync();

            var result = new List<QuestionResponseDto>();
            foreach (var item in questions)
            {
                var question = _mapper.Map<QuestionResponseDto>(item);
                var userShort = users.First(x => x.Id == item.AuthorId);
                if (userShort is not null)
                {
                    question.UserShort.FullName = userShort.Fullname;
                    question.UserShort.Id = userShort.Id;
                    question.UserShort.Image = userShort.Image;
                }
                question.SaveNumber = await _dataContext.QuestionSaves.Where(x => x.QuestionId.Equals(item.Id)).CountAsync();
                question.CommentNumber = await _dataContext.Answers.Where(x => x.QuestionId.Equals(item.Id) && !x.IsDeleted).CountAsync();
                question.LikeNumber = await _dataContext.QuestionLikes.Where(x => x.QuestionId.Equals(item.Id)).CountAsync();
                result.Add(question);
            }

            return new ApiSuccessResult<List<QuestionResponseDto>>(result);
        }
    }


}
