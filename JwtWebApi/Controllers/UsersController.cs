using JwtWebApi.Models;
using JwtWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Controllers
{
        [EnableCors]
        [Authorize]
        [Route("api/[controller]")]
        [ApiController]
        public class UsersController : ControllerBase
        {
                private readonly UserRespository _userRespository;

                public UsersController(UserRespository userRespository)
                {
                        _userRespository = userRespository;
                }

                [HttpGet("profile")]
                public ActionResult GetUserProfile()
                {
                        string? username = HttpContext.User.Identity!.Name;

                        UserProfile? user = _userRespository.GetUserProfile(username!);

                        if (user == null)
                        {
                                return BadRequest("No UserProfile");
                        }

                        return Ok(user);
                }

                [HttpPut("updateProfile")]
                public ActionResult UpdateUserProfile([FromBody] UserProfile userProfile)
                {
                        UserProfile? user = _userRespository.UpdateUserProfile(userProfile);

                        if (user == null)
                        {
                                return BadRequest();
                        }

                        return Ok(user);
                }
        }
}