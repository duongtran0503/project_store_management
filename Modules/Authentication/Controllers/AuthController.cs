using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Authentication.DTOs.Requests;
using StoreManagement.API.Modules.Authentication.DTOs.Responses;
using StoreManagement.API.Modules.Authentication.Services;

namespace StoreManagement.API.Modules.Authentication.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController: ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
             _userService = userService;
         }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse.ErrorInput(ModelState));
            }
            var response = await _userService.RegisterUser(request);
           return Ok(ApiResponse<AuthenticationResponse>.Ok(response));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse.ErrorInput(ModelState));
            }
            var response = await _userService.Login(request);
            return Ok(ApiResponse<AuthenticationResponse>.Ok(response));
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
           var response = await _userService.GetProfile();
            return Ok(ApiResponse<UserResponse>.Ok(response));
        }

        
      
         
    }
}
