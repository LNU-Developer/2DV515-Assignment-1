using Backend.Models.Database;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
namespace Backend.Models.Repositories
{
    public class MovieRepository : Repository<Context, Movie>, IMovieRepository
    {
        public MovieRepository(Context context) : base(context)
        {
        }

        public async Task<Movie> GetMovieById(int movieId)
        {
            return await _context.Movies
                   .Include(x => x.Ratings)
                   .FirstOrDefaultAsync(x => x.MovieId == movieId);
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            return await _context.Movies.Include(x => x.Ratings).ThenInclude(y => y.User).ToListAsync();
        }
        public async Task<List<Movie>> GetAllSeenMoviesByUserId(int userId)
        {
            return await _context.Movies.Include(x => x.Ratings).ThenInclude(y => y.User).Where(z => z.Ratings.Any(o => o.UserId == userId)).ToListAsync();
        }

        public async Task<List<Movie>> GetAllMoviesDataExceptSelected(Movie selectedMovie)
        {
            return await _context.Movies
                   .Include(x => x.Ratings)
                   .ThenInclude(x => x.User)
                   .Where(x => x.MovieId != selectedMovie.MovieId) //We need to exclude the selected user.
                   .ToListAsync();
        }
    }
}