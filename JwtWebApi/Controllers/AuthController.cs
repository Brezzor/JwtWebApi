using JwtWebApi.Helpers;
using JwtWebApi.Models;
using JwtWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Controllers
{
        [AllowAnonymous]
        [EnableCors]
        [Route("api/[controller]")]
        [ApiController]
        public class AuthenticationController : ControllerBase
        {
                private readonly Authenticator _authenticator;
                private readonly UserRespository _userRespository;

                public AuthenticationController(Authenticator authenticator, UserRespository userRespository)
                {
                        _authenticator = authenticator;
                        _userRespository = userRespository;
                }

                [HttpPost("register")]
                public ActionResult Register([FromBody] UserDto request)
                {
                        if (request == null)
                        {
                                return BadRequest("Invalid user request");
                        }

                        _authenticator.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                        bool newUser = _userRespository.CreateUser(request.Username, passwordHash, passwordSalt);

                        if (newUser)
                        {
                                return BadRequest("New user not created. Username already exists");
                        }

                        return Ok("New user registered");
                }

                [HttpPost("login")]
                public ActionResult Login([FromBody] UserDto request)
                {
                        User? user = _userRespository.AuthLogin(request);

                        if (user == null)
                        {
                                return BadRequest($"Username dosn't exist");
                        }

                        if (!_authenticator.VerifyPasswordHash(request.Password, user.PasswordHash!, user.PasswordSalt!))
                        {
                                return BadRequest("Wrong password");
                        }

                        var token = _authenticator.CreateToken(user);
                        return Ok(token);
                }
        }
}
