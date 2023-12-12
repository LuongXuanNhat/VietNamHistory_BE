using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.ExamHistory;
using VNH.Domain.Entities;

namespace VNH.Application.Mappers
{
    public class ExamHistoryMapper : Profile
    {
        public ExamHistoryMapper() { 
           CreateMap<CreateExamHistoryDto,ExamHistory>().ReverseMap();
           CreateMap<ExamHistoryResponseDto,ExamHistory>().ReverseMap();  
        }
    }
}
