using FluentValidation;
using DevLog.Application.Features.Categories.Commands;

namespace DevLog.Application.Features.Categories.Validators
{
    // FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
    public class UpdateCategoryValidator: AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            // kural uymazsa hata mesajı client'a dönüyor
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori adı zorunludur")
                .MinimumLength(3).WithMessage("Kategori adı en az 3 karakter olmalıdır");

            RuleFor(x => x.Id).NotEmpty().WithMessage("Kategori Id zorunludur");
        }
    }
}
