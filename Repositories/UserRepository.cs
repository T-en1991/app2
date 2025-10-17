using Microsoft.EntityFrameworkCore;
using app2.Models;

namespace app2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly app2.AppDbContext _db;

        public UserRepository(app2.AppDbContext db)
        {
            _db = db;
        }

        public Task<User?> GetByIdAsync(int id)
        {
            return _db.Users.FirstOrDefaultAsync(u => u.id == id && !u.is_delete);
        }

        public Task<User?> GetByNameAsync(string name)
        {
            return _db.Users.FirstOrDefaultAsync(u => u.name == name && !u.is_delete);
        }

        public async Task AddAsync(User user)
        {
            await _db.Users.AddAsync(user);
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}