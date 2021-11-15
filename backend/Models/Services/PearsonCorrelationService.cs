using Backend.Models.Database;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Backend.Models.Recommendation;

namespace Backend.Models.Services
{
    public class PearsonCorrelationService : RecommendationSystemService
    {
        public PearsonCorrelationService(Context context) : base(context)
        {
        }

        private double CalculatePearsonCorrelation(User A, User B)
        {
            double sum1 = 0, sum2 = 0, sum1sq = 0, sum2sq = 0, psum = 0;
            int n = 0;
            // Iterate over all rating combinations
            foreach (var rA in A.Ratings)
            {
                foreach (var rB in B.Ratings)
                {
                    if (rA.MovieId == rB.MovieId)
                    {
                        sum1 += rA.Score;
                        sum2 += rB.Score;
                        sum1sq += rA.Score * rA.Score;
                        sum2sq += rB.Score * rB.Score;
                        psum += rA.Score * rB.Score;
                        n++;
                    }
                }
            }
            if (n == 0) return 0;
            double num = psum - ((sum1 * sum2) / n);
            double den = Math.Sqrt((sum1sq - Math.Pow(sum1, 2.0) / n) * (sum2sq - Math.Pow(sum2, 2.0) / n));
            return num / den;
        }


        public override async Task<List<Similarity>> CalculateSimilarityScores(User selectedUser)
        {
            var users = await GetAllUsersDataExceptSelected(selectedUser);

            var similarityList = new List<Similarity>();
            foreach (var user in users)
            {
                double pearsonCorrelation = CalculatePearsonCorrelation(selectedUser, user);

                if (pearsonCorrelation < 0) continue; //Not interested dissimilar scores, i.e only show scores with similarites
                similarityList.Add(new Similarity
                {
                    UserId = user.UserId,
                    SimilarityScore = pearsonCorrelation
                });
            }
            return similarityList;
        }
    }
}