using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Categories.Commands;
using DevLog.Domain.Entities;
using DevLog.Domain.Entities.Common;

namespace DevLog.Application.Features.Categories.Handlers;

// UpdateCategoryCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class UpdateCategoryCommandHandler(IRepository<Category> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCategoryCommand, BaseResult<bool>>
{
    public async Task<BaseResult<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        // İngilizce adı da güncellediğim için çevirileriyle beraber yüklüyorum (lazy loading kapalı)
        var category = repository.GetQueryWithIncludes("Translations")
            .FirstOrDefault(x => x.Id == request.Id);
        // kayıt yoksa hata dönüyorum
        if (category is null)
        {
            return BaseResult<bool>.NotFound("Kategori bulunamadı");
        }

        // gelen yeni değerleri var olan kaydın üstüne yazıyor
        mapper.Map(request, category);
        category.SetTranslation(Languages.English, request.CategoryNameEn);
        repository.Update(category);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        var response = await unitOfWork.SaveChangesAsync();

        return response
            ? BaseResult<bool>.Success(response)
            : BaseResult<bool>.Fail("Kategori güncellenemedi");
    }
}
