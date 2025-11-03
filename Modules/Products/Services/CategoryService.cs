using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Modules.Products.ErrorCode;
using StoreManagement.API.Modules.Products.Repository;

namespace StoreManagement.API.Modules.Products.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;
        public CategoryService(CategoryRepository categoryRepository) { 
          _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> CreateCategory(CreateCategoryRequest request)
        {
            var check = await _categoryRepository.CheckCategoryByCategoryCodeAsync(request.CategoryCode);
            if(check)
            {
                throw new AppException(CategoryErrorCode.CategoryExisted);
            }
            var category = await _categoryRepository.CreateCategoryAsync(new Common.Entities.Category
            {
                CategoryCode = request.CategoryCode,
                CategoryName = request.CategoryName,
            });
            return new CategoryResponse { CategoryName =category.CategoryName,Id = category.Id,CreatedDate =category.CreatedAt,UpdateAt = category.UpdatedAt,CategoryCode =category.CategoryCode};
        }

        public async Task<List<CategoryResponse>> GetCategories()
        {
            var categories = await _categoryRepository.GetCategories();

          
            List<CategoryResponse> result = categories
                .Select(category => new CategoryResponse
                {
                    CategoryName = category.CategoryName,
                    Id = category.Id,
                    CreatedDate = category.CreatedAt,
                    UpdateAt = category.UpdatedAt,
                    CategoryCode = category.CategoryCode
                })
                .ToList(); 

            return result;
        }
    }  
}
