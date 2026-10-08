using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.ContactInfos.Queries;
using DevLog.Application.Features.ContactInfos.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.ContactInfos.Handlers;

// GetContactInfoByIdQuery gönderilince MediatR bu handler'ı çalıştırıyor
public class GetContactInfoByIdQueryHandler(IRepository<ContactInfo> repository, IMapper mapper)
    : IRequestHandler<GetContactInfoByIdQuery, BaseResult<GetContactInfoByIdQueryResult>>
{
    public async Task<BaseResult<GetContactInfoByIdQueryResult>> Handle(GetContactInfoByIdQuery request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var value = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (value is null)
        {
            return BaseResult<GetContactInfoByIdQueryResult>.NotFound("İletişim bilgisi bulunamadı");
        }

        // entity'i dto'ya çeviriyorum
        var result = mapper.Map<GetContactInfoByIdQueryResult>(value);
        return BaseResult<GetContactInfoByIdQueryResult>.Success(result);
    }
}
