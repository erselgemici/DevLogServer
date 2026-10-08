using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Socials.Queries;
using DevLog.Application.Features.Socials.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Socials.Handlers;

// GetSocialsQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetSocialsQueryHandler(IRepository<Social> repository, IMapper mapper)
    : IRequestHandler<GetSocialsQuery, BaseResult<List<GetSocialsQueryResult>>>
{
    public async Task<BaseResult<List<GetSocialsQueryResult>>> Handle(GetSocialsQuery request, CancellationToken cancellationToken)
    {
        // bütün kayıtları çekiyorum
        var values = await repository.GetAllAsync();
        // entity listesini dto listesine çeviriyorum
        var result = mapper.Map<List<GetSocialsQueryResult>>(values);
        return BaseResult<List<GetSocialsQueryResult>>.Success(result);
    }
}
