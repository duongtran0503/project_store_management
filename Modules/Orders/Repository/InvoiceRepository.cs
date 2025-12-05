using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Modules.Orders.Constants;
using StoreManagement.API.Modules.Orders.ErrorCode;
using StoreManagement.API.Shared.Data;

namespace StoreManagement.API.Modules.Orders.Repository
{
    public class InvoiceRepository
    {
        private readonly ApplicationDbContext _context;
        public InvoiceRepository(ApplicationDbContext context) { 
         _context = context;
        }
        public async Task<Invoice> CreateInvoiceAsync(
          Invoice invoice,
          List<InvoiceDetail> invoiceDetails,
          Dictionary<string, int> stockReductions)
        {
            var executionStrategy = _context.Database.CreateExecutionStrategy();

            return await executionStrategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                  
                    _context.Invoices.Add(invoice);
                    await _context.SaveChangesAsync();
                    foreach (var detail in invoiceDetails)
                    {
                      
                         
                        detail.InvoiceId = invoice.Id;
                      
                        detail.Invoice = invoice;
                    }

                    _context.InvoiceDetails.AddRange(invoiceDetails);
                    await _context.SaveChangesAsync();

                  
                    var bookIds = stockReductions.Keys.ToList();

                   
                    var inventories = await _context.Inventories
                                                    .Where(i => bookIds.Contains(i.BookId))
                                                    .ToDictionaryAsync(i => i.BookId);

                    foreach (var kvp in stockReductions)
                    {
                        if (inventories.TryGetValue(kvp.Key, out var inventory))
                        {
                           
                            inventory.AvailableStock -= kvp.Value;
                            if(invoice.OrderType==InvoiceOrderTypeConstant.ONLINE)
                            {
                                if(invoice.Status==InvoiceStatusConstant.PENDING)
                                {
                                    inventory.ReservedStock += kvp.Value;
                                }
                            }

                            
                        }
                        else
                        {

                            throw new AppException(InvoiceErrorCode.OutOfStock);
                        }
                    }

                  
                    await _context.SaveChangesAsync();

                   
                    await transaction.CommitAsync();

                    return invoice;
                }
                catch (Exception)
                {
                   
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task<Dictionary<string,decimal>> GetPriceBooksOrderAsync(List<string> bookIds)
        {
            return await _context.Books.Where(b => bookIds.Contains(b.Id))
                .Select(b => new { b.Id, b.RetailPrice })
                .AsNoTracking()
                .ToDictionaryAsync(b => b.Id, b => b.RetailPrice);
        }

        public async Task<Dictionary<string,Book>> GetListinfoBookOrder(List<string> ids)
        {
            return await _context.Books
                .Where(b => ids.Contains(b.Id))
                .AsNoTracking().Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .ToDictionaryAsync(b => b.Id, b => b);
                
        }

        public async Task<List<Invoice>> GetPageInvoices(int pageNumber,int pageSize)
        {
            return await _context.Invoices.AsNoTracking()
                .Include(i => i.Customer)
        .Include(i => i.CashierStaff)
        .Include(i => i.InvoiceDetails)

            .ThenInclude(d => d.Book)
            .OrderByDescending(b=>b.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Customer?> GetInfoCustomerOrder(string id)
        {
            return await _context.customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Account?> GetInfoStaff(string id)
        {
            return await _context.Accounts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task<Voucher?> GetVoucherByid(string id)
        {
            return await _context.Vouchers.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        }


    }
}
