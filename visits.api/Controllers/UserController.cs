using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using visits.api.DTOs;
using visits.api.Interfaces;
using visits.api.Utils;

namespace visits.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("GetProfile")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<UserProfile>), 200)]
        public async Task<IActionResult> GetProfile()
        {
            if(!ModelState.IsValid)
                return BadRequest();

            return Ok(await userService.GetMyUserProfileAsync());
        }

        [HttpGet("GetUserById")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<UserProfile>), 200)]
        public async Task<IActionResult> GetUserById(Guid guid)
        {
            return Ok(await userService.GetUserById(guid));
        }
        
        [HttpPost("SaveUserProfile")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseHandler), 200)]
        public async Task<IActionResult> SaveUserProfile([FromBody] UserProfile guid)
        {
            return Ok(await userService.SaveUserProfileAsync(guid));
        }
    }
}
