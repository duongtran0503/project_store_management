using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Shared.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace StoreManagement.API.Modules.Products.Repository
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateCategoryAsync(Category ca)
        {
            await _context.Categories.AddAsync(ca);
            await _context.SaveChangesAsync();
            return ca;
        }

        public  async Task<bool> CheckCategoryByCategoryCodeAsync(string code)
        {
            return await _context.Categories.IgnoreQueryFilters().AnyAsync(c => c.CategoryCode == code);
        }

        public async Task<List<Category>> GetCategories(int pageNumber,int pageSize)
        {
            return await _context.Categories
                .AsNoTracking()
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<CategoryWithDetail>> GetPageCategoriesWithDetailAsync(int pageNumber,int pageSize)
        {
            return await _context.Categories
        .AsNoTracking()
      
        .Select(c => new CategoryWithDetail
        {
             Category = new CategoryResponse
             {
                 CategoryCode = c.CategoryCode,
                 CategoryName = c.CategoryName,
                 Status = c.Status,
                 IsDeleted = c.IsDeleted,
                 CreatedAt = c.CreatedAt,
                 UpdatedAt = c.UpdatedAt,
                 Id = c.Id
             },
             TotalBooks = c.Books.Count()
        })
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
        }

        public async Task<List<Category>> FilterCategoryAsync(FilterCategoryRequest request)
        {
            var query = _context.Categories.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                query = query.Where(au => au.CategoryName.ToLower().Contains(term) || au.CategoryCode.ToLower().Contains(term));
            }
            return await query.AsNoTracking().Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
        }
        public async Task<List<SuggestionsResponse>> GetSuggestionsAsync(FilterCategoryRequest request)
        {
            var query = _context.Categories.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                query = query.Where(au => au.CategoryName.ToLower().Contains(term) || au.CategoryCode.ToLower().Contains(term));
            }
            return await query
        .Select(au => new SuggestionsResponse
        {
            Id = au.Id,
            Title = au.CategoryName
        })
        .ToListAsync();
        }

    
    public async Task<bool> CheckCategoryHaveProduct(string categoryId)
        {
           
            return await _context.Books
                .AsNoTracking()
                .IgnoreQueryFilters()
                .AnyAsync(b => b.CategoryId == categoryId);
        }

       

        public async Task<Category> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<Category?> GetCategoryById(string id)
        {
            return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c=>c.Id==id);
        }
        public async Task<int> GetTotalBookAsync(string id)
        {
            return await _context.Books
         .CountAsync(b => b.CategoryId == id);
        }

     
    }
}
