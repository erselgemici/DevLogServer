namespace DevLog.Application.Contracts.Persistence
{
    // kaydetme işini tek yerden yapmak için
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync();
    }
}
