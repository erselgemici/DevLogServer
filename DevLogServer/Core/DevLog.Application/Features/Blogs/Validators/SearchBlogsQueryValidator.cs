using FluentValidation;
using DevLog.Application.Features.Blogs.Queries;

namespace DevLog.Application.Features.Blogs.Validators;

// validator sadece command'lar için değil, query'ler için de çalışıyor (ValidationBehavior hepsine bakıyor)
// tek harfle arama yapılınca neredeyse bütün yazılar dönüyor, o yüzden en az 2 karakter istiyorum
public class SearchBlogsQueryValidator : AbstractValidator<SearchBlogsQuery>
{
    public SearchBlogsQueryValidator()
    {
        RuleFor(x => x.Text).NotEmpty().WithMessage("Aranacak kelime boş olamaz")
            .MinimumLength(2).WithMessage("En az 2 karakter yazmalısınız")
            .MaximumLength(100).WithMessage("Arama en fazla 100 karakter olabilir");
    }
}
