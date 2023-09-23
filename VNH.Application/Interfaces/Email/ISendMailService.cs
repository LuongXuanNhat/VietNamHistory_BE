using VNH.Application.DTOs.Common.SendEmail;

namespace VNH.Application.Interfaces.Email
{
    public interface ISendMailService
    {
        Task SendMail(MailContent mailContent);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
