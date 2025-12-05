using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Modules.Products.Services;

namespace StoreManagement.API.Modules.Products.Controllers
{
    [ApiController]
    [Route("/api/categories")]

    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService) {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse.ErrorInput(ModelState));
            }
            var response = await _categoryService.CreateCategory(request);
            return Ok(ApiResponse<CategoryResponse>.Ok(response));

        }

        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] PaginationRequest request)
        {
            var res = await _categoryService.GetCategories(request);
            return Ok(ApiResponse<PaginationResponse<CategoryResponse>>.Ok(res));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] string id)
        {
            var res = await _categoryService.GetCategoryById(id);
            return Ok(ApiResponse<CategoryResponse>.Ok(res));

        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory([FromRoute] string id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok(ApiResponse.Ok());
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory([FromRoute] string id, [FromBody] UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ApiResponse.ErrorInput(ModelState));

            var res = await _categoryService.UpdateCategory(request, id);
            return Ok(ApiResponse<CategoryResponse>.Ok(res));
        }

        [HttpPatch("restore/{id}")]
        [Authorize]
        public async Task<IActionResult> RestoreCategory([FromRoute] string id)
        {
           var res =   await _categoryService.RestoreCategory(id);
            return Ok(ApiResponse<CategoryResponse>.Ok(res));
        }

        [HttpGet("search")]
        public async Task<IActionResult> FilterAuthor([FromQuery] FilterCategoryRequest request)
        {
            var res = await _categoryService.FilterPublisher(request);
            return Ok(ApiResponse<PaginationResponse<CategoryResponse>>.Ok(res));
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetListSuggestion([FromQuery] FilterCategoryRequest request)
        {
            var res = await _categoryService.GetListSuggestions(request);
            return Ok(ApiResponse<List<SuggestionsResponse>>.Ok(res));
        }
    }
}
