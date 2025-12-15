using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Modules.Products.Constants;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Shared.Data;
using TimeZoneConverter;

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

        public async Task<List<AuthorResponse>> GetPageAuthorAsync(int pageNumber, int pageSize)
        {
          
            var authors = await _context.Authorities
                .Where(p => p.Status == AuthorStatusConstants.DEFAULT && p.IsDeleted == false)
                .AsNoTracking()
                .OrderBy(p => p.Name) 
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuthorResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Code = a.Code,
                    Status = a.Status,
                    IsDeleted = a.IsDeleted,
                    TotalBook = a.Books.Count(),
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                })
                .ToListAsync();

            return (authors);
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

        public async Task<List<AuthorResponse>> FilterAuthorAsync(FiltertAuthorRequest request)
        {
            var query = _context.Authorities.AsNoTracking().AsQueryable();

            if(!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                query = query.Where(au => au.Name.ToLower().Contains(term) || au.Code.ToLower().Contains(term));
            }
            return await query.AsNoTracking()
                .OrderBy(p => p.Name)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new AuthorResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Code = a.Code,
                    Status = a.Status,
                    IsDeleted = a.IsDeleted,
                    TotalBook = a.Books.Count(),
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                })
                .ToListAsync();
        }
        public async Task<List<Author>> GetAuthorsByIds(List<string> ids)
        {
            if (ids == null || !ids.Any()) return new List<Author>();

            return await _context.Authorities
                .AsNoTracking()
                .Where(a => ids.Contains(a.Id))
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
