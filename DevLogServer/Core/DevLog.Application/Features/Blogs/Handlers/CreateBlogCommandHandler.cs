using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Blogs.Commands;
using DevLog.Application.Features.Blogs.Result;
using DevLog.Domain.Entities;
using DevLog.Domain.Entities.Common;

namespace DevLog.Application.Features.Blogs.Handlers;

// CreateBlogCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class CreateBlogCommandHandler(IRepository<Blog> repository, IRepository<Tag> tagRepository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateBlogCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        // request'i entity'e çeviriyorum
        var blog = mapper.Map<Blog>(request);
        // İngilizce alanlar doluysa çeviri olarak ekleniyor
        blog.SetTranslation(Languages.English, request.TitleEn, request.DescriptionEn);
        // seçilen etiketleri bağlıyorum
        // NOT: GetQuery() tracking açık sorgu, etiketler context tarafından takip ediliyor.
        // AsNoTracking ile gelseydi EF onları yeni kayıt sanıp tekrar eklemeye çalışabilirdi
        var tagIds = request.TagIds ?? new List<Guid>();
        blog.Tags = tagRepository.GetQuery().Where(x => tagIds.Contains(x.Id)).ToList();
        // context'e ekliyor ama daha veritabanına yazmadı
        await repository.CreateAsync(blog);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        // NOT: burada önceden entity'nin kendisini (blog) dönüyordum. JSON'a çevrilirken
        // blog.Tags -> etiketin bütün blogları -> onların yorumları... diye lazy loading zincirleme
        // neredeyse bütün veritabanını yüklüyordu, yeni yazı eklemek saniyeler sürüyordu.
        // entity yerine dto dönünce sadece gereken alanlar gidiyor
        return BaseResult<object>.Success(mapper.Map<GetBlogsQueryResult>(blog));
    }
}
