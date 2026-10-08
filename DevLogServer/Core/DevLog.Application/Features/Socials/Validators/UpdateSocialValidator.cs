using FluentValidation;
using DevLog.Application.Features.Socials.Commands;

namespace DevLog.Application.Features.Socials.Validators;

// FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
public class UpdateSocialValidator : AbstractValidator<UpdateSocialCommand>
{
    public UpdateSocialValidator()
    {
        // kural uymazsa hata mesajı client'a dönüyor
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id zorunludur.");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık zorunludur.")
            .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("URL zorunludur.")
            .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _)).WithMessage("Geçerli bir URL giriniz.");
        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("İkon zorunludur.")
            .MaximumLength(50).WithMessage("İkon en fazla 50 karakter olabilir.");
    }
}
