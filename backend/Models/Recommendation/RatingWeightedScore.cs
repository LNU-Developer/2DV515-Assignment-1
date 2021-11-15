namespace Backend.Models.Recommendation
{
    public class RatingWeightedScore
    {
        public int MovieId { get; set; }
        public double WeightedScore { get; set; }
    }
}