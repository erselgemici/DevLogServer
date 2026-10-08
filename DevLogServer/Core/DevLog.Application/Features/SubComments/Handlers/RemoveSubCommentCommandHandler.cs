using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.SubComments.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.SubComments.Handlers;

// RemoveSubCommentCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class RemoveSubCommentCommandHandler(IRepository<SubComment> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveSubCommentCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(RemoveSubCommentCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var subComment = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (subComment is null)
        {
            return BaseResult<object>.NotFound("Yanıt bulunamadı");
        }

        // silinecek olarak işaretliyor, SaveChanges ile siliniyor
        repository.Delete(subComment);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("Yanıt silindi");
    }
}
