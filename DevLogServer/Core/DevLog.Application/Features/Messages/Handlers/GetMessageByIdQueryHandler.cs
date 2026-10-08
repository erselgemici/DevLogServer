using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Messages.Queries;
using DevLog.Application.Features.Messages.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Messages.Handlers;

// GetMessageByIdQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetMessageByIdQueryHandler(IRepository<Message> repository, IMapper mapper)
    : IRequestHandler<GetMessageByIdQuery, BaseResult<GetMessageByIdQueryResult>>
{
    public async Task<BaseResult<GetMessageByIdQueryResult>> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var value = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (value is null)
        {
            return BaseResult<GetMessageByIdQueryResult>.NotFound("Mesaj bulunamadı");
        }

        // entity'i dto'ya çeviriyorum
        var result = mapper.Map<GetMessageByIdQueryResult>(value);
        return BaseResult<GetMessageByIdQueryResult>.Success(result);
    }
}
