using FluentValidation;
using DevLog.Application.Features.Users.Commands;

namespace DevLog.Application.Features.Users.Validators;

// FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        // kural uymazsa hata mesajı client'a dönüyor
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad zorunludur");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad zorunludur");
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı zorunludur");
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta zorunludur")
            .EmailAddress().WithMessage("Geçerli bir e-posta giriniz");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre zorunludur");
    }
}
