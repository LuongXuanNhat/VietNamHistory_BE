using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Notifications;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Implement.Catalog.NotificationServices;
using VNH.Domain.Entities;
using VNH.Domain.Enums;
using VNH.Infrastructure.Presenters;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private readonly VietNamHistoryContext _dataContext;
        private readonly IHubContext<ChatSignalR> _notiHubContext;
        private readonly IMapper _mapper;
        public NotificationService(VietNamHistoryContext vietNamHistoryContext, IHubContext<ChatSignalR> chatSignalR,
         IMapper mapper   )
        {
            _dataContext = vietNamHistoryContext;
            _notiHubContext = chatSignalR; 
            _mapper = mapper;
        }
        public async Task<ApiResult<NotificationDto>> Add(string title)
        {
            var noti = new Notification()
            {
                Title = title,
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
            };
            _dataContext.Notifications.Add(noti);
            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<NotificationDto>(new());
        }

        public async Task AddNotificationDetail(NotificationDto notification)
        {
            var noti = await _dataContext.Notifications.FirstOrDefaultAsync();
            if (noti == null)
            {
                return;
            }
            var notificationDetail = new NotificationDetail()
            {
                Id = Guid.NewGuid(),
                NotificationId = noti.Id,
                UserId = notification.UserId,
                Content = notification.Content,
                Url = notification.Url,
                Date = DateTime.Now,
                IsRead = Confirm.No,
                IdObject = notification.IdObject
            };

            _dataContext.NotificationDetails.Add(notificationDetail);
            await _dataContext.SaveChangesAsync();
            await _notiHubContext.Clients.Group(notification.UserId.ToString()).SendAsync("ReceiveNoti", notification);
        }

        public async Task<ApiResult<List<NotificationDto>>> GetAll(string userId)
        {
            var notidetails = await _dataContext.NotificationDetails.Where(x => x.UserId == Guid.Parse(userId)).ToListAsync();
            if (notidetails.Any())
            {
                List<NotificationDto> result = new();
                foreach (var item in notidetails)
                {
                    var notificationDetail = new NotificationDto();
                    notificationDetail = _mapper.Map<NotificationDto>(item);
                    result.Add(notificationDetail);
                }
                return new ApiSuccessResult<List<NotificationDto>>(result);
            }
            return new ApiSuccessResult<List<NotificationDto>>(new());
        }

        public Task<ApiResult<NotificationDto>> Update(NotificationDto notification)
        {
            throw new NotImplementedException();
        }
    }
}
