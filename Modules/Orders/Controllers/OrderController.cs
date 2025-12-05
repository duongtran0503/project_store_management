using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Orders.Dtos.Requests;
using StoreManagement.API.Modules.Orders.Dtos.Responses;
using StoreManagement.API.Modules.Orders.Services;

namespace StoreManagement.API.Modules.Orders.Controllers
{
    [ApiController]
    [Route("/api/orders")]
    public class OrderController:ControllerBase
    {
        private readonly InvoiceService _invoiceService;
        public OrderController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost("online")]
        public async Task<IActionResult> CreateInvoiceOnline([FromBody] CreateInvoiceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _invoiceService.CreateInvoiceOnline(request);
            return Ok(ApiResponse<InvoiceDetailResponse>.Ok(res));
        }

        [HttpPost("pos")]
        [Authorize]
        public async Task<IActionResult> CreateInvoicePos([FromBody] CreateInvoiceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _invoiceService.CreateInvoicePOS(request);
            return Ok(ApiResponse<InvoiceDetailResponse>.Ok(res));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetpageInvoices([FromQuery] PaginationRequest request)
        {
           var res =await _invoiceService.GetPageInvoices(request);
            return Ok(ApiResponse<PaginationResponse<InvoiceResponse>>.Ok(res));
        }
    }
}
