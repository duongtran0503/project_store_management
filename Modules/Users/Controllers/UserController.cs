using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Users.Dtos.Response;
using StoreManagement.API.Modules.Users.Services;

namespace StoreManagement.API.Modules.Users.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController:ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        { 
            var response =  await _userService.GetProfile();
            return Ok(ApiResponse<UserResponse>.Ok(response));
        }

    
    }

}
