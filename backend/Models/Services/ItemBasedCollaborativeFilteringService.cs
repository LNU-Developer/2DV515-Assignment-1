using Backend.Models.Database;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Models.Recommendation;
using Backend.Models.Repositories;
using System;


namespace Backend.Models.Services
{
    public class ItemBasedCollaborativeFilteringService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemBasedCollaborativeFilteringService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public double CalculateDistance(Movie A, Movie B)
        {
            //distance
            double d = 0;
            int n = 0;
            // Go through the whole collection checking against the users own ratings.
            foreach (var rA in A.Ratings)
            {
                foreach (var rB in B.Ratings)
                {
                    if (rA.UserId == rB.UserId)
                    {
                        d += Math.Pow(rA.Score - rB.Score, 2.0);
                        n++;
                    }
                }
            }
            if (n == 0) return 0; //No movies found
            return 1 / (1 + d); //The higher value the more dissimmilar the scores are, can either invert or take this into account in the sorting model. I have choosen to invert. Making higher scores better.
        }

        public async Task<List<Similarity>> CalculateSimilarityScores(Movie selectedMovie)
        {
            var movies = await _unitOfWork.Movies.GetAllMoviesDataExceptSelected(selectedMovie);
            var similarityList = new List<Similarity>();
            foreach (var movie in movies)
            {
                double distance = CalculateDistance(selectedMovie, movie);
                if (distance < 0) continue; //Not interested dissimilar scores, i.e only show scores with similarites
                similarityList.Add(new Similarity
                {
                    Id = movie.MovieId,
                    SimilarityScore = distance
                });
            }
            return similarityList;
        }

        public async Task<List<MovieRecommendation>> FindKMovieRecommendation(int userId, int k = 3)
        {
            var seenMovies = await _unitOfWork.Movies.GetAllSeenMoviesByUserId(userId);
            var similarityDictionary = new Dictionary<Movie, List<Similarity>>();

            foreach (var seenMovie in seenMovies)
                similarityDictionary.Add(seenMovie, await CalculateSimilarityScores(seenMovie));

            foreach (var pair in similarityDictionary)
            {
                var movieId = pair.Key.MovieId;
                foreach (var item in similarityDictionary)
                {
                    item.Value.RemoveAll(x => x.Id == movieId);
                }
            }

            var weightedScores = await CalculateRatingWeightedScores(userId, similarityDictionary);
            var bestMovies = await FindBestMovies(similarityDictionary, weightedScores);
            return bestMovies.Where(x => x.AverageWeightedRating > 0).Take(k).ToList();
        }

        public async Task<List<RatingWeightedScore>> CalculateRatingWeightedScores(int userId, Dictionary<Movie, List<Similarity>> similarityDictionary)
        {
            var weightedScores = new List<RatingWeightedScore>();

            foreach (var pair in similarityDictionary)
            {
                var movieId = pair.Key.MovieId;
                var rating = await _unitOfWork.Ratings.GetUserRatingByMovieId(movieId, userId);
                pair.Value.ForEach(movie =>
                {
                    if (rating is not null)
                        weightedScores.Add(new RatingWeightedScore
                        {
                            MovieId = movie.Id,
                            WeightedScore = rating.Score * movie.SimilarityScore
                        });
                });
            }
            return weightedScores;
        }

        public async Task<List<MovieRecommendation>> FindBestMovies(Dictionary<Movie, List<Similarity>> movieDicionary, List<RatingWeightedScore> weightedScores)
        {
            var recommendationList = new List<MovieRecommendation>();
            var similarityList = movieDicionary.Values    // Getting only List<int>
                      .SelectMany(x => x)       // Flatten
                      .ToList();
            foreach (var movie in weightedScores)
            {
                var sumOfMovieScores = weightedScores.Where(x => x.MovieId == movie.MovieId).Sum(y => y.WeightedScore);
                var sumOfSimilarities = similarityList.Where(y => y.Id == movie.MovieId).Sum(x => x.SimilarityScore);
                var movieInfo = await _unitOfWork.Movies.GetMovieById(movie.MovieId);
                if (!recommendationList.Any(x => x.MovieId == movie.MovieId))
                    recommendationList.Add(new MovieRecommendation
                    {
                        MovieId = movie.MovieId,
                        MovieTitle = movieInfo.MovieTitle,
                        MovieYear = movieInfo.MovieYear,
                        AverageWeightedRating = sumOfMovieScores / sumOfSimilarities
                    });
            }
            return recommendationList.OrderByDescending(x => x.AverageWeightedRating).ToList();
        }
    }
}