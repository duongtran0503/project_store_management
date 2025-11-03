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

    public class CategoryController:ControllerBase
    {
        private readonly CategoryService _categoryService;
        public CategoryController(CategoryService categoryService) {
            _categoryService = categoryService; 
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ApiResponse.ErrorInput(ModelState));
            }
            var response  = await _categoryService.CreateCategory(request);
            return Ok(ApiResponse<CategoryResponse>.Ok(response));

        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var res = await _categoryService.GetCategories();
            return Ok(ApiResponse<List<CategoryResponse>>.Ok(res));
        }
    }
}
