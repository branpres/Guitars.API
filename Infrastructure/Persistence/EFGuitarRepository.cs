using Guitars.Application.Interfaces;
using Guitars.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class EFGuitarRepository : EFRepository, IGuitarRepository
    {
        public EFGuitarRepository(DbContext dbContext) : base(dbContext) { }

        public async Task CreateAsync(Guitar guitar)
        {
            await DbContext.Guitar.AddAsync(guitar);
        }

        public async Task<Guitar?> ReadAsync(int id)
        {
            return await DbContext.Guitar
                .Include(x => x.GuitarStrings)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Guitar>> ReadAllAsync()
        {
            return await DbContext.Guitar
                .Include(x => x.GuitarStrings)
                .ToListAsync();
        }

        public async Task<Guitar?> FindAsync(int id)
        {
            return await DbContext.Guitar.FindAsync(id);
        }

        public void Update(Guitar guitar)
        {
            DbContext.Guitar.Update(guitar);
        }

        public void Delete(Guitar guitar)
        {
            DbContext.Guitar.Remove(guitar);
        }
    }
}