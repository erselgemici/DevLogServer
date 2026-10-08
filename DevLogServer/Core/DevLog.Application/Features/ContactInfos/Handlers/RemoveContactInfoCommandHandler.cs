using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.ContactInfos.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.ContactInfos.Handlers;

// RemoveContactInfoCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class RemoveContactInfoCommandHandler(IRepository<ContactInfo> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveContactInfoCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(RemoveContactInfoCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var entity = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (entity is null)
        {
            return BaseResult<object>.NotFound("İletişim bilgisi bulunamadı");
        }

        // silinecek olarak işaretliyor, SaveChanges ile siliniyor
        repository.Delete(entity);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("İletişim bilgisi silindi");
    }
}
