using Backend.Models.Database;
using System.Threading.Tasks;

namespace Backend.Models.Repositories
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<Rating> GetUserRatingByMovieId(int movieId, int userId);
    }
}
