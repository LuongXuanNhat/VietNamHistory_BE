using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VNH.Application.DTOs.Catalog.Notifications;
using VNH.Application.Implement.Catalog.NotificationServices;

namespace VNH.WebAPi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notification)
        {
            _notificationService = notification;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var result = await _notificationService.GetAll(userId);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Add(string title)
        {
            var result = await _notificationService.Add(title);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }

        [HttpPost("AddNotification")]
        [Authorize]
        public async Task<IActionResult> AddNotificationDetail(NotificationDto notification)
        {
            await _notificationService.AddNotificationDetail(notification);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(NotificationDto notification)
        {
            var result = await _notificationService.Update(notification);
            return !result.IsSuccessed ? BadRequest(result) : Ok(result);
        }
    }
}
