using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Modules.Products.Constants;
using StoreManagement.API.Modules.Products.Dtos.Request;
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

        public async Task<Book?> GetBookByIdAsync(string id)
        {
          return await   _context.Books
          .AsNoTracking() 
          .Include(b => b.Category)
          .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<(List<Book> books, int totalCount)> GetPagedBooksDeletedAsync(int pageNumber, int pageSize)
        {

            var totalCount = await _context.Books.Where(b => b.IsAvailable == false).CountAsync();


            var books = await _context.Books
                .Where(b => b.IsAvailable == false)
                .Include(b => b.Category)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (books, totalCount);
        }
        public async Task<(List<Book> books, int totalCount)> GetPagedBooksAsync(int pageNumber, int pageSize)
        {
           
            var totalCount = await _context.Books.Where(b=>b.IsAvailable==true).CountAsync();

        
            var books = await _context.Books
                .Where(b=>b.IsAvailable == true)
                .Include(b => b.Category)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (books, totalCount);
        }


        public async Task<Book> UpdateBookAsync(Book b)
        {
            _context.Books.Update(b);
            await _context.SaveChangesAsync();
            return b;
        }

        public async Task<(List<Book> books, int totalCount)> GetFilteredBooksAsync(FilterProductRequest request)
        {
            var query = _context.Books.AsNoTracking().AsQueryable();
            query = query.Where(b => b.IsAvailable == true);
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(term) ||
                    b.Author.ToLower().Contains(term)
                );
            }
            if (!string.IsNullOrEmpty(request.AuthorName))
            {
                query = query.Where(b => b.Author.ToLower().Contains(request.AuthorName.ToLower()));
            }

 
            if (!string.IsNullOrEmpty(request.CategoryId))
            {
                query = query.Where(b => b.CategoryId == request.CategoryId);
            }

  
            if (request.MinPrice.HasValue)
            {
                query = query.Where(b => b.RetailPrice >= request.MinPrice.Value);
            }
            if (request.MaxPrice.HasValue)
            {
                query = query.Where(b => b.RetailPrice <= request.MaxPrice.Value);
            }

            var sortBy = request.SortBy?.ToLower() ?? ProductConstants.PRODUCT_SORT_CREATEDAT; 
            var sortOrder = request.SortOrder?.ToLower() ?? ProductConstants.PRODUCT_SORT_DESC;

            if (sortBy == ProductConstants.PRODUCT_SORT_PRICE)
            {
                query = (sortOrder == ProductConstants.PRODUCT_SORT_ASC)
                    ? query.OrderBy(b => b.RetailPrice)
                    : query.OrderByDescending(b => b.RetailPrice);
            }
         
            else 
            {
                query = (sortOrder == ProductConstants.PRODUCT_SORT_ASC)
                    ? query.OrderBy(b => b.CreatedAt)
                    : query.OrderByDescending(b => b.CreatedAt);
            }




           

            var books = await query
                 .Include(b => b.Category)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            var totalCount = books.Count();
            return (books, totalCount);
        }
    }
}
