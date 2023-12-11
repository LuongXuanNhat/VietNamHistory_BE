using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.MultipleChoiceDto;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Domain;
using VNH.Domain.Entities;

namespace VNH.Application.Mappers
{
    public class MultipleChoiceMapper : Profile
    {
        public MultipleChoiceMapper() {

            CreateMap<MultipleChoice, MultipleChoiceResponseDto>().ReverseMap();
            CreateMap<Quiz, QuizDto>().ReverseMap();
            CreateMap<QuizAnswer, QuizAnswerDto>();

        }
    }
}
