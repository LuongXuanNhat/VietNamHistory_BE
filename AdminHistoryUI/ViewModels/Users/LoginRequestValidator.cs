using FluentValidation;
using VNH.Application.DTOs.Catalog.Users;

namespace AdminHistoryUI.ViewModels.Users
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Tên tài khoản không được để trống").OverridePropertyName("Email");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu không được để trống").OverridePropertyName("Password");
        }
    }
}
