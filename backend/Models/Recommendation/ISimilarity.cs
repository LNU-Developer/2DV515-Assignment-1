namespace Backend.Models.Recommendation
{
    public interface ISimilarity
    {
        int Id { get; set; }
        string Name { get; set; }
        double SimilarityScore { get; set; }
    }
}