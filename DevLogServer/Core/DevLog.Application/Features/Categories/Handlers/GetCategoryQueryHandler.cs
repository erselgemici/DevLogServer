using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Categories.Queries;
using DevLog.Application.Features.Categories.Result;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Categories.Handlers
{
    // GetCategoryQuery gönderilince MediatR bu handler'ı çalıştırıyor
    public class GetCategoryQueryHandler(IRepository<Category> _repository, IMapper _mapper) : IRequestHandler<GetCategoryQuery, BaseResult<List<GetCategoryQueryResult>>>
    {
        public Task<BaseResult<List<GetCategoryQueryResult>>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            // ana sayfa ve footer bu endpoint'i kullanıyor, en çok çağrılan yer burası
            // kategori -> blogları -> blogların yazarı ve çevirileri, hepsi Include ile tek seferde
            var categories = _repository.GetQueryWithIncludes("Translations", "Blogs", "Blogs.Translations", "Blogs.User")
                .ToList();

            // her kategorinin blogları yeniden eskiye sıralı gelsin
            // Include edilen listeye OrderBy yazamadığım için sıralamayı burada (bellekte) yapıyorum
            foreach (var category in categories)
            {
                category.Blogs = category.Blogs.OrderByDescending(x => x.CreatedAt).ToList();
            }

            // entity listesini dto listesine çeviriyorum
            var response = _mapper.Map<List<GetCategoryQueryResult>>(categories);

            return Task.FromResult(BaseResult<List<GetCategoryQueryResult>>.Success(response));
        }
    }
}
