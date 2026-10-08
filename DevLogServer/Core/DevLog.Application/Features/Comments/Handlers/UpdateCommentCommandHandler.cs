using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Comments.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Comments.Handlers;

// UpdateCommentCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class UpdateCommentCommandHandler(IRepository<Comment> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateCommentCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var comment = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (comment is null)
        {
            return BaseResult<object>.NotFound("Yorum bulunamadı");
        }

        // gelen yeni değerleri var olan kaydın üstüne yazıyor
        mapper.Map(request, comment);
        repository.Update(comment);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("Yorum güncellendi");
    }
}
