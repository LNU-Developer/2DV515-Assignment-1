using Backend.Models.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
namespace Backend.Models.Repositories
{
    public class UserRepository : Repository<Context, User>, IUserRepository
    {
        public UserRepository(Context context) : base(context)
        {
        }
        public async Task<List<User>> GetAllUsersDataExceptSelected(User selectedUser)
        {
            var users = await _context.Users
                   .Include(x => x.Ratings)
                   .ThenInclude(x => x.Movie)
                   .Where(x => x.UserId != selectedUser.UserId) //We need to exclude the selected user.
                   .ToListAsync();
            return users;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users
                   .Include(x => x.Ratings)
                   .ThenInclude(x => x.Movie)
                   .FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}