using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.Notifications;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Domain.Entities;

namespace VNH.Application.Implement.Catalog.NotificationServices
{
    public interface INotificationService
    {
        Task<ApiResult<List<NotificationDto>>> GetAll(string userId);
        Task<ApiResult<NotificationDto>> Add(string title);
        Task AddNotificationDetail(NotificationDto notification);
        Task<ApiResult<NotificationDto>> Update(NotificationDto notification);
    }
}
