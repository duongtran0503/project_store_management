using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Users.Dtos.Requests;
using StoreManagement.API.Modules.Users.Dtos.Response;
using StoreManagement.API.Modules.Users.Services;

namespace StoreManagement.API.Modules.Users.Controllers
{
    [ApiController]
    [Route("/api/customers")]
    public class CustomerController:ControllerBase
    {
        private readonly CustomerService _customerService;
        public CustomerController(CustomerService customerService) { 
         _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListCustomer([FromQuery] Common.Responses.PaginationRequest request)
        {
            var res = await _customerService.GetPageCustomer(request);
            return Ok(ApiResponse<Common.Responses.PaginationResponse<CustomerResponse>>.Ok(res));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _customerService.CreateCustomer(request);
            return Ok(ApiResponse<CustomerResponse>.Ok(res));
        }



    }
}
