namespace Backend.Models
{
    public class Recommendation
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public int MovieYear { get; set; }
        public double AverageWeightedRating { get; set; }
    }
}