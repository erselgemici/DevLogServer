using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.SubComments.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.SubComments.Handlers;

// UpdateSubCommentCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class UpdateSubCommentCommandHandler(IRepository<SubComment> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSubCommentCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(UpdateSubCommentCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var subComment = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (subComment is null)
        {
            return BaseResult<object>.NotFound("Yanıt bulunamadı");
        }

        // gelen yeni değerleri var olan kaydın üstüne yazıyor
        mapper.Map(request, subComment);
        repository.Update(subComment);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("Yanıt güncellendi");
    }
}
