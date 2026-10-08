using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Blogs.Queries;
using DevLog.Application.Features.Blogs.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Blogs.Handlers;

// GetBlogByIdQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetBlogByIdQueryHandler(IRepository<Blog> repository, IMapper mapper)
    : IRequestHandler<GetBlogByIdQuery, BaseResult<GetBlogByIdQueryResult>>
{
    public Task<BaseResult<GetBlogByIdQueryResult>> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
    {
        // detay sayfasında yorumlar da lazım, onları da Include ediyorum
        // NOT: GetByIdAsync (FindAsync) Include desteklemiyor, o yüzden FirstOrDefault kullandım
        var value = repository.GetQueryWithIncludes("Translations", "User", "Category", "Category.Translations", "Comments", "Comments.SubComments", "Tags")
            .FirstOrDefault(x => x.Id == request.Id);
        // kayıt yoksa hata dönüyorum
        if (value is null)
        {
            return Task.FromResult(BaseResult<GetBlogByIdQueryResult>.NotFound("Blog bulunamadı"));
        }

        // entity'i dto'ya çeviriyorum
        var blog = mapper.Map<GetBlogByIdQueryResult>(value);
        return Task.FromResult(BaseResult<GetBlogByIdQueryResult>.Success(blog));
    }
}
