using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Socials.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Socials.Handlers;

// UpdateSocialCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class UpdateSocialCommandHandler(IRepository<Social> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSocialCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(UpdateSocialCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var entity = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (entity is null)
        {
            return BaseResult<object>.NotFound("Sosyal medya kaydı bulunamadı");
        }

        // gelen yeni değerleri var olan kaydın üstüne yazıyor
        mapper.Map(request, entity);
        repository.Update(entity);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("Sosyal medya kaydı güncellendi");
    }
}
