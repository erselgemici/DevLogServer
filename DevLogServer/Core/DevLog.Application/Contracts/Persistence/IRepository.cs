using System.Linq.Expressions;
using DevLog.Domain.Entities.Common;

namespace DevLog.Application.Contracts.Persistence;

// generic repository, her entity için ayrı repository yazmamak için
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<List<TEntity>> GetAllAsync();

    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter);

    // kendi sorgumu yazmam gerektiğinde kullanıyorum (ör: son 5 blog)
    IQueryable<TEntity> GetQuery();

    // ilişkili tabloları da sorguya eklemek için (Include)
    // örnek: GetQueryWithIncludes("User", "Category.Translations")
    // nokta ile iç içe gidebiliyorsun: Category.Translations = blogun kategorisinin çevirileri
    // NOT: Application katmanında EF Core paketi yok, o yüzden Include'u direkt burada yazamıyorum,
    // repository'nin içine koydum (Include metodu Persistence'ta çalışıyor)
    IQueryable<TEntity> GetQueryWithIncludes(params string[] includes);

    Task<TEntity> GetByIdAsync(Guid id);

    Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter);

    Task CreateAsync(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);
}
