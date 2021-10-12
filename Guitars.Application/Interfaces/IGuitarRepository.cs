using Guitars.Domain.Models;

namespace Guitars.Application.Interfaces
{
    public interface IGuitarRepository
    {
        Task CreateAsync(Guitar guitar);
        Task<Guitar?> ReadAsync(int id);
        Task<List<Guitar>> ReadAllAsync();
        Task<Guitar?> FindAsync(int id);
        void Update(Guitar guitar);
        void Delete(Guitar guitar);
    }
}