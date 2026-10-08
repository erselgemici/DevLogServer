using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.ContactInfos.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.ContactInfos.Handlers;

// CreateContactInfoCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class CreateContactInfoCommandHandler(IRepository<ContactInfo> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateContactInfoCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(CreateContactInfoCommand request, CancellationToken cancellationToken)
    {
        // request'i entity'e çeviriyorum
        var entity = mapper.Map<ContactInfo>(request);
        // context'e ekliyor ama daha veritabanına yazmadı
        await repository.CreateAsync(entity);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success(entity);
    }
}
