using MediatR;
using DevLog.Application.Base;
using DevLog.Application.Contracts.Persistence;
using DevLog.Application.Features.Blogs.Commands;
using DevLog.Domain.Entities;

namespace DevLog.Application.Features.Blogs.Handlers;

// RemoveBlogCommand gönderilince MediatR bu handler'ı çalıştırıyor
public class RemoveBlogCommandHandler(IRepository<Blog> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoveBlogCommand, BaseResult<object>>
{
    public async Task<BaseResult<object>> Handle(RemoveBlogCommand request, CancellationToken cancellationToken)
    {
        // önce kaydı id ile buluyorum
        var blog = await repository.GetByIdAsync(request.Id);
        // kayıt yoksa hata dönüyorum
        if (blog is null)
        {
            return BaseResult<object>.NotFound("Blog bulunamadı");
        }

        // silinecek olarak işaretliyor, SaveChanges ile siliniyor
        // yazar sadece kendi yazısını silebilir, admin hepsini
        if (!request.IsAdmin && blog.UserId != request.CurrentUserId)
        {
            return BaseResult<object>.Fail("Sadece kendi yazılarınızı silebilirsiniz");
        }

        repository.Delete(blog);
        // veritabanına kaydetme işi burada oluyor (unit of work)
        await unitOfWork.SaveChangesAsync();
        return BaseResult<object>.Success("Blog silindi");
    }
}
