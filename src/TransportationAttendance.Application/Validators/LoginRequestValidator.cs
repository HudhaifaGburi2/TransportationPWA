using FluentValidation;
using TransportationAttendance.Application.DTOs.Auth;

namespace TransportationAttendance.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("اسم المستخدم مطلوب")
            .MaximumLength(100).WithMessage("اسم المستخدم يجب ألا يتجاوز 100 حرف");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("كلمة المرور مطلوبة")
            .MinimumLength(3).WithMessage("كلمة المرور يجب أن تكون 3 أحرف على الأقل");
    }
}
