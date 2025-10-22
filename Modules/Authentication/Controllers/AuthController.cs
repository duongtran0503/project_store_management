using Microsoft.AspNetCore.Mvc;

namespace StoreManagement.API.Modules.Authentication.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController: ControllerBase
    {
        [HttpGet]
      async   public Task<IActionResult> hello() {
            return  Ok("hello");
        }
    }
}
