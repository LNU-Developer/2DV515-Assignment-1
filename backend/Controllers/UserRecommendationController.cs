using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Backend.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRecommendationController : ControllerBase
    {
        private readonly Context _context;
        public UserRecommendationController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _context.Users.ToListAsync());
        }
    }
}
