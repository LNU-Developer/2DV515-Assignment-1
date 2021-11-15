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
        public RecommendationController(EuclideanDistanceService euclideanDistance, PearsonCorrelationService pearsonsCorrelation)
        {
            _euclideanDistance = euclideanDistance;
            _pearsonsCorrelation = pearsonsCorrelation;
        }

        [HttpGet("euclidiandistance/")]
        public async Task<IActionResult> GetMovieRecommendationsByEuclidianDistance(int userId, int k = 3)
        {
            return Ok(await _euclideanDistance.FindKMovieRecommendation(userId, k));
        }

        [HttpGet("pearsoncorrelation/")]
        public async Task<IActionResult> GetMovieRecommendationsByPearsonCorrelation(int userId, int k = 3)
        {
            return Ok(await _pearsonsCorrelation.FindKMovieRecommendation(userId, k));
        }
    }
}
