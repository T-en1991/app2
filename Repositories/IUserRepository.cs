using app2.Models;

namespace app2.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByNameAsync(string name);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}