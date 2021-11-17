using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Backend.Models.Repositories;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _unitOfWork.Users.GetAllAsync());
        }
    }
}
