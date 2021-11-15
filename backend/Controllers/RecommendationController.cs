using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Backend.Models.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly EuclideanDistance _euclideanDistance;
        public RecommendationController(EuclideanDistance euclideanDistance)
        {
            _euclideanDistance = euclideanDistance;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieRecommendations(int userId, int k)
        {
            return Ok(await _euclideanDistance.FindKMovieRecommendation(userId, k));
        }
    }
}
