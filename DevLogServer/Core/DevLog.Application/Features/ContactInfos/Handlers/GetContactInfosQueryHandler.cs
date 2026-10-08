using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.ContactInfos.Queries;
using DevLog.Application.Features.ContactInfos.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.ContactInfos.Handlers;

// GetContactInfosQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetContactInfosQueryHandler(IRepository<ContactInfo> repository, IMapper mapper)
    : IRequestHandler<GetContactInfosQuery, BaseResult<List<GetContactInfosQueryResult>>>
{
    public async Task<BaseResult<List<GetContactInfosQueryResult>>> Handle(GetContactInfosQuery request, CancellationToken cancellationToken)
    {
        // bütün kayıtları çekiyorum
        var values = await repository.GetAllAsync();
        // entity listesini dto listesine çeviriyorum
        var result = mapper.Map<List<GetContactInfosQueryResult>>(values);
        return BaseResult<List<GetContactInfosQueryResult>>.Success(result);
    }
}
