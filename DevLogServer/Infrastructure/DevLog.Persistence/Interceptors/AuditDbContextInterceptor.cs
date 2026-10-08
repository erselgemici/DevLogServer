using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using DevLog.Domain.Entities.Common;

namespace DevLog.Persistence.Interceptors
{
    // kaydetmeden hemen önce CreatedAt ve UpdatedAt'i otomatik dolduruyor, handler'larda elle yazmıyorum
    public class AuditDbContextInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            SetAuditFields(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            SetAuditFields(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        private static void SetAuditFields(DbContext? context)
        {
            if (context is null) return;

            var now = DateTime.Now;

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    // yeni kayıt
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.UpdatedAt = now;
                        break;

                    // güncellenen kayıt, CreatedAt değişmesin
                    case EntityState.Modified:
                        entry.Property(x => x.CreatedAt).IsModified = false;
                        entry.Entity.UpdatedAt = now;
                        break;
                }
            }
        }
    }
}
