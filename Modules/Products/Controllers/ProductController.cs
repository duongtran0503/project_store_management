using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Modules.Products.Services;

namespace StoreManagement.API.Modules.Products.Controllers
 {
    [ApiController]
    [Route("/api/products")]
    public class ProductController:ControllerBase
    {
        private readonly ProductService _productService;
        public ProductController(ProductService productService) { 
         _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var res =await _productService.GetBooks();
            return Ok(ApiResponse<List<BookResponse>>.Ok(res));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] CreateBookRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ApiResponse.ErrorInput(ModelState));
            }
            var res = await _productService.CreateProduct(request);
            return Ok(ApiResponse<BookResponse>.Ok(res));
        }
       
    }
}
