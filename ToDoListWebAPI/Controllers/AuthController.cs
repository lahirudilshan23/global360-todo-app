using Microsoft.AspNetCore.Mvc;
using ToDoListWebAPI.Models;
using ToDoListWebAPI.Services;

namespace ToDoListWebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public IActionResult Login(In_LoginRequest request)
        {
            try
            {
                return Ok(_authService.Login(
                    request.Username,
                    request.Password));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("refresh")]
        public IActionResult Refresh(In_RefreshTokenRequest request)
        {
            try
            {
                return Ok(_authService.Refresh(request.Username,request.RefreshToken));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
