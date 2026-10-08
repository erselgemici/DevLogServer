using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Socials.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Socials.Handlers;

// RemoveSocialCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class RemoveSocialCommandHandler(IRepository<Social> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveSocialCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(RemoveSocialCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var entity = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (entity is null)
        {
            return BaseResult<object>.NotFound("Sosyal medya kaydı bulunamadı");
        }

        // silinecek olarak işaretliyor, SaveChanges ile siliniyor
        repository.Delete(entity);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("Sosyal medya kaydı silindi");
    }
}
