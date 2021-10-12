using Guitars.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Guitars.Infrastructure.Persistence
{
    public abstract class EFRepository : IRepository
    {
        protected readonly GuitarsContext DbContext;

        protected EFRepository(DbContext dbContext)
        {
            DbContext = (GuitarsContext)dbContext;
        }

        public void SaveChanges() => DbContext.SaveChanges();

        public async Task SaveChangesAsync() => await DbContext.SaveChangesAsync();
    }
}