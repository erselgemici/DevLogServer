using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.SubComments.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.SubComments.Handlers;

// CreateSubCommentCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class CreateSubCommentCommandHandler(IRepository<SubComment> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSubCommentCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(CreateSubCommentCommand request, CancellationToken cancellationToken)
    {
        // request'i entity'e çeviriyorum
        var subComment = mapper.Map<SubComment>(request);
        // context'e ekliyor ama daha veritabanına yazmadı
        await repository.CreateAsync(subComment);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success(subComment);
    }
}
