using VNH.Application.DTOs.Catalog.Reports;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Interfaces.Catalog.Reports
{
    public interface IReportService
    {
        Task<ApiResult<List<ReportDto>>> Create(ReportDto requestDto);
        Task<ApiResult<bool>> Update(ReportDto requestDto);
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<ReportDto>> GetById(Guid id);
        Task<ApiResult<List<ReportDto>>> GetAll();
    }
}
