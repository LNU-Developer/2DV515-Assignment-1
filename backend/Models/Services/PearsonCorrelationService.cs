using Backend.Models.Database;
using Backend.Models.Repositories;
using System;

namespace Backend.Models.Services
{
    public class PearsonCorrelationService : UserBasedCollaborativeFilteringService
    {
        public PearsonCorrelationService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override double CalculateDistance(User A, User B)
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
    }
}