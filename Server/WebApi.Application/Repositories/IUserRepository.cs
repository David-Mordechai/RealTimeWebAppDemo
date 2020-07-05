using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Domain.Entities;

namespace WebApi.Application.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task AddUserAsync(User user);
        Task<User> GetUserById(int userId);
        Task UpdateUser(User user);
    }
}
