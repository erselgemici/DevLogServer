using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Messages.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Messages.Handlers;

// RemoveMessageCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class RemoveMessageCommandHandler(IRepository<Message> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveMessageCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(RemoveMessageCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var entity = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (entity is null)
        {
            return BaseResult<object>.NotFound("Mesaj bulunamadı");
        }

        // silinecek olarak işaretliyor, SaveChanges ile siliniyor
        repository.Delete(entity);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("Mesaj silindi");
    }
}
