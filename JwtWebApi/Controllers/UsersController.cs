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

                        if (username == null)
                        {
                                return BadRequest("No valid username");
                        }

                        UserProfileDto? user = _userRespository.GetUserProfile(username!);

                        if (user == null)
                        {
                                return NotFound("No UserProfile with given username");
                        }

                        return Ok(user);
                }

                [HttpPut("updateProfile")]
                public ActionResult UpdateUserProfile([FromBody] EditUserDto editUser)
                {
                        string? username = HttpContext.User.Identity!.Name;

                        if (username == null)
                        {
                                return BadRequest("No valid username");
                        }

                        EditUserDto? user = _userRespository.UpdateUserProfile(username, editUser);

                        if (user == null)
                        {
                                return NotFound("No UserProfile with given username");
                        }

                        return Ok(user);
                }
        }
}