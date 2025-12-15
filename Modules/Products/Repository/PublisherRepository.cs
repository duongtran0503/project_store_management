using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Modules.Products.Constants;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Shared.Data;

namespace StoreManagement.API.Modules.Products.Repository
{
    public class PublisherRepository
    {

        private readonly ApplicationDbContext _context;
        public PublisherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Publisher?> GetPublisherById(string id)
        {
            return await _context.Publisher.AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Publisher> CreatePublisherAsync(Publisher p)
        {
                 _context.Publisher.Add(p);
            await _context.SaveChangesAsync();  
            return p;
        } 

        public async Task<bool> CheckPublisherByCodeAsync(string code)
        {
            return await _context.Publisher.IgnoreQueryFilters().AnyAsync(p=>p.Code == code);
        }


        public async Task<(List<Publisher> publishers, int totalCount)> GetPagePublisherAsync(int pageNumber, int pageSize)
        {
            var publishers  = await _context.Publisher
      
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (publishers, publishers.Count);
        }

        public async Task<List<Publisher>> GetAllAsync()
        {
            return await _context.Publisher
         .AsNoTracking() 
         .ToListAsync();
        }

        public async Task<Publisher> UpdatePublisherAsync(Publisher publisher)
        {
            _context.Publisher.Update(publisher);
            await _context.SaveChangesAsync();
            return publisher;
        }

        public async Task<Publisher> RestorePublisherAsync(Publisher publisher)
        {
            _context.Publisher.Update(publisher);
            await _context.SaveChangesAsync();
            return publisher;
        }

        public async Task<List<Publisher>> FilterPublisherAsync(FIlterPublisherRequest request)
        {
            var query = _context.Publisher.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                query = query.Where(au => au.Name.ToLower().Contains(term) || au.Code.ToLower().Contains(term));
            }
            return await query.AsNoTracking().Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
        }

        public async Task<List<SuggestionsResponse>> GetSuggestionsAsync(FIlterPublisherRequest request)
        {
            var query = _context.Publisher.AsNoTracking().AsQueryable();
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

        public async Task<List<Publisher>> GetPublishersByIds(List<string> ids)
        {
            if (ids == null || !ids.Any()) return new List<Publisher>();

            return await _context.Publisher
                .AsNoTracking()
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();
        }


    }
}
