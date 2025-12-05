using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] PaginationRequest request)
        {
            var result = await _productService.GetBooksAsync(request);

           
            return Ok(ApiResponse<PaginationResponse<BookResponse>>.Ok(result));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook(
            [FromRoute] string id,
            [FromBody] UpdateBookRequest request
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse.ErrorInput(ModelState));
            }

            var res = await _productService.UpdateBook(request, id);
            return Ok(ApiResponse<BookResponse>.Ok(res));

        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
          var res =    await _productService.DeleteProduct(id);
            return Ok(ApiResponse<DeletedResponse>.Ok(res,message:"Xóa thành công"));
        }
        [HttpPatch("restore/{id}")]
        [Authorize]
        public async Task<IActionResult> RestoreProduct([FromRoute] string id)
        {
           var res= await _productService.RestoreProduct(id);
            return Ok(ApiResponse<BookResponse>.Ok(res,message:"Khôi phục thành công"));

        }

        [HttpGet("search")]
        public async Task<IActionResult> FilterProducts([FromQuery] FilterProductRequest request)
        {
            var result = await _productService.FilterProducts(request);


            return Ok(ApiResponse<PaginationResponse<BookResponse>>.Ok(result));
        }

        [HttpGet("{id}")]
       
        public async Task<IActionResult> GetBookById([FromRoute] string id)
        {
            var res = await _productService.GetBookById(id);
            return Ok(ApiResponse<BookResponse>.Ok(res));
        }

        [HttpGet("deleted")]
        [Authorize]
        public async Task<IActionResult> GetBookDeleted([FromQuery] PaginationRequest request)
        {
            var result = await _productService.GetBookDeleted(request);


            return Ok(ApiResponse<PaginationResponse<BookResponse>>.Ok(result));
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestion([FromQuery] FilterProductRequest request)
        {
            var res =  await _productService.GetSuggestions(request);
            return Ok(ApiResponse<List<SuggestionsResponse>>.Ok(res));

        }

    }

   
}
