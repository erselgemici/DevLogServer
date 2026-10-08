using FluentValidation;
using DevLog.Application.Features.Categories.Commands;

namespace DevLog.Application.Features.Categories.Validators
{
    // FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
    public class CreateCategoryValidator: AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            // kural uymazsa hata mesajı client'a dönüyor
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori adı zorunludur")
                .MinimumLength(3).WithMessage("Kategori adı en az 3 karakter olmalıdır");

        }
    }
}
