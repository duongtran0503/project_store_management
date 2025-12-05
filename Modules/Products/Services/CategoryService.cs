using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Products.Constants;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Modules.Products.ErrorCode;
using StoreManagement.API.Modules.Products.Repository;
using TimeZoneConverter;

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
                Status = CategoryStatusConstants.DEFAULT,
            });
            return ToCategoryResponse(category);
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
            category.IsDeleted = true;
            await _categoryRepository.UpdateCategory(category);
        }

        public async Task<CategoryResponse> RestoreCategory(string id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null) throw new AppException(CategoryErrorCode.CategoryNotExisted);
            category.IsDeleted = false;
            var update =  await _categoryRepository.UpdateCategory(category);
            return ToCategoryResponse(update);

        }

        public async Task<PaginationResponse<CategoryResponse>> FilterPublisher(FilterCategoryRequest request)
        {
            var categoryEntities = await _categoryRepository.FilterCategoryAsync(request);
            var categories = categoryEntities.Select(au => ToCategoryResponse(au)).ToList();
            return new PaginationResponse<CategoryResponse>(categories, categories.Count, request.PageNumber, request.PageSize);
        }

        public async Task<List<SuggestionsResponse>> GetListSuggestions(FilterCategoryRequest request)
        {
            return await _categoryRepository.GetSuggestionsAsync(request);
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
            category.Status = request.Status;
            
            var update = await _categoryRepository.UpdateCategory(category);
            return ToCategoryResponse(category);
        }
        public async Task<PaginationResponse<CategoryResponse>> GetCategories(PaginationRequest request)
        {
            var categoryEntities = await _categoryRepository.GetPageCategoriesWithDetailAsync(request.PageNumber,request.PageSize);

            var categories = categoryEntities.Select( ca =>
            {
                int total =ca.TotalBooks;
                ca.Category.TotalBooks = total;
                return ca.Category;

            }).ToList();
          
            return new PaginationResponse<CategoryResponse>
                (categories, categories.ToList().Count, request.PageNumber, request.PageSize);
          
        }

        private CategoryResponse ToCategoryResponse(Category category,int totalBook=0)
        {
            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(category.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(category.UpdatedAt, vietnamTimeZone);
            return new CategoryResponse
            {
                CategoryCode = category.CategoryCode,
                CategoryName = category.CategoryName,
                Status = category.Status,
               IsDeleted = category.IsDeleted,   
               TotalBooks=  totalBook,
                CreatedAt = createdAtVN,
                UpdatedAt = updatedAtVN,
                Id = category.Id
            };
        }
    }  
}
