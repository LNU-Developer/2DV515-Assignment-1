using Microsoft.EntityFrameworkCore;
using Backend.Models.Database;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Backend.Models.Services
{
    public abstract class RecommendationSystemService
    {
        private readonly Context _context;

        public RecommendationSystemService(Context context)
        {
            _context = context;
        }

        public async Task<List<RatingWeightedScore>> CalculateRatingWeightedScores(User selectedUser, List<Similarity> similarites)
        {
            var users = await GetAllUsersDataExceptSelected(selectedUser);

            var weightedScores = new List<RatingWeightedScore>();
            foreach (var user in users)
            {
                double similarityScore = similarites.FirstOrDefault(x => x.UserId == user.UserId).SimilarityScore;
                if (similarityScore > 0) // Only simalrities above zero are interesting, not negative or zero.
                    foreach (var rating in user.Ratings.Where(x => selectedUser.Ratings.All(y => y.MovieId != x.MovieId)))
                    {
                        weightedScores.Add(new RatingWeightedScore
                        {
                            MovieId = rating.MovieId,
                            WeightedScore = similarityScore * rating.Score
                        });
                    }
            }
            return weightedScores;
        }

        public async Task<List<Recommendation>> FindBestMovies(List<Similarity> similarites, List<RatingWeightedScore> weightedScores)
        {
            var movies = await GetAllMovies();

            var recommendationList = new List<Recommendation>();
            foreach (var movie in movies)
            {
                var allUsersThatRatedMovie = movie.Ratings.Where(x => x.MovieId == movie.MovieId).Select(y => y.User).ToList();
                var sumOfSimilarities = 0.0;

                //Sum all similaritiescores on users that have voted on that movie
                foreach (var user in allUsersThatRatedMovie)
                    sumOfSimilarities += similarites.Where(y => y.UserId == user.UserId).Sum(x => x.SimilarityScore);

                //Sum all weighted scores for that movie
                var sumOfMovieScores = weightedScores.Where(x => x.MovieId == movie.MovieId).Sum(y => y.WeightedScore);
                recommendationList.Add(new Recommendation
                {
                    MovieId = movie.MovieId,
                    MovieTitle = movie.MovieTitle,
                    MovieYear = movie.MovieYear,
                    AverageWeightedRating = sumOfMovieScores / sumOfSimilarities
                });
            }
            return recommendationList.OrderByDescending(x => x.AverageWeightedRating).ToList();
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

        public async Task<List<Movie>> GetAllMovies()
        {
            return await _context.Movies.Include(x => x.Ratings).ThenInclude(y => y.User).ToListAsync();
        }
    }
}