using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.MultipleChoiceDto;
using VNH.Domain.Entities;
using VNH.Domain;
using VNH.Application.DTOs.Catalog.Notifications;

namespace VNH.Application.Mappers
{
    public class NotifiMapper : Profile
    {
        public NotifiMapper() {
            CreateMap<NotificationDetail, NotificationDto>().ReverseMap();
        }
    }
}
