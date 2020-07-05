using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Application.Repositories;
using WebApi.Domain.Entities;

namespace WebApi.Persistent.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddUserAsync(User user)
        {
            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _appDbContext.Users.ToListAsync();
        }

        public async Task UpdateUser(User user)
        {
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
