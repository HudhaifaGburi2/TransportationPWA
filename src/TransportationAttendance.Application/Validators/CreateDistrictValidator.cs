using FluentValidation;
using TransportationAttendance.Application.DTOs.District;

namespace TransportationAttendance.Application.Validators;

public class CreateDistrictValidator : AbstractValidator<CreateDistrictDto>
{
    public CreateDistrictValidator()
    {
        RuleFor(x => x.DistrictNameAr)
            .NotEmpty().WithMessage("اسم المنطقة بالعربية مطلوب")
            .MaximumLength(200).WithMessage("اسم المنطقة يجب ألا يتجاوز 200 حرف");

        RuleFor(x => x.DistrictNameEn)
            .MaximumLength(200).WithMessage("اسم المنطقة بالإنجليزية يجب ألا يتجاوز 200 حرف")
            .When(x => !string.IsNullOrEmpty(x.DistrictNameEn));
    }
}
