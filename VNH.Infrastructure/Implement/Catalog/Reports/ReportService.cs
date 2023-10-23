using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VNH.Application.DTOs.Catalog.Reports;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Catalog.Reports;
using VNH.Domain;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.Reports
{
    public class ReportService : IReportService
    {
        private readonly VietNamHistoryContext _dataContext;
        private readonly IMapper _mapper;
        public ReportService(VietNamHistoryContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<ApiResult<List<ReportDto>>> Create(ReportDto requestDto)
        {
            Report report = new()
            {
                Id = requestDto.Id,
                CreatedAt = DateTime.UtcNow,
                Description = requestDto.Description,
                Title = requestDto.Title
            };
            _dataContext.Reports.Add(report);
            await _dataContext.SaveChangesAsync();
            var reports = await _dataContext.Reports.ToListAsync();

            var results = new List<ReportDto>();
            foreach (var item in reports)
            {
                results.Add(new()
                {
                    Id = item.Id,
                    Description = item.Description,
                    Title = item.Title
                });
            }
            return new ApiSuccessResult<List<ReportDto>>(results);
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var report = await _dataContext.Reports.FindAsync(id);
            if (report == null)
            {
                return new ApiErrorResult<bool>("Đối tượng không tồn tại hoặc đã bị xóa");
            }

            _dataContext.Reports.Remove(report);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<ReportDto>>> GetAll()
        {
            var reports = await _dataContext.Reports.ToListAsync();
            var results = new List<ReportDto>();
            foreach (var report in reports)
            {
                results.Add(new()
                {
                    Id = report.Id,
                    Description = report.Description,
                    Title = report.Title,
                });
            }
            return new ApiSuccessResult<List<ReportDto>>(results);
        }

        public async Task<ApiResult<ReportDto>> GetById(Guid id)
        {
            var report = await _dataContext.Reports.FindAsync(id);
            return report != null ? new ApiSuccessResult<ReportDto>(_mapper.Map<ReportDto>(report)) : new ApiErrorResult<ReportDto>("Đối tượng không tồn tại");
        }

        public async Task<ApiResult<bool>> Update(ReportDto requestDto)
        {
            var report = await _dataContext.Reports.FindAsync(requestDto.Id);
            if (report == null)
            {
                return new ApiErrorResult<bool>("Đối tượng cập nhập không tồn tại hoặc đã bị xóa");
            }
            report.Title = requestDto.Title;
            report.Description = requestDto.Description;

            _dataContext.Reports.Update(report);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }
    }
}
