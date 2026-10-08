using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Blogs.Commands;
using DevLog.Domain.Entities;
using DevLog.Domain.Entities.Common;

namespace DevLog.Application.Features.Blogs.Handlers;

// UpdateBlogCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class UpdateBlogCommandHandler(IRepository<Blog> repository, IRepository<Tag> tagRepository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBlogCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        // çeviri ve etiketleri de güncelleyeceğim için onları da yüklüyorum
        // NOT: lazy loading kapalı, Include etmezsem Translations/Tags boş gelir ve EF var olan
        // kayıtları göremeyip aynı çeviriyi tekrar eklemeye çalışır (unique index hatası)
        var blog = repository.GetQueryWithIncludes("Translations", "Tags")
            .FirstOrDefault(x => x.Id == request.Id);
        // kayıt yoksa hata dönüyorum
        if (blog is null)
        {
            return BaseResult<object>.NotFound("Blog bulunamadı");
        }

        // gelen yeni değerleri var olan kaydın üstüne yazıyor
        // yazar sadece kendi yazısını düzenleyebilir, admin hepsini
        if (!request.IsAdmin && blog.UserId != request.CurrentUserId)
        {
            return BaseResult<object>.Fail("Sadece kendi yazılarınızı düzenleyebilirsiniz");
        }

        // güncellemede yazarı değişmesin
        var authorId = blog.UserId;
        mapper.Map(request, blog);
        blog.UserId = authorId;
        blog.SetTranslation(Languages.English, request.TitleEn, request.DescriptionEn);

        // etiketleri güncelliyorum: önce hepsini kaldırıp seçilenleri tekrar ekliyorum
        // (ara tablodaki kayıtları EF kendisi siliyor/ekliyor)
        var tagIds = request.TagIds ?? new List<Guid>();
        blog.Tags ??= new List<Tag>();
        blog.Tags.Clear();
        foreach (var tag in tagRepository.GetQuery().Where(x => tagIds.Contains(x.Id)).ToList())
        {
            blog.Tags.Add(tag);
        }
        repository.Update(blog);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("Blog güncellendi");
    }
}
