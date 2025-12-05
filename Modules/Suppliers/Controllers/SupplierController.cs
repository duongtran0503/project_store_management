using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Suppliers.Dtos.Requests;
using StoreManagement.API.Modules.Suppliers.Dtos.Responses;
using StoreManagement.API.Modules.Suppliers.Services;

namespace StoreManagement.API.Modules.Suppliers.Controllers
{
    [ApiController]
    [Route("/api/suppliers")]
    public class SupplierController:ControllerBase
    {
        private readonly SupplierService _supplierService;
        public SupplierController(SupplierService supplierService) { 
         _supplierService = supplierService;
        }

        [HttpGet]
        [Authorize]
         public async Task<IActionResult> GetSuppliers([FromQuery] PaginationRequest request)
        {
            var res = await _supplierService.GetSuppliers(request);
            return Ok(ApiResponse<PaginationResponse<SupplierResponse>>.Ok(res));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierRequest request)
        {
            if(!ModelState.IsValid)  return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res =await _supplierService.CreateSupplier(request);
            return Ok(ApiResponse<SupplierResponse>.Ok(res));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateSupplier([FromRoute] string id, [FromBody] UpdateSupplierRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _supplierService.UpdateSupplier(request, id);
            return Ok(ApiResponse<SupplierResponse>.Ok(res));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetSupplierById([FromRoute] string id)
        {
           
            var res = await _supplierService.GetSupplierById(id);
            return Ok(ApiResponse<SupplierResponse>.Ok(res));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchSupplier([FromQuery] SearchSupplierRequest request)
        {
            var res = await _supplierService.SearchSupplier(request);
            return Ok(ApiResponse<PaginationResponse<SupplierResponse>>.Ok(res));
        }

    }
}
