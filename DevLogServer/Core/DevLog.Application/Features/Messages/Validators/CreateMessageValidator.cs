using FluentValidation;
using DevLog.Application.Features.Messages.Commands;

namespace DevLog.Application.Features.Messages.Validators;

// FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
public class CreateMessageValidator : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageValidator()
    {
        // kural uymazsa hata mesajı client'a dönüyor
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ad zorunludur.")
            .Length(2, 50).WithMessage("Ad 2 ile 50 karakter arasında olmalıdır.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir e-posta giriniz.");
        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Konu zorunludur.")
            .Length(2, 100).WithMessage("Konu 2 ile 100 karakter arasında olmalıdır.");
        RuleFor(x => x.MessageBody)
            .NotEmpty().WithMessage("Mesaj zorunludur.")
            .Length(2, 500).WithMessage("Mesaj 2 ile 500 karakter arasında olmalıdır.");
    }
}
