using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Comments.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Comments.Handlers;

// RemoveCommentCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class RemoveCommentCommandHandler(IRepository<Comment> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveCommentCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var comment = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (comment is null)
        {
            return BaseResult<object>.NotFound("Yorum bulunamadı");
        }

        // silinecek olarak işaretliyor, SaveChanges ile siliniyor
        repository.Delete(comment);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("Yorum silindi");
    }
}
