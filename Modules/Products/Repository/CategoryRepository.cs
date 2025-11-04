using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Shared.Data;

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
            return await _context.Categories.AnyAsync(c => c.CategoryCode == code);
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> CheckCategoryHaveProduct(string categoryId)
        {
           
            return await _context.Books
                .AsNoTracking()
                .AnyAsync(b => b.CategoryId == categoryId);
        }

        public async Task DeleteCategory(Category category)
        {
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<Category?> GetCategoryById(string id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}
