using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Shared.Data;

namespace StoreManagement.API.Modules.Users.Repository
{
    public class CustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<Customer> CreateCustomerAsync(Customer c)
        {
            _context.customers.Add(c);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task<Customer?> GetCustomerByPhone(string phone)
        {
            return await _context.customers.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(
                u=>u.Phone==phone);
        }

        public async Task<Customer?> GetCustomerById(string id)
        {
            return await _context.customers.IgnoreQueryFilters().AsNoTracking().FirstOrDefaultAsync(
                u => u.Id == id);
        }

        public async Task<List<Customer>> GetPageCustomer(int pageNumber,int pageSize)
        {
            return await _context.customers.AsNoTracking()
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();
        }


        public async Task<bool> CheckCustomerByPhoneAsync(string phone)
        {
            return await _context.customers.IgnoreQueryFilters().AsNoTracking().AnyAsync(
                u=>u.Phone==phone);
        }
    }
}
