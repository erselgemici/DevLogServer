using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Messages.Queries;
using DevLog.Application.Features.Messages.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Messages.Handlers;

// GetMessagesQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetMessagesQueryHandler(IRepository<Message> repository, IMapper mapper)
    : IRequestHandler<GetMessagesQuery, BaseResult<List<GetMessagesQueryResult>>>
{
    public async Task<BaseResult<List<GetMessagesQueryResult>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        // bütün kayıtları çekiyorum
        var values = await repository.GetAllAsync();
        // entity listesini dto listesine çeviriyorum
        var result = mapper.Map<List<GetMessagesQueryResult>>(values);
        return BaseResult<List<GetMessagesQueryResult>>.Success(result);
    }
}
