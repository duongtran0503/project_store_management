using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Modules.Products.Constants;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Shared.Data;

namespace StoreManagement.API.Modules.Products.Repository
{
    public class AuthorRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthorRepository(ApplicationDbContext context) { 
         _context = context;
        }

        public async Task<Author?> GetAuthorByIdAsync(string id)
        {
            return await _context.Authorities
                .AsNoTracking()
        .IgnoreQueryFilters()
        .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> CheckAuthorByCode(string code)
        {
            return await _context.Authorities.IgnoreQueryFilters().AnyAsync(c=>c.Code == code);
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            _context.Authorities.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<(List<Author> authors, int totalCount)> GetPageAuthorAsync(int pageNumber, int pageSize)
        {
            var authors = await _context.Authorities
                .Where(p => p.Status == AuthorStatusConstants.DEFAULT && p.IsDeleted==false)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (authors, authors.Count);
        }

        public async Task<int> GetTotalBookAsnc(string id)
        {
            return await _context.Books
         .CountAsync(b => b.CategoryId == id);
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _context.Authorities
          .AsNoTracking()
          .ToListAsync();
        }

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
             _context.Authorities.Update(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author> RestoreAuthorAsync(Author author)
        {
            _context.Authorities.Update(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<List<Author>> FilterAuthorAsync(FiltertAuthorRequest request)
        {
            var query = _context.Authorities.AsNoTracking().AsQueryable();

            if(!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                query = query.Where(au => au.Name.ToLower().Contains(term) || au.Code.ToLower().Contains(term));
            }
            return await query.AsNoTracking().Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
        }

        public async Task<List<SuggestionsResponse>> GetSuggestionsAsync(FiltertAuthorRequest request)
        {
            var query = _context.Authorities.AsNoTracking().AsQueryable();
            query = query.Take(10);
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                query = query.Where(au => au.Name.ToLower().Contains(term) || au.Code.ToLower().Contains(term));
            }
            return await query
         .Select(au => new SuggestionsResponse
         {
             Id = au.Id,
             Title = au.Name
         })
         .ToListAsync();
        }

    }
}
