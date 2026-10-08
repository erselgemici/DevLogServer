using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Socials.Queries;
using DevLog.Application.Features.Socials.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Socials.Handlers;

// GetSocialByIdQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetSocialByIdQueryHandler(IRepository<Social> repository, IMapper mapper)
    : IRequestHandler<GetSocialByIdQuery, BaseResult<GetSocialByIdQueryResult>>
{
    public async Task<BaseResult<GetSocialByIdQueryResult>> Handle(GetSocialByIdQuery request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var value = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (value is null)
        {
            return BaseResult<GetSocialByIdQueryResult>.NotFound("Sosyal medya kaydı bulunamadı");
        }

        // entity'i dto'ya çeviriyorum
        var result = mapper.Map<GetSocialByIdQueryResult>(value);
        return BaseResult<GetSocialByIdQueryResult>.Success(result);
    }
}
