using FluentValidation;
using Web.Models.Catalogs;

namespace Web.Validators
{
    public class CourseCreateInputValidator : AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim boş olamaz.").MaximumLength(100).WithMessage("İsim çok uzun");
            RuleFor(x => x.Description).NotNull().NotEmpty().MaximumLength(255).WithMessage("Açıklama çok uzun");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1,150).WithMessage("Kursun süresini yazmalısınız.");
            RuleFor(x => x.Description).MaximumLength(255).WithMessage("Açıklama çok uzun");
            RuleFor(x=>x.Price).GreaterThanOrEqualTo(0).ScalePrecision(2,6); //virgülden sonra 2 karakter,virgülden önce 4 $$$$.$$
            
        }
    }
}
