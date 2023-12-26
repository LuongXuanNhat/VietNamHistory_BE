using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.MultipleChoiceDto;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Interfaces.Catalog.MultipleChoices
{
    public interface IMultipleChoiceService
    {
        Task<ApiResult<string>> Create(CreateQuizDto requestDto, string name);
        Task<ApiResult<MultipleChoiceResponseDto>> Detail(string id);
        Task<ApiResult<List<MultipleChoiceResponseDto>>> GetAll();

        Task<ApiResult<string>> Update(CreateQuizDto requestDto,string name);
        Task<ApiResult<QuizDto>> UpdateQuizById(QuizDto requestDto);

        Task<ApiResult<string>> Delete(string id, string userId);

        Task<ApiResult<string>> DeleteQuizById(string id);
        Task<ApiResult<List<MultipleChoiceResponseDto>>> GetMyMultipleChoice(string id);

        Task<ApiResult<List<MultipleChoiceResponseDto>>> Search(string keyWord);

    }
}
