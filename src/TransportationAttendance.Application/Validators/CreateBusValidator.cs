using FluentValidation;
using TransportationAttendance.Application.DTOs.Bus;

namespace TransportationAttendance.Application.Validators;

public class CreateBusValidator : AbstractValidator<CreateBusDto>
{
    public CreateBusValidator()
    {
        // PlateNumber is the authoritative identifier from official CSV
        // Uniqueness enforced on (PlateNumber + PeriodId) - same plate can exist in different periods
        RuleFor(x => x.PlateNumber)
            .NotEmpty().WithMessage("رقم اللوحة مطلوب")
            .MaximumLength(20).WithMessage("رقم اللوحة يجب ألا يتجاوز 20 حرف");

        RuleFor(x => x.BusNumber)
            .MaximumLength(10).WithMessage("رقم الباص يجب ألا يتجاوز 10 أحرف")
            .When(x => !string.IsNullOrEmpty(x.BusNumber));

        RuleFor(x => x.PeriodId)
            .GreaterThan(0).WithMessage("الفترة مطلوبة");

        RuleFor(x => x.Capacity)
            .InclusiveBetween(1, 100).WithMessage("السعة يجب أن تكون بين 1 و 100");
    }
}

public class UpdateBusValidator : AbstractValidator<UpdateBusDto>
{
    public UpdateBusValidator()
    {
        // PlateNumber is the authoritative identifier from official CSV
        // Uniqueness enforced on (PlateNumber + PeriodId) - same plate can exist in different periods
        RuleFor(x => x.PlateNumber)
            .NotEmpty().WithMessage("رقم اللوحة مطلوب")
            .MaximumLength(20).WithMessage("رقم اللوحة يجب ألا يتجاوز 20 حرف");

        RuleFor(x => x.BusNumber)
            .MaximumLength(10).WithMessage("رقم الباص يجب ألا يتجاوز 10 أحرف")
            .When(x => !string.IsNullOrEmpty(x.BusNumber));

        RuleFor(x => x.PeriodId)
            .GreaterThan(0).WithMessage("الفترة مطلوبة");

        RuleFor(x => x.Capacity)
            .InclusiveBetween(1, 100).WithMessage("السعة يجب أن تكون بين 1 و 100");
    }
}
