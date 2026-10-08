using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Messages.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Messages.Handlers;

// CreateMessageCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class CreateMessageCommandHandler(IRepository<Message> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMessageCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        // request'i entity'e çeviriyorum
        var entity = mapper.Map<Message>(request);
        // context'e ekliyor ama daha veritabanına yazmadı
        await repository.CreateAsync(entity);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success(entity);
    }
}
