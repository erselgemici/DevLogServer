using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DevLog.Application.Contracts.Persistence;
using DevLog.Domain.Entities.Common;
using DevLog.Persistence.Context;

namespace DevLog.Persistence.Concrete;

// IRepository'nin EF Core ile yazılmış hali
public class GenericRepository<TEntity>(AppDbContext _context) : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _table = _context.Set<TEntity>();

    public async Task CreateAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
    }

    public void Delete(TEntity entity)
    {
        _context.Remove(entity);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        // sadece okuma yaptığım için tracking'e gerek yok, daha hızlı
        return await _table.AsNoTracking().ToListAsync();
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _table.AsNoTracking().Where(filter).ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        // FindAsync önce context'e bakıyor, yoksa db'ye gidiyor
        return await _table.FindAsync(id);
    }

    public IQueryable<TEntity> GetQuery()
    {
        return _table;
    }

    public IQueryable<TEntity> GetQueryWithIncludes(params string[] includes)
    {
        IQueryable<TEntity> query = _table;

        // NOT (N+1 problemi): lazy loading açık olduğu için listede blog.User, blog.Category
        // gibi alanlara her erişildiğinde EF ayrı bir sorgu atıyordu.
        // 87 blog varken tek sayfa için yüzlerce sorgu = sayfa geç açılıyordu.
        // Include ile bu tabloları JOIN'leyip baştan beraber çekiyorum, sorgu sayısı birkaça iniyor
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // AsSplitQuery: birden fazla liste (Blogs, Translations...) include edince tek büyük JOIN
        // satırları çoğaltıyor (cartesian explosion). split query her liste için ayrı küçük sorgu atıyor
        // ToList() çağrılana kadar sorgu çalışmıyor, IQueryable döndüğü için handler'da
        // Where / OrderBy / Take eklenebiliyor ve hepsi SQL'e çevriliyor
        return query.AsSplitQuery();
    }

    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _table.AsNoTracking().FirstOrDefaultAsync(filter);
    }

    public void Update(TEntity entity)
    {
        _context.Update(entity);
    }
}
