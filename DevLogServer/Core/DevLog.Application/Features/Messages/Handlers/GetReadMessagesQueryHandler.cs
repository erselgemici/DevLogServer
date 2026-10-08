using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Messages.Queries;
using DevLog.Application.Features.Messages.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Messages.Handlers;

// GetReadMessagesQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetReadMessagesQueryHandler(IRepository<Message> repository, IMapper mapper)
    : IRequestHandler<GetReadMessagesQuery, BaseResult<List<GetReadMessagesQueryResult>>>
{
    public async Task<BaseResult<List<GetReadMessagesQueryResult>>> Handle(GetReadMessagesQuery request, CancellationToken cancellationToken)
    {
        // sadece şarta uyan kayıtlar geliyor
        var messages = await repository.GetAllAsync(x => x.IsRead == true);
        // entity listesini dto listesine çeviriyorum
        var result = mapper.Map<List<GetReadMessagesQueryResult>>(messages);
        return BaseResult<List<GetReadMessagesQueryResult>>.Success(result);
    }
}
