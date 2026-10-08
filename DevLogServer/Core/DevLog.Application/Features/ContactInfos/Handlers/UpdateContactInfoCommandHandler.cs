using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.ContactInfos.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.ContactInfos.Handlers;

// UpdateContactInfoCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class UpdateContactInfoCommandHandler(IRepository<ContactInfo> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateContactInfoCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(UpdateContactInfoCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var entity = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (entity is null)
        {
            return BaseResult<object>.NotFound("İletişim bilgisi bulunamadı");
        }

        // gelen yeni değerleri var olan kaydın üstüne yazıyor
        mapper.Map(request, entity);
        repository.Update(entity);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("İletişim bilgisi güncellendi");
    }
}
