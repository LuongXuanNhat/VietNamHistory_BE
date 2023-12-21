using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.ExamHistory;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Interfaces.Catalog.ExamHistory
{
    public interface IExamHistoryService
    {

        Task<ApiResult<ExamHistoryResponseDto>> Create(CreateExamHistoryDto requestDto, string name);
        Task<ApiResult<ExamHistoryResponseDto>> Update(CreateExamHistoryDto requestDto, string name);

        Task<ApiResult<List<ExamHistoryResponseDto>>> GetMyExamHistory(string id);
        Task<ApiResult<List<ExamHistoryResponseDto>>> GetExamHistory(string examId);
    }
}
