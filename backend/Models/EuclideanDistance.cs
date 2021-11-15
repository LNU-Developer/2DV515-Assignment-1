using Backend.Models.Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Backend.Models.Services
{
    public class EuclideanDistance : RecommendationSystemService
    {
        public EuclideanDistance(Context context) : base(context)
        {
        }

        private double CalculateEuclideanDistance(User A, User B)
        {
            //distance
            double d = 0;
            int n = 0;
            // Go through the whole collection checking against the users own ratings.
            foreach (var rA in A.Ratings)
            {
                foreach (var rB in B.Ratings)
                {
                    if (rA.MovieId == rB.MovieId)
                    {
                        d += Math.Pow(rA.Score - rB.Score, 2.0);
                        n++;
                    }
                }
            }
            if (n == 0) return 0; //No movies found
            return 1 / (1 + d); //The higher value the more dissimmilar the scores are, can either invert or take this into account in the sorting model. I have choosen to invert. Making higher scores better.
        }

        public async Task<List<Recommendation>> FindKMovieRecommendation(int selectedUserId, int k = 3)
        {
            var selectedUser = await GetUserById(selectedUserId);
            var similarites = await CalculateSimilarityScores(selectedUser);
            var weightedScores = await CalculateRatingWeightedScores(selectedUser, similarites);
            var bestMovies = await FindBestMovies(similarites, weightedScores);
            return bestMovies.Where(x => x.AverageWeightedRating > 0).Take(k).ToList();
        }

        private async Task<List<Similarity>> CalculateSimilarityScores(User selectedUser)
        {
            var users = await GetAllUsersDataExceptSelected(selectedUser);

            var similarityList = new List<Similarity>();
            foreach (var user in users)
            {
                double euclideanDistance = CalculateEuclideanDistance(selectedUser, user);
                if (euclideanDistance < 0) continue; //Not interested dissimilar scores, i.e only show scores with similarites
                similarityList.Add(new Similarity
                {
                    UserId = user.UserId,
                    SimilarityScore = euclideanDistance
                });
            }
            return similarityList;
        }
    }
}