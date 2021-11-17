using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Backend.Models.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly EuclideanDistanceService _euclideanDistance;
        private readonly PearsonCorrelationService _pearsonsCorrelation;
        private readonly ItemBasedCollaborativeFilteringService _itemBasedCollaborativeFilteringService;
        public RecommendationController(EuclideanDistanceService euclideanDistance, PearsonCorrelationService pearsonsCorrelation, ItemBasedCollaborativeFilteringService itemBasedCollaborativeFilteringService)
        {
            _euclideanDistance = euclideanDistance;
            _pearsonsCorrelation = pearsonsCorrelation;
            _itemBasedCollaborativeFilteringService = itemBasedCollaborativeFilteringService;
        }

        [HttpGet("ubcf/ed/")]
        public async Task<IActionResult> GetMovieRecommendationsByEuclidianDistance(int userId, int k = 3)
        {
            return Ok(await _euclideanDistance.FindKMovieRecommendation(userId, k));
        }

        [HttpGet("ubcf/pc/")]
        public async Task<IActionResult> GetMovieRecommendationsByPearsonCorrelation(int userId, int k = 3)
        {
            return Ok(await _pearsonsCorrelation.FindKMovieRecommendation(userId, k));
        }

        [HttpGet("ibcf/ed/")]
        public async Task<IActionResult> GetMovieRecommendationsByItemBasedCollaborativeFilteringService(int userId, int k = 3)
        {
            return Ok(await _itemBasedCollaborativeFilteringService.FindKMovieRecommendation(userId, k));
        }
    }
}
