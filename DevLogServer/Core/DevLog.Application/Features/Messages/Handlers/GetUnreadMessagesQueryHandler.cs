using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Messages.Queries;
using DevLog.Application.Features.Messages.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Messages.Handlers;

// GetUnreadMessagesQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetUnreadMessagesQueryHandler(IRepository<Message> repository, IMapper mapper)
    : IRequestHandler<GetUnreadMessagesQuery, BaseResult<List<GetUnreadMessagesQueryResult>>>
{
    public async Task<BaseResult<List<GetUnreadMessagesQueryResult>>> Handle(GetUnreadMessagesQuery request, CancellationToken cancellationToken)
    {
        // sadece şarta uyan kayıtlar geliyor
        var messages = await repository.GetAllAsync(x => x.IsRead == false);
        // entity listesini dto listesine çeviriyorum
        var result = mapper.Map<List<GetUnreadMessagesQueryResult>>(messages);
        return BaseResult<List<GetUnreadMessagesQueryResult>>.Success(result);
    }
}
