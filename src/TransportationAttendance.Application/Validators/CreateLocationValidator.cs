using FluentValidation;
using TransportationAttendance.Application.DTOs.Location;

namespace TransportationAttendance.Application.Validators;

public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.LocationCode)
            .NotEmpty().WithMessage("رمز الموقع مطلوب")
            .MaximumLength(20).WithMessage("رمز الموقع يجب ألا يتجاوز 20 حرف")
            .Matches("^[A-Za-z0-9-]+$").WithMessage("رمز الموقع يجب أن يحتوي على أحرف وأرقام فقط");

        RuleFor(x => x.LocationName)
            .NotEmpty().WithMessage("اسم الموقع مطلوب")
            .MaximumLength(100).WithMessage("اسم الموقع يجب ألا يتجاوز 100 حرف");

        RuleFor(x => x.LocationType)
            .MaximumLength(50).WithMessage("نوع الموقع يجب ألا يتجاوز 50 حرف")
            .When(x => !string.IsNullOrEmpty(x.LocationType));
    }
}
