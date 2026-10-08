using FluentValidation;
using DevLog.Application.Features.Users.Queries;

namespace DevLog.Application.Features.Users.Validators;

// FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
public class GetLoginValidator : AbstractValidator<GetLoginQuery>
{
    public GetLoginValidator()
    {
        // kural uymazsa hata mesajı client'a dönüyor
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta zorunludur");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre zorunludur");
    }
}
