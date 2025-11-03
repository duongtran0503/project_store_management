using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Shared.Data;

namespace StoreManagement.API.Modules.Products.Repository
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _context;
        
        public ProductRepository(ApplicationDbContext context) { _context = context; }

        public async Task<Book> CreateBookAsync(Book b)
        {
            _context.Books.Add(b);
            await _context.SaveChangesAsync();
            return b;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _context.Books.
                 Include(b=>b.Category)
                .AsNoTracking().ToListAsync();
        }

        public async Task<bool> CheckBookByISBN(string isbn)
        {
            return await _context.Books.AnyAsync(b => b.Isbn == isbn);
        }
    }
}
