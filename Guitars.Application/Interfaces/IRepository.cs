namespace Guitars.Application.Interfaces
{
    public interface IRepository
    {
        void SaveChanges();

        Task SaveChangesAsync();
    }
}