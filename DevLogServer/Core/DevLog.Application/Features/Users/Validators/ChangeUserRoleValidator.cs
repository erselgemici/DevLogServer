using FluentValidation;
using DevLog.Application.Constants;
using DevLog.Application.Features.Users.Commands;

namespace DevLog.Application.Features.Users.Validators;

// FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
public class ChangeUserRoleValidator : AbstractValidator<ChangeUserRoleCommand>
{
    public ChangeUserRoleValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Kullanıcı zorunludur");
        RuleFor(x => x.Role).Must(role => Roles.All.Contains(role)).WithMessage("Geçersiz rol");
    }
}
