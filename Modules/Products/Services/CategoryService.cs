using StoreManagement.API.Common.Entities;
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

        public async Task<CategoryResponse> GetCategoryById(string id)
        {

            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null) throw new AppException(CategoryErrorCode.CategoryNotExisted);
            return ToCategoryResponse(category);
        }

        public async Task DeleteCategory(string id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null) throw new AppException(CategoryErrorCode.CategoryNotExisted);
            var check = await _categoryRepository.CheckCategoryHaveProduct(id);
            if (check) throw new AppException(CategoryErrorCode.CategoryCantNotDelete);
            await _categoryRepository.DeleteCategory(category);
        }

        public async Task<CategoryResponse> UpdateCategory(UpdateCategoryRequest request,string id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null) throw new AppException(CategoryErrorCode.CategoryNotExisted);

            if(category.CategoryCode!=request.CategoryCode )
            {
                var check = await _categoryRepository.CheckCategoryByCategoryCodeAsync(request.CategoryCode);
                if (check) throw new AppException(CategoryErrorCode.CategoryCodeExisted);
            }
            category.CategoryName = request.CategoryName;
            category.CategoryCode = request.CategoryCode;
            var update = await _categoryRepository.UpdateCategory(category);
            return ToCategoryResponse(category);
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

        private CategoryResponse ToCategoryResponse(Category category)
        {
            return new CategoryResponse
            {
                CategoryCode = category.CategoryCode,
                CategoryName = category.CategoryName
            ,
                CreatedDate = category.CreatedAt,
                UpdateAt = category.UpdatedAt,
                Id = category.Id
            };
        }
    }  
}
