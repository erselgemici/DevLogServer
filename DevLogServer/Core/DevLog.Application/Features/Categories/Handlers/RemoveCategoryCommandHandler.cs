using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Categories.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Categories.Handlers;

// RemoveCategoryCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class RemoveCategoryCommandHandler(IRepository<Category> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveCategoryCommand, BaseResult<bool>>
{
    public async Task<BaseResult<bool>> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var category = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (category is null)
        {
            return BaseResult<bool>.NotFound("Kategori bulunamadı");
        }

        // silinecek olarak işaretliyor, SaveChanges ile siliniyor
        repository.Delete(category);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        var response = await unitOfWork.SaveChangesAsync();

        return response
            ? BaseResult<bool>.Success(response)
            : BaseResult<bool>.Fail("Kategori silinemedi");
    }
}
