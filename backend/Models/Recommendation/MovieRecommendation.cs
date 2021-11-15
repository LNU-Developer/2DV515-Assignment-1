namespace Backend.Models.Recommendation
{
    public class MovieRecommendation
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public int MovieYear { get; set; }
        public double AverageWeightedRating { get; set; }
    }
}