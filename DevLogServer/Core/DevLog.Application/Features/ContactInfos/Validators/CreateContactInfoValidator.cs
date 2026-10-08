using FluentValidation;
using DevLog.Application.Features.ContactInfos.Commands;

namespace DevLog.Application.Features.ContactInfos.Validators;

// FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
public class CreateContactInfoValidator : AbstractValidator<CreateContactInfoCommand>
{
    public CreateContactInfoValidator()
    {

        // kural uymazsa hata mesajı client'a dönüyor
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Adres zorunludur.")
            .MaximumLength(200).WithMessage("Adres en fazla 200 karakter olabilir.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir e-posta giriniz.");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("Telefon numarası zorunludur.");
        RuleFor(x => x.MapUrl).NotEmpty().WithMessage("Harita URL'si zorunludur.");
    }
}
