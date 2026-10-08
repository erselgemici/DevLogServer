using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Socials.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Socials.Handlers;

// CreateSocialCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class CreateSocialCommandHandler(IRepository<Social> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSocialCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(CreateSocialCommand request, CancellationToken cancellationToken)
    {
        // request'i entity'e çeviriyorum
        var entity = mapper.Map<Social>(request);
        // context'e ekliyor ama daha veritabanına yazmadı
        await repository.CreateAsync(entity);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success(entity);
    }
}
