using Guitars.Application.Interfaces;
using Guitars.Domain.Models;

namespace Infrastructure.Persistence
{
    public class InMemoryGuitarRepository : IGuitarRepository
    {
        private readonly Dictionary<int, Guitar> _guitars = new();
        private int id = 0;

        public Task CreateAsync(Guitar guitar)
        {
            id++;
            guitar.Id = id;
            _guitars.Add(id, guitar);

            return Task.CompletedTask;
        }

        public Task<Guitar?> ReadAsync(int id) => Task.FromResult(_guitars[id] ?? null);

        public Task<List<Guitar>> ReadAllAsync() => Task.FromResult(_guitars.Values.ToList());

        public Task<Guitar?> FindAsync(int id) => Task.FromResult(_guitars[id] ?? null);

        public void Update(Guitar guitar) => _guitars[guitar.Id] = guitar;

        public void Delete(Guitar guitar) => _guitars.Remove(guitar.Id);

        public static void SaveChanges() { }

        public static Task SaveChangesAsync() => Task.CompletedTask;
    }
}