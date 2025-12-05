using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Promotions.Dtos.Requests;
using StoreManagement.API.Modules.Promotions.Dtos.Responses;
using StoreManagement.API.Modules.Promotions.Services;

namespace StoreManagement.API.Modules.Promotions.Controllers
{
    [ApiController]
    [Route("/api/vouchers")]
    public class VoucherController :ControllerBase
    {
        private readonly VoucherService _voucherService;
        public VoucherController(VoucherService voucherService) { 
         _voucherService = voucherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPageVouchers([FromQuery] PaginationRequest request)
        {
            var res = await _voucherService.GetPageVouchers(request);
            return Ok(ApiResponse<PaginationResponse<VoucherResponse>>.Ok(res));    
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateVoucher([FromBody] CreateVoucherRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _voucherService.CreateVoucher(request);
            return Ok(ApiResponse<VoucherResponse>.Ok(res));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateVoucher([FromBody] UpdateVoucherRequest request,
            [FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _voucherService.UpdateVourcher(request,id);
            return Ok(ApiResponse<VoucherResponse>.Ok(res));
        }

        [HttpGet("search")]
        public async Task<IActionResult> FilterVoucher([FromQuery] FilterVoucherRequest request)
        {
            var res = await _voucherService.FilterVoucher(request);
            return Ok(ApiResponse<PaginationResponse<VoucherResponse>>.Ok(res));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVoucher([FromRoute] string id)
        {
            var res = await _voucherService.DeleteVoucher(id);
            return Ok(ApiResponse<DeletedResponse>.Ok(res));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> RestoreVoucher([FromRoute] string id)
        {
            var res = await _voucherService.RestoreVoucher(id);
            return Ok(ApiResponse<VoucherResponse>.Ok(res));
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] FilterVoucherRequest request)
        {
            var res = await _voucherService.GetSuggestons(request);
            return Ok(ApiResponse<List<SuggestionResponse>>.Ok(res));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetVoucherById([FromRoute] string id)
        {
            var res = await _voucherService.GetVoucherDetail(id);
            return Ok(ApiResponse<DetailVoucherResponse>.Ok(res));
        }
    }
}
