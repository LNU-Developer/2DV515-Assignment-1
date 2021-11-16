namespace Backend.Models.Recommendation
{
    public class MovieSimilarity
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public int MovieYear { get; set; }
        public double SimilarityScore { get; set; }
    }
}