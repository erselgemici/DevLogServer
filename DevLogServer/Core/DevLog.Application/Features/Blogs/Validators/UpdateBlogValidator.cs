using FluentValidation;
using DevLog.Application.Features.Blogs.Commands;

namespace DevLog.Application.Features.Blogs.Validators;

// FluentValidation kuralları, ValidationBehavior handler'dan önce burayı kontrol ediyor
public class UpdateBlogValidator : AbstractValidator<UpdateBlogCommand>
{
    public UpdateBlogValidator()
    {
        // kural uymazsa hata mesajı client'a dönüyor
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id zorunludur");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık zorunludur");
        RuleFor(x => x.Description).NotEmpty().WithMessage("İçerik zorunludur");
        RuleFor(x => x.CoverImage).NotEmpty().WithMessage("Kapak görseli zorunludur");
        RuleFor(x => x.BlogImage).NotEmpty().WithMessage("Blog görseli zorunludur");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori zorunludur");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Yazar zorunludur");
    }
}
