namespace Backend.Models.Recommendation
{
    public interface ISimilarity
    {
        int Id { get; set; }
        double SimilarityScore { get; set; }
    }
}