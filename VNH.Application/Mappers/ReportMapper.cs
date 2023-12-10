using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Forum.Question;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Catalog.Reports;
using VNH.Domain;

namespace VNH.Application.Mappers
{
    public class ReportMapper : Profile
    {
        public ReportMapper() {
           CreateMap<ReportDto, Report>().ReverseMap();
           CreateMap<ReportPostDto, PostReportDetail>().ReverseMap();
           CreateMap<ReportQuestionDto, QuestionReportDetail>().ReverseMap();

        }
    }
}
