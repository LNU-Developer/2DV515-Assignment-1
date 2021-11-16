using Backend.Models.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Backend.Models.Repositories
{
    public class RatingRepository : Repository<Context, Rating>, IRatingRepository
    {
        public RatingRepository(Context context) : base(context)
        {
        }

        public async Task<Rating> GetUserRatingByMovieId(int movieId, int userId)
        {
            return await _context.Ratings.Include(x => x.User).Include(x => x.Movie).FirstOrDefaultAsync(x => x.Movie.MovieId == movieId && x.User.UserId == userId);
        }
    }
}