using Backend.Models.Database;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models.Repositories;
using Backend.Models.Recommendation;

namespace Backend.Models.Services
{
    public abstract class UserBasedCollaborativeFilteringService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserBasedCollaborativeFilteringService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MovieRecommendation>> FindKMovieRecommendation(int selectedUserId, int k = 3)
        {
            var selectedUser = await _unitOfWork.Users.GetUserById(selectedUserId);
            var similarites = await CalculateSimilarityScores(selectedUser);
            var weightedScores = await CalculateRatingWeightedScores(selectedUser, similarites);
            var bestMovies = await FindBestMovies(similarites, weightedScores);
            return bestMovies.Where(x => x.AverageWeightedRating > 0).Take(k).ToList();
        }

        public async Task<List<Similarity>> FindKTopSimilar(int selectedUserId, int k = 3)
        {
            var selectedUser = await _unitOfWork.Users.GetUserById(selectedUserId);
            var similarites = await CalculateSimilarityScores(selectedUser);
            return similarites.Where(x => x.SimilarityScore > 0).Take(k).ToList();
        }

        public abstract double CalculateDistance(User A, User B);

        public async Task<List<Similarity>> CalculateSimilarityScores(User selectedUser)
        {
            var users = await _unitOfWork.Users.GetAllUsersDataExceptSelected(selectedUser);

            var similarityList = new List<Similarity>();
            foreach (var user in users)
            {
                double distance = CalculateDistance(selectedUser, user);
                if (distance < 0) continue; //Not interested dissimilar scores, i.e only show scores with similarites
                similarityList.Add(new Similarity
                {
                    Id = user.UserId,
                    Name = user.UserName,
                    SimilarityScore = distance
                });
            }
            return similarityList;
        }

        public async Task<List<RatingWeightedScore>> CalculateRatingWeightedScores(User selectedUser, List<Similarity> similarites)
        {
            var users = await _unitOfWork.Users.GetAllUsersDataExceptSelected(selectedUser);

            var weightedScores = new List<RatingWeightedScore>();
            foreach (var user in users)
            {
                var similarityUser = similarites.FirstOrDefault(x => x.Id == user.UserId);
                if (similarityUser is not null) // Only simalrities above zero are interesting, not negative or zero.
                {
                    var similarityScore = similarityUser.SimilarityScore;
                    if (similarityScore > 0)
                        foreach (var rating in user.Ratings.Where(x => selectedUser.Ratings.All(y => y.MovieId != x.MovieId)))
                        {
                            weightedScores.Add(new RatingWeightedScore
                            {
                                MovieId = rating.MovieId,
                                WeightedScore = similarityScore * rating.Score
                            });
                        }
                }
            }
            return weightedScores;
        }

        public async Task<List<MovieRecommendation>> FindBestMovies(List<Similarity> similarites, List<RatingWeightedScore> weightedScores)
        {
            var movies = await _unitOfWork.Movies.GetAllMovies();

            var recommendationList = new List<MovieRecommendation>();
            foreach (var movie in movies)
            {
                var allUsersThatRatedMovie = movie.Ratings.Where(x => x.MovieId == movie.MovieId).Select(y => y.User).ToList();
                var sumOfSimilarities = 0.0;

                //Sum all similaritiescores on users that have voted on that movie
                foreach (var user in allUsersThatRatedMovie)
                    sumOfSimilarities += similarites.Where(y => y.Id == user.UserId).Sum(x => x.SimilarityScore);

                //Sum all weighted scores for that movie
                var sumOfMovieScores = weightedScores.Where(x => x.MovieId == movie.MovieId).Sum(y => y.WeightedScore);
                recommendationList.Add(new MovieRecommendation
                {
                    MovieId = movie.MovieId,
                    MovieTitle = movie.MovieTitle,
                    MovieYear = movie.MovieYear,
                    AverageWeightedRating = sumOfMovieScores / sumOfSimilarities
                });
            }
            return recommendationList.OrderByDescending(x => x.AverageWeightedRating).ToList();
        }
    }
}