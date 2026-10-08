using FluentValidation;
using DevLog.Application.Features.SubComments.Commands;

namespace DevLog.Application.Features.SubComments.Validators;

// FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
public class UpdateSubCommentValidator : AbstractValidator<UpdateSubCommentCommand>
{
    public UpdateSubCommentValidator()
    {
        // kural uymazsa hata mesajı client'a dönüyor
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id zorunludur");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad zorunludur");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad zorunludur");
        RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta zorunludur")
            .EmailAddress().WithMessage("Geçerli bir e-posta giriniz");
        RuleFor(x => x.CommentId).NotEmpty().WithMessage("Yorum zorunludur");
        RuleFor(x => x.Body).NotEmpty().WithMessage("Yorum zorunludur")
            .MaximumLength(200).WithMessage("Yorum en fazla 200 karakter olabilir");
    }
}
