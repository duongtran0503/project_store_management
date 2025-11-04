using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Modules.Suppliers.Dtos.Requests;
using StoreManagement.API.Shared.Data;

namespace StoreManagement.API.Modules.Suppliers.Repository
{
    public class SupplierRepository
    {
        private readonly ApplicationDbContext _context;
        public SupplierRepository(ApplicationDbContext context) { _context = context; }
    
        public async Task<Supplier> CreateSupplierAsync(Supplier su)
        {
            _context.Suppliers.Add(su);
            await _context.SaveChangesAsync();
            return su;
        }

        public async Task<(List<Supplier>,int totalSupplier)> GetPageSupplierAsync(int pageSize,int pageNumber)
        {
           var data = await _context.Suppliers
                .AsNoTracking()
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();
            int total = data.Count();
            return (data, total);
        }

        public async Task<Supplier?> GetSupplierByIdAsync(string id)
        {
            return await _context.Suppliers.FindAsync(id);
        }
        public async Task<Supplier> UpdateSupplierAsync(Supplier s)
        {
            _context.Suppliers.Update(s);
            await _context.SaveChangesAsync();
            return s;
        }
        public async Task<bool> CheckSupplierByPhone(string phone)
        {
            return await _context.Suppliers.AnyAsync(s=>s.Phone == phone);
        }

        public async Task<List<Supplier>> SearchSupplierAsync(SearchSupplierRequest filter)
        {
            var query = _context.Suppliers.AsNoTracking().AsQueryable();
            if (!String.IsNullOrEmpty(filter.Query)) { 
                var queryValue = filter.Query.ToLower();
                query = query.Where(s=>s.SupplierName.Contains(queryValue) || s.Phone.Contains(queryValue));
            }
             return await query
                .Skip((filter.PageNumber-1)*filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }
        
    }
}
