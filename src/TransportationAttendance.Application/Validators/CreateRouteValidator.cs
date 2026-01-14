using FluentValidation;
using TransportationAttendance.Application.DTOs.Route;

namespace TransportationAttendance.Application.Validators;

public class CreateRouteValidator : AbstractValidator<CreateRouteDto>
{
    public CreateRouteValidator()
    {
        RuleFor(x => x.RouteName)
            .NotEmpty().WithMessage("اسم المسار مطلوب")
            .MaximumLength(200).WithMessage("اسم المسار يجب ألا يتجاوز 200 حرف");

        RuleFor(x => x.RouteDescription)
            .MaximumLength(1000).WithMessage("وصف المسار يجب ألا يتجاوز 1000 حرف")
            .When(x => !string.IsNullOrEmpty(x.RouteDescription));
    }
}

public class UpdateRouteValidator : AbstractValidator<UpdateRouteDto>
{
    public UpdateRouteValidator()
    {
        RuleFor(x => x.RouteName)
            .NotEmpty().WithMessage("اسم المسار مطلوب")
            .MaximumLength(200).WithMessage("اسم المسار يجب ألا يتجاوز 200 حرف");

        RuleFor(x => x.RouteDescription)
            .MaximumLength(1000).WithMessage("وصف المسار يجب ألا يتجاوز 1000 حرف")
            .When(x => !string.IsNullOrEmpty(x.RouteDescription));
    }
}
