using FluentValidation;
using DevLog.Application.Features.Comments.Commands;

namespace DevLog.Application.Features.Comments.Validators;

// FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
public class UpdateCommentValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentValidator()
    {
        // kural uymazsa hata mesajı client'a dönüyor
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id zorunludur");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad zorunludur");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad zorunludur");
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta zorunludur")
            .EmailAddress().WithMessage("Geçerli bir e-posta giriniz");
        RuleFor(x => x.BlogId).NotEmpty().WithMessage("Blog zorunludur");
        RuleFor(x => x.Body).NotEmpty().WithMessage("Yorum metni zorunludur");
    }
}
