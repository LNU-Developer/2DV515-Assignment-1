namespace Backend.Models.Recommendation
{
    public interface IMovieRecommendation
    {
        int MovieId { get; set; }
        string MovieTitle { get; set; }
        int MovieYear { get; set; }
        double AverageWeightedRating { get; set; }
    }
}