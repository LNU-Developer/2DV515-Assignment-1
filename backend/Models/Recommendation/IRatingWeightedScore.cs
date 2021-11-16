namespace Backend.Models.Recommendation
{
    public interface IRatingWeightedScore
    {
        int MovieId { get; set; }
        double WeightedScore { get; set; }
    }
}