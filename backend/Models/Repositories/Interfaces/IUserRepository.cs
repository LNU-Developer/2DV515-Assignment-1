using Backend.Models.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Backend.Models.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetAllUsersDataExceptSelected(User selectedUser);
        Task<User> GetUserById(int userId);

    }
}
