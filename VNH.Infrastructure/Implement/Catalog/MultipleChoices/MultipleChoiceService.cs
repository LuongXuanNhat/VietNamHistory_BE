using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.MultipleChoiceDto;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Catalog.MultipleChoices;
using VNH.Domain;
using VNH.Domain.Entities;
using VNH.Infrastructure.Implement.Common;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.MultipleChoices
{
    public class MultipleChoiceService : IMultipleChoiceService
    {
        private readonly UserManager<User> _userManager;
        private readonly VietNamHistoryContext _dataContext;
        private readonly IMapper _mapper;


        public MultipleChoiceService(UserManager<User> userManager, VietNamHistoryContext dataContext, IMapper mapper)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ApiResult<string>> Create(CreateQuizDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var multipleChoice = new MultipleChoice();
            var quiz = ReadQuestionFromDocx(requestDto.File);
            multipleChoice.Title = requestDto.Title;
            multipleChoice.Description = requestDto.Description;
            multipleChoice.WorkTime = requestDto.WorkTime;
            multipleChoice.UserId = user.Id;
            multipleChoice.CreatedAt = DateTime.Now;
            try
            {
                _dataContext.MultipleChoices.Add(multipleChoice);
                await _dataContext.SaveChangesAsync();
                foreach (var quiestion in quiz)
                {
                    var quizz = new Quiz()
                    {
                        Id = Guid.NewGuid(),
                        MultipleChoiceId = multipleChoice.Id,
                        Content = quiestion.Content,
                    };
                    _dataContext.Quizzes.Add(quizz);

                    foreach (var quizanswer in quiestion.QuizAnswers)
                    {
                        var answer = new QuizAnswer()
                        {
                            Id = Guid.NewGuid(),
                            Content = quizanswer.Content,
                            QuizId = quizz.Id,
                            isCorrect = quizanswer.isCorrect
                        };

                        _dataContext.QuizAnswers.Add(answer);
                    }
                }
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<string>("Tạo thành công");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>("Lỗi lưu  : " + ex.Message);
            }



        }

        /* static List<QuizDto> ReadQuestionFromDocx(IFormFile file)
         {
             List<QuizDto> quizzes = new List<QuizDto>();
             using(var stream = file.OpenReadStream())

                     using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, false))
                     {
                         var body = doc.MainDocumentPart.Document.Body;
                         var paragraphs = body.Elements<Paragraph>();

                         QuizDto currentQuiz = null;

                         foreach (var paragraph in paragraphs)
                         {
                             var text = paragraph.InnerText.Trim();
                             if (text.StartsWith("Câu hỏi:"))
                             {
                                 currentQuiz = new QuizDto
                                 {
                                     Content = text.Substring("Câu hỏi:".Length).Trim(),
                                     QuizAnswers = new List<QuizAnswerDto>()

                                 };
                             }
                             else if (text.StartsWith("-"))
                             {
                                 var answerText = text.Substring(1).Trim();
                                 var isCorrectt = answerText.EndsWith("(Đúng)");

                                 if (isCorrectt)
                                 {
                                     answerText = answerText.Substring(0, answerText.Length - "(Đúng)".Length).Trim();
                                 }

                                 var answer = new QuizAnswerDto
                                 {

                                     Content = answerText,
                                     isCorrect = isCorrectt


                                 };
                                 currentQuiz.QuizAnswers.Add(answer);


                             }
                     else if (currentQuiz != null)
                     {
                         quizzes.Add(currentQuiz);
                         currentQuiz = null;

                     }

                   }
                     }
                     return quizzes;




         }*/




        /*static List<QuizDto> ReadQuestionFromDocx(IFormFile file)
            {
                List<QuizDto> quizzes = new List<QuizDto>();
                using (var stream = file.OpenReadStream())
                using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, false))
                {
                    var body = doc.MainDocumentPart.Document.Body;
                    var paragraphs = body.Elements<Paragraph>();

                    QuizDto currentQuiz = null;

                    foreach (var paragraph in paragraphs)
                    {
                        var text = paragraph.InnerText.Trim();

                        if (text.StartsWith("Câu"))
                        {
                            // Start of a new question
                            currentQuiz = new QuizDto
                            {
                                Content = text.Trim(),
                                QuizAnswers = new List<QuizAnswerDto>()
                            };
                            quizzes.Add(currentQuiz);
                        }
                        else if (Regex.IsMatch(text, @"^[ABCD][\.\)]"))
                        {
                            // Answer choices
                            currentQuiz.QuizAnswers.Add(new QuizAnswerDto
                            {
                                Content = text.Trim(),
                                isCorrect = false // Default to false
                            });
                        }
                        else if (text.StartsWith("Đáp án đúng:"))
                        {
                            // Identifying the correct answer
                            var correctAnswer = text.Split(':')[1].Trim();
                            foreach (var answer in currentQuiz.QuizAnswers)
                            {
                                if (answer.Content.StartsWith(correctAnswer))
                                {
                                    answer.isCorrect = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                return quizzes;
            }
        */




        static List<QuizDto> ReadQuestionFromDocx(IFormFile file)
        {
            List<QuizDto> quizzes = new List<QuizDto>();
            using (var stream = file.OpenReadStream())
            using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, false))
            {
                var body = doc.MainDocumentPart.Document.Body;
                var paragraphs = body.Elements<Paragraph>();

                QuizDto currentQuiz = null;

                foreach (var paragraph in paragraphs)
                {
                    var text = paragraph.InnerText.Trim();

                    if (text.StartsWith("Câu"))
                    {
                        var questionContent = Regex.Replace(text, @"^Câu\s*\d+\s*:\s*", "").Trim();
                        currentQuiz = new QuizDto
                        {
                            Content = questionContent,
                            QuizAnswers = new List<QuizAnswerDto>()
                        };
                        quizzes.Add(currentQuiz);
                    }
                    else if (Regex.IsMatch(text, @"^[ABCD][\.\)]"))
                    {
                        // Answer choices
                        currentQuiz.QuizAnswers.Add(new QuizAnswerDto
                        {
                            Content = text.Trim(),
                            isCorrect = false // Default to false
                        });
                    }
                    else if (text.StartsWith("Đáp án đúng:"))
                    {
                        var correctAnswerLabel = text.Split(':')[1].Trim();
                        foreach (var answer in currentQuiz.QuizAnswers)
                        {
                            if (answer.Content.StartsWith(correctAnswerLabel))
                            {
                                answer.isCorrect = true;
                            }

                            answer.Content = Regex.Replace(answer.Content, @"^[ABCD][\.\)]\s*", "").Trim();
                        }
                    }
                }
            }
            return quizzes;
        }

        public async Task<ApiResult<MultipleChoiceResponseDto>> Detail(string id)
        {
            var multipleChoice = await _dataContext.MultipleChoices.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)));
            if (multipleChoice is null)
            {
                return new ApiErrorResult<MultipleChoiceResponseDto>("Không tìm thấy bài làm");
            }
            var user = await _userManager.FindByIdAsync(multipleChoice.UserId.ToString());
            var response = _mapper.Map<MultipleChoiceResponseDto>(multipleChoice);
            var quizs = await _dataContext.Quizzes.Where(x => x.MultipleChoiceId.Equals(Guid.Parse(id))).ToListAsync();
            response.WorkTime = multipleChoice.WorkTime;

            foreach (var quiz in quizs)
            {

                var quizanswers = await _dataContext.QuizAnswers.Where(x => x.QuizId.Equals(quiz.Id)).ToListAsync();

                response.Quizs.Add(new()
                {
                    Id = quiz.Id,
                    Content = quiz.Content,
                    QuizAnswers = _mapper.Map<List<QuizAnswerDto>>(quizanswers),

                });




            }
            foreach (var item in response.Quizs)
            {
                var quizanswers = await _dataContext.QuizAnswers.Where(x => x.QuizId.Equals(item.Id)).ToListAsync();

            }

            response.UserShort = new()
            {
                FullName = user.Fullname,
                Id = user.Id,
                Image = user.Image,
            };
            return new ApiSuccessResult<MultipleChoiceResponseDto>(response);
        }


        public async Task<ApiResult<List<MultipleChoiceResponseDto>>> GetAll()
        {
            var list = await _dataContext.MultipleChoices.OrderByDescending(x => x.CreatedAt).ToListAsync();
            var users = await _dataContext.User.ToListAsync();
            var quizlist = await _dataContext.Quizzes.ToListAsync();
            var quizanswerList = await _dataContext.QuizAnswers.ToListAsync();

            var result = new List<MultipleChoiceResponseDto>();

            foreach (var item in list)
            {
                var multi = _mapper.Map<MultipleChoiceResponseDto>(item);
                multi.NumberQuiz = quizlist.Where(x => x.MultipleChoiceId.Equals(item.Id)).Count();
                var userShort = users.First(x => x.Id == item.UserId);
                if (userShort is not null)
                {
                    multi.UserShort.FullName = userShort.Fullname;
                    multi.UserShort.Id = userShort.Id;
                    multi.UserShort.Image = userShort.Image;
                }

                result.Add(multi);

            }
            return new ApiSuccessResult<List<MultipleChoiceResponseDto>>(result);
        }

        public async Task<ApiResult<string>> Update(CreateQuizDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);

            var updateMultipleChoice = await _dataContext.MultipleChoices.FirstOrDefaultAsync(x => x.Id == Guid.Parse(requestDto.Id));
            if (updateMultipleChoice is null)
            {
                return new ApiErrorResult<string>("Lỗi :Không được cập nhập (không tìm thấy bài thi nào)");
            }
            updateMultipleChoice.Title = requestDto.Title;
            updateMultipleChoice.Description = requestDto.Description;
            updateMultipleChoice.WorkTime = requestDto.WorkTime;
            updateMultipleChoice.UpdatedAt = DateTime.Now;
            try
            {
                _dataContext.MultipleChoices.Update(updateMultipleChoice);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<string>("Cập nhập bài thi thành công");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>("Lỗi lưu bài thi : " + ex.Message);

            }
        }


        public async Task<ApiResult<QuizDto>> UpdateQuizById(QuizDto requestDto)
        {
            var updateQuiz = _dataContext.Quizzes.First(x => x.Id.Equals(requestDto.Id));
            if (updateQuiz is null)
            {
                return new ApiErrorResult<QuizDto>("Lỗi :Không được cập nhập (không tìm thấy câu hỏi nào)");
            }
            updateQuiz.Content = requestDto.Content;
            foreach (var item in requestDto.QuizAnswers)
            {
                var updateQuizAnswer = _dataContext.QuizAnswers.First(x => x.Id.Equals(item.Id));
                if (updateQuizAnswer != null)
                {
                    updateQuizAnswer.Content = item.Content;
                    updateQuizAnswer.isCorrect = item.isCorrect;
                }
                _dataContext.QuizAnswers.Update(updateQuizAnswer);
                await _dataContext.SaveChangesAsync();
            }
            try
            {
                _dataContext.Quizzes.Update(updateQuiz);
                await _dataContext.SaveChangesAsync();
                var response = _mapper.Map<QuizDto>(updateQuiz);
                return new ApiSuccessResult<QuizDto>(response);

            }
            catch (Exception ex) {
                return new ApiErrorResult<QuizDto>("Lỗi lưu câu hỏi : " + ex.Message);

            }
        }


        public async Task<ApiResult<string>> Delete(string id, string userId)
        {
            var multiple = await _dataContext.MultipleChoices
                .FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)) && x.UserId.Equals(Guid.Parse(userId)));

            if (multiple is null)
            {
                return new ApiErrorResult<string>("Không tìm thấy bài thi");
            }

            var multipleChoiceId = multiple.Id;

            // Xoá lịch sử bài thi
            var examHistoriesToDelete = _dataContext.ExamHistories
                .Where(x => x.MultipleChoiceId == multipleChoiceId)
                .ToList();

            if (examHistoriesToDelete.Any())
            {
                _dataContext.ExamHistories.RemoveRange(examHistoriesToDelete);
            }

            // Xoá câu trả lời của từng câu hỏi
            var quizIdsToDelete = _dataContext.Quizzes
                .Where(x => x.MultipleChoiceId.Equals(multipleChoiceId))
                .Select(x => x.Id)
                .ToList();

            var quizAnswerToDelete = _dataContext.QuizAnswers
                .Where(x => quizIdsToDelete.Contains(x.QuizId))
                .ToList();

            if (quizAnswerToDelete.Any())
            {
                _dataContext.QuizAnswers.RemoveRange(quizAnswerToDelete);
            }

            // Xoá câu hỏi
            var quizzesToDelete = _dataContext.Quizzes
                .Where(x => x.MultipleChoiceId.Equals(multipleChoiceId))
                .ToList();

            if (quizzesToDelete.Any())
            {
                _dataContext.Quizzes.RemoveRange(quizzesToDelete);
            }

            // Xoá bài thi
            _dataContext.MultipleChoices.Remove(multiple);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<string>("Xoá bài thi thành công");
        }


        public async Task<ApiResult<string>> DeleteQuizById(string id)
        {
            var quiz = await _dataContext.Quizzes.FirstOrDefaultAsync(x => x.Id.Equals(Guid.Parse(id)));
            if (quiz == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy câu hỏi");
            }
            var quizanswers = await _dataContext.QuizAnswers.ToListAsync();
            foreach (var item in quizanswers)
            {
                if (item.QuizId.Equals(id))
                {
                    _dataContext.QuizAnswers.Remove(item);
                    await _dataContext.SaveChangesAsync();
                }
            }
            _dataContext.Quizzes.Remove(quiz);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<string>("Xoá câu hỏi thành công");

        }

        public async Task<ApiResult<List<MultipleChoiceResponseDto>>> GetMyMultipleChoice(string id)
        {
            var list = await _dataContext.MultipleChoices.Where(x => x.UserId.Equals(Guid.Parse(id)) ).ToListAsync();
            
            var result = new List<MultipleChoiceResponseDto>();
            var user = await _dataContext.User.Where(x => !x.IsDeleted && x.Id.ToString().Equals(id)).FirstOrDefaultAsync();
            foreach( var item in list)
            {
                var multi = _mapper.Map<MultipleChoiceResponseDto>(item);
               
                result.Add(multi);
            }
            return new ApiSuccessResult<List<MultipleChoiceResponseDto>>(result);

        }



        public async Task<ApiResult<List<MultipleChoiceResponseDto>>> Search(string keyWord)
        {
            var multiples = new List<MultipleChoiceResponseDto>();
            var users = await _dataContext.Users.Where(x=>!x.IsDeleted).ToListAsync();
            string[] searchKeywords = keyWord.ToLower().Split(' ');
            var result = from multiple in _dataContext.MultipleChoices as IEnumerable<MultipleChoice>
                         let titleWords = multiple.Title.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                         let searchPhrases = HandleCommon.GenerateSearchPhrases(searchKeywords)
                         let matchingPhrases = searchPhrases.Where(phrase => titleWords.Contains(phrase))
                         where matchingPhrases.Any()
                         let matchCount = matchingPhrases.Count()
                         orderby matchCount descending
                         select new MultipleChoice()
                         {
                             Id = multiple.Id,
                             Title = multiple.Title,
                             Description = multiple.Description,
                             CreatedAt = multiple.CreatedAt,
                             UpdatedAt = multiple.UpdatedAt,
                             WorkTime = multiple.WorkTime,
                             UserId = multiple.UserId,
                         };

            if(result.Count() > 0 )
            {
                var quizlist = await _dataContext.Quizzes.ToListAsync();
                foreach (var multi in result)
                {
                    var item = _mapper.Map<MultipleChoiceResponseDto>(multi);
                    var userShort = users.FirstOrDefault(x => x.Id == multi.UserId);
                    item.NumberQuiz = quizlist.Where(x => x.MultipleChoiceId.Equals(multi.Id)).Count();
                    if (userShort is not null)
                    {
                        item.UserShort.FullName = userShort.Fullname;
                        item.UserShort.Id = userShort.Id;
                        item.UserShort.Image = userShort.Image;
                    }
                    multiples.Add(item);
                }
            }

            return new ApiSuccessResult<List<MultipleChoiceResponseDto>>(multiples);

        }

    }

  





}
