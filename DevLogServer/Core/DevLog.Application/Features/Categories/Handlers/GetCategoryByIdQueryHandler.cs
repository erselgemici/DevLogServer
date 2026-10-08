using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Categories.Queries;
using DevLog.Application.Features.Categories.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Categories.Handlers
{
    // GetCategoryByIdQuery gönderilince MediatR bu handler'ı çalıştırıyor
    public class GetCategoryByIdQueryHandler(IRepository<Category> repository,IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, BaseResult<GetCategoryByIdQueryResult>>
    {
        public Task<BaseResult<GetCategoryByIdQueryResult>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            // önce kaydı id ile buluyorum
            // İngilizce ad çeviri tablosunda, onu da Include ediyorum
            var category = repository.GetQueryWithIncludes("Translations")
                .FirstOrDefault(x => x.Id == request.Id);
            // kayıt yoksa hata dönüyorum
            if (category == null) 
            {
                return Task.FromResult(BaseResult<GetCategoryByIdQueryResult>.NotFound("Kategori bulunamadı"));
            }

            // entity'i dto'ya çeviriyorum
            var response = mapper.Map<GetCategoryByIdQueryResult>(category);

            return Task.FromResult(BaseResult<GetCategoryByIdQueryResult>.Success(response));

        }
    }
}
