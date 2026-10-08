using AutoMapper;
using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Categories.Commands;
using DevLog.Domain.Entities;
using DevLog.Domain.Entities.Common;

namespace DevLog.Application.Features.Categories.Handlers
{
    // CreateCategoryCommand gönderilince MediatR bu handler'ı çalıştırıyor
    public class CreateCategoryCommandHandler(IRepository<Category> _repository,
                                              IUnitOfWork _unitOfWork,
                                              IMapper _mapper) : IRequestHandler<CreateCategoryCommand, BaseResult<object>>
    {
        public async Task<BaseResult<object>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // request'i entity'e çeviriyorum
            var category = _mapper.Map<Category>(request);
            category.SetTranslation(Languages.English, request.CategoryNameEn);
            // context'e ekliyor ama daha veritabanına yazmadı
            await _repository.CreateAsync(category);
            // veritabanına kaydetme işi burada oluyor (unit of work)
            await _unitOfWork.SaveChangesAsync();
            return BaseResult<object>.Success(category);
        }
    }
}
