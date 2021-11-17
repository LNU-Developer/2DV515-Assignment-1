using Backend.Models.Database;
using Backend.Models.Repositories;
using System;

namespace Backend.Models.Services
{
    public class EuclideanDistanceService : UserBasedCollaborativeFilteringService
    {
        public EuclideanDistanceService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override double CalculateDistance(User A, User B)
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
    }
}