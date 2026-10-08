using DevLog.Application.Contracts.Persistence;
using DevLog.Persistence.Context;

namespace DevLog.Persistence.Concrete
{
    public class UnitOfWork(AppDbContext _context) : IUnitOfWork
    {
        // etkilenen satır sayısı 0'dan büyükse true dönüyor
        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
