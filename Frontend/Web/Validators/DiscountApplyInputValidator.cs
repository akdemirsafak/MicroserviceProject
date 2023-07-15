using FluentValidation;
using Web.Models.Discounts;

namespace Web.Validators
{
    public class DiscountApplyInputValidator:AbstractValidator<DiscountApplyInput>
    {
        public DiscountApplyInputValidator() 
        {
            RuleFor(x => x.Code)
                .NotNull().WithMessage("İndirim kodu boş olamaz.")
                .NotEmpty().WithMessage("İndirim kodu boş olamaz.");
        }
    }
}
