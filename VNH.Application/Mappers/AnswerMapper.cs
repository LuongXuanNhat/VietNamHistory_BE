using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Forum.Answer;
using VNH.Domain;

namespace VNH.Application.Mappers
{
    public class AnswerMapper : Profile
    {

        public AnswerMapper() { 
        CreateMap<AnswerQuestionDto,Answer>().ReverseMap();
    
            CreateMap<SubAnswerQuestionDto, SubAnswer>().ReverseMap();
       

        }
    }
}
