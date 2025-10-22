using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Authentication.Services;

namespace StoreManagement.API.Modules.Authentication.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController: ControllerBase
    {
        private readonly TestService _testService;

        public AuthController(TestService testService)
        {
             this._testService = testService;
         }
        [HttpGet]
      async   public Task<IActionResult> hello() {
            _testService.Hello();
            return Ok(ApiResponse<string>.Ok("hellow"));
        }
    }
}
