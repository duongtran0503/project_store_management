using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Inventories.Dtos.Requests;
using StoreManagement.API.Modules.Inventories.Dtos.Response;
using StoreManagement.API.Modules.Inventories.Services;

namespace StoreManagement.API.Modules.Inventories.Controllers
{
    [ApiController]
    [Route("/api/inventories")]
    public class InventoryController:ControllerBase
    {
        private readonly InventoryService _inventoryService;
        public InventoryController(InventoryService inventoryService) { 
        _inventoryService = inventoryService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ImportGoods([FromBody] CreateInventoryReceiptRequest request)
        {
            if(!ModelState.IsValid)  return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res =await _inventoryService.CreateInventoryReceipt(request);
            return Ok(ApiResponse<InventoryReceiptResponse>.Ok(res));
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateImportGoods([FromBody] UpdateInventoryReceiptRequest request
            , [FromRoute] string id
            )
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));
            var res = await _inventoryService.UpdateInventoryReceipt(id,request);
            return Ok(ApiResponse<InventoryReceiptResponse>.Ok(res));
        }
        [HttpGet("receipts/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(
           [FromRoute] string id
           )
        {
          
            var res = await _inventoryService.GetInventoryReceiptById(id);
            return Ok(ApiResponse<InventoryReceiptResponse>.Ok(res));
        }

        [HttpGet("receipts/history")]
        [Authorize]
        public async Task<IActionResult> GetPageReceiptDetail([FromQuery] PaginationRequest request)
        {
            var res =await _inventoryService.GetPageReceiptHistory(request);
            return Ok(ApiResponse<PaginationResponse<InventoryReceiptItemResponse>>.Ok(res));
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetPageInventory([FromQuery] PaginationRequest request)
        {
            var res = await _inventoryService.GetPageInventory(request);
            return Ok(ApiResponse<PaginationResponse<InventoryResponse>>.Ok(res));
        }

        [HttpGet("receipts")]
        [Authorize]
        public async Task<IActionResult> GetPageInventoryReceipt([FromQuery] PaginationRequest request)
        {
            var res = await _inventoryService.GetPageInventoryReceipt(request);
            return Ok(ApiResponse<PaginationResponse<InventoryReceiptResponse>>.Ok(res));
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateGRNStatus([FromBody] UpdateGRNStatusRequest request,
            [FromRoute] string id)
        {
          var res =    await _inventoryService.UpdateGRNStatusReceipt(id, request);
             return Ok(ApiResponse<UpdateGRNStatusResponse>.Ok(res));
        }

    }
}
