using AutoMapper;

using VNH.Application.DTOs.Catalog.Forum.Question;
using VNH.Domain;

namespace VNH.Application.Mappers
{
    public class QuestionMapper : Profile

    {
        public QuestionMapper()
        {

            CreateMap<CreateQuestionDto, Question>().ReverseMap();

            CreateMap<QuestionResponseDto, Question>().ReverseMap(); 


        }
    }
}
