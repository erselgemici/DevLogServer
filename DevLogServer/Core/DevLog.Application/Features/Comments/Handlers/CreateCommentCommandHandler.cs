using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Comments.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Comments.Handlers;

// CreateCommentCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class CreateCommentCommandHandler(IRepository<Comment> repository, IMapper mapper, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCommentCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        // request'i entity'e çeviriyorum
        var comment = mapper.Map<Comment>(request);
        // context'e ekliyor ama daha veritabanına yazmadı
        await repository.CreateAsync(comment);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success(comment);
    }
}
