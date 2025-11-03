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

        public async Task<Category?> GetCategoryById(string id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}
