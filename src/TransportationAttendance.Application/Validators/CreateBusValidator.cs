using FluentValidation;
using TransportationAttendance.Application.DTOs.Bus;

namespace TransportationAttendance.Application.Validators;

public class CreateBusValidator : AbstractValidator<CreateBusDto>
{
    public CreateBusValidator()
    {
        RuleFor(x => x.BusNumber)
            .NotEmpty().WithMessage("رقم الباص مطلوب")
            .MaximumLength(20).WithMessage("رقم الباص يجب ألا يتجاوز 20 حرف");

        RuleFor(x => x.PeriodId)
            .GreaterThan(0).WithMessage("الفترة مطلوبة");

        RuleFor(x => x.Capacity)
            .InclusiveBetween(1, 100).WithMessage("السعة يجب أن تكون بين 1 و 100");

        RuleFor(x => x.DriverName)
            .MaximumLength(200).WithMessage("اسم السائق يجب ألا يتجاوز 200 حرف")
            .When(x => !string.IsNullOrEmpty(x.DriverName));

        RuleFor(x => x.DriverPhoneNumber)
            .MaximumLength(20).WithMessage("رقم الهاتف يجب ألا يتجاوز 20 رقم")
            .Matches(@"^[\d\s\+\-]+$").WithMessage("رقم الهاتف يجب أن يحتوي على أرقام فقط")
            .When(x => !string.IsNullOrEmpty(x.DriverPhoneNumber));
    }
}

public class UpdateBusValidator : AbstractValidator<UpdateBusDto>
{
    public UpdateBusValidator()
    {
        RuleFor(x => x.BusNumber)
            .NotEmpty().WithMessage("رقم الباص مطلوب")
            .MaximumLength(20).WithMessage("رقم الباص يجب ألا يتجاوز 20 حرف");

        RuleFor(x => x.PeriodId)
            .GreaterThan(0).WithMessage("الفترة مطلوبة");

        RuleFor(x => x.Capacity)
            .InclusiveBetween(1, 100).WithMessage("السعة يجب أن تكون بين 1 و 100");

        RuleFor(x => x.DriverName)
            .MaximumLength(200).WithMessage("اسم السائق يجب ألا يتجاوز 200 حرف")
            .When(x => !string.IsNullOrEmpty(x.DriverName));

        RuleFor(x => x.DriverPhoneNumber)
            .MaximumLength(20).WithMessage("رقم الهاتف يجب ألا يتجاوز 20 رقم")
            .Matches(@"^[\d\s\+\-]+$").WithMessage("رقم الهاتف يجب أن يحتوي على أرقام فقط")
            .When(x => !string.IsNullOrEmpty(x.DriverPhoneNumber));
    }
}
