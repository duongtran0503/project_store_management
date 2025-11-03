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
        private readonly AuthService _authService;

        public AuthController(AuthService userService)
        {
             _authService = userService;
         }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse.ErrorInput(ModelState));
            }
            var response = await _authService.RegisterUser(request);
           return Ok(ApiResponse<AuthenticationResponse>.Ok(response));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse.ErrorInput(ModelState));
            }
            var response = await _authService.Login(request);
            return Ok(ApiResponse<AuthenticationResponse>.Ok(response));
        }

       

        
      
         
    }
}
