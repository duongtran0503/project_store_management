using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Modules.Inventories.Constants;
using StoreManagement.API.Modules.Inventories.Dtos.Response;
using StoreManagement.API.Modules.Inventories.ErrorCode;
using StoreManagement.API.Shared.Data;
using System.Text;
using TimeZoneConverter;

namespace StoreManagement.API.Modules.Inventories.Repository
{
    public class InventoryRepository
    {
      private readonly ApplicationDbContext _context;
        public InventoryRepository(ApplicationDbContext context) { 
         _context = context;
        }

        public async Task<Supplier?> GetSupplierByIdAsync(string id)
        {
            return await _context.Suppliers.AsNoTracking().IgnoreQueryFilters().FirstOrDefaultAsync(s=>s.Id == id); 
        }

       
        public async Task<InventoryReceipt> CreateInventoryReceiptAsync(
        InventoryReceipt receipt,
        List<ReceiptDetail> receiptDetails,
        Dictionary<string, int> stockUpdates)
        {
            var executionStrategy = _context.Database.CreateExecutionStrategy();


            return await executionStrategy.ExecuteAsync(async () =>
            {

                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {

                    _context.InventoryReceipts.Add(receipt);
                    await _context.SaveChangesAsync();

                    _context.ReceiptDetails.AddRange(receiptDetails.Select(r =>
                    {
                        r.ReceiptId = receipt.Id;
                        r.Receipt = receipt;
                        return r;
                    }).ToList());
                   


                    var bookIds = stockUpdates.Keys.ToList();
                    var inventories = await _context.Inventories
                                                    .Where(i => bookIds.Contains(i.BookId))

                                                    .ToDictionaryAsync(i => i.BookId);

                    foreach (var kvp in stockUpdates)
                    {
                        if (inventories.TryGetValue(kvp.Key, out var inventory))
                        {

                           if(receipt.GRNStatus ==GRNStatusConstant.COMPLETED)
                            {
                                inventory.AvailableStock += kvp.Value;
                            }
                        }
                        else
                        {

                            var newInventory = new Inventory
                            {
                                BookId = kvp.Key,
                                AvailableStock = kvp.Value,
                                ReservedStock = 0,

                            };
                            _context.Inventories.Add(newInventory);
                        }
                    }


                    await _context.SaveChangesAsync();


                    await transaction.CommitAsync();

                    return await _context.InventoryReceipts
           .Include(r => r.Supplier) 
           .Include(r => r.ReceivingStaff)
           .Include(r => r.ReceiptDetails)
            
               .ThenInclude(d => d.Book) 
           .FirstAsync(r => r.Id == receipt.Id);

                 
                }
                catch (Exception)
                {

                    await transaction.RollbackAsync();
                    throw;
                }
            });

        }

        public async Task<InventoryReceipt> UpdateGRNStatusAsync(InventoryReceipt inventoryReceipt)
        {

            var executionStrategy = _context.Database.CreateExecutionStrategy();

            return await executionStrategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    _context.InventoryReceipts.Update(inventoryReceipt);
                    await _context.SaveChangesAsync();
                    var newStockUpdate = inventoryReceipt.ReceiptDetails.Select(d => new { Id = d.BookId, quantity = d.QuantityReceived })
               .ToDictionary(k => k.Id, v => v.quantity);

                    if (inventoryReceipt.GRNStatus == GRNStatusConstant.COMPLETED)
                    {
                        await UpdateStocksInTransactionAsync(newStockUpdate);
                    }

                    await transaction.CommitAsync();
                    return inventoryReceipt;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
           
           
        }
        public async Task<List<InventoryReceipt>> GetPageIventoryReceiptAsync(int pageNumber, int pageSize)
        {
            return  await _context.InventoryReceipts.AsNoTracking()
                .Include(i => i.ReceiptDetails)
                .ThenInclude(id=>id.Book)
                .Include(i=>i.Supplier)
                .Include(i=>i.ReceivingStaff)
                
                  .Skip((pageNumber - 1) * pageSize)
         .Take(pageSize).
         OrderByDescending(i=>i.UpdatedAt)
         .
         ToListAsync();
            
            
        }


        public async Task<InventoryReceipt?> GetInventoryReceiptWithDetailsAsync(string receiptId)
        {
            
            return await _context.InventoryReceipts
                .Include(r => r.ReceiptDetails)
                  .ThenInclude(id => id.Book)

                .Include(i => i.Supplier)
                .Include(i => i.ReceivingStaff)
                .FirstOrDefaultAsync(r => r.Id == receiptId);
        }

        public async Task<InventoryReceipt> UpdateInventoryReceiptAsync(
    InventoryReceipt receiptToUpdate,
    List<ReceiptDetail> newDetails,
    Dictionary<string, int> undoStockUpdates,
    Dictionary<string, int> newStockUpdates)
        {
            var executionStrategy = _context.Database.CreateExecutionStrategy();

            return await executionStrategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    

                  
                   if(receiptToUpdate.GRNStatus==GRNStatusConstant.COMPLETED)
                    {
                        await UpdateStocksInTransactionAsync(undoStockUpdates);
                    }

                   
                  
                    _context.ReceiptDetails.RemoveRange(receiptToUpdate.ReceiptDetails);

                  
                    _context.InventoryReceipts.Update(receiptToUpdate);

                 
                    foreach (var detail in newDetails)
                    {
                      
                        detail.ReceiptId = receiptToUpdate.Id;
                    }
                    _context.ReceiptDetails.AddRange(newDetails);

                   
                    await _context.SaveChangesAsync();

                    if (receiptToUpdate.GRNStatus == GRNStatusConstant.COMPLETED)
                    {
                        await UpdateStocksInTransactionAsync(newStockUpdates);
                    }

                  

                 
                    await transaction.CommitAsync();

                    return receiptToUpdate;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task CheckStockAvailabilityAsync(Dictionary<string, int> stockReductions)
        {
            var bookIds = stockReductions.Keys.ToList();

          
            var inventories = await _context.Inventories
                .Where(i => bookIds.Contains(i.BookId))
                .ToDictionaryAsync(i => i.BookId);

            var errors = new List<string>();
            foreach (var kvp in stockReductions)
            {
                string bookId = kvp.Key;
                int quantityToSell = kvp.Value;

                if (inventories.TryGetValue(bookId, out var inventory))
                {
                   
                    if (inventory.StockCanBeSold < quantityToSell)
                    {
                        errors.Add($"Sách ID '{bookId}' không đủ tồn kho. Yêu cầu: {quantityToSell}, Khả dụng: {inventory.StockCanBeSold}");
                    }
                }
                else
                {
                   
                    errors.Add($"Sách ID '{bookId}' không tồn tại trong hệ thống tồn kho.");
                }
            }

       
            if (errors.Any())
            {
                
              
                throw new AppException(InventoryErrorCode.OutOfStock, errors.First());
            }
        }


        public async Task<List<InventoryReceiptItemResponse>> GetPagedReceiptDetails(int pageNumber, int pageSize)
        {
            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            var responseList = await _context.ReceiptDetails.AsNoTracking()
         .Skip((pageNumber - 1) * pageSize)
         .Take(pageSize)
         .Where(rd=>rd.Receipt.GRNStatus==GRNStatusConstant.COMPLETED)
         .Include(rd => rd.Book)
             .ThenInclude(b => b.Inventory)
         .Include(rd => rd.Receipt) 
             .ThenInclude(ir => ir.Supplier) 
         .Select(rd => new InventoryReceiptItemResponse
         {
             Id = rd.Id,
             Name = rd.Book.Title,
             ImportQuantity = rd.QuantityReceived,
             ImportPrice = rd.UnitCost,
             TotalValueImport = rd.UnitCost*rd.QuantityReceived,
             SupplierName = rd.Receipt.Supplier.SupplierName,
             DateImport = TimeZoneInfo.ConvertTimeFromUtc(rd.Receipt.ReceiptDate,vietnamTimeZone),
             
         })
         .ToListAsync();

            return responseList;
        }

        public async Task<List<InventoryResponse>> GetpageInventory(int pageNumber,int PageSizse)
        {
            var inventories = await _context.Inventories.AsNoTracking()
                .Include(i => i.Book)
                .Skip((pageNumber - 1) * PageSizse)
                .Take(PageSizse)
                .ToListAsync();
            var bookids = inventories.Select(i => i.BookId).ToList();

            var booksAndCost = await _context.ReceiptDetails.AsNoTracking()
              .Where(rd => bookids.Contains(rd.BookId) && rd.Receipt.GRNStatus == GRNStatusConstant.COMPLETED)
                .GroupBy(r => r.BookId)
                .Select(g => new
                {
                    BookId = g.Key,
                    TotalValue = g.Sum(rd => rd.QuantityReceived * rd.UnitCost),
                    totalQuantity = g.Sum(rd => rd.QuantityReceived),
                }).ToListAsync();

            var costLookup = booksAndCost.ToDictionary(
                k => k.BookId,
                v =>
                 new
                 {
                     avgCost = v.totalQuantity > 0 ? v.TotalValue / v.totalQuantity : 0,
                     totalQuantity = v.totalQuantity
                 }
                );

            var result = inventories.Select(i =>
            {
                var avgCost = costLookup.ContainsKey(i.BookId) ? costLookup[i.BookId].avgCost : decimal.Zero;
                return new InventoryResponse
                {
                    BookId = i.BookId,
                    AvailableStock = i.AvailableStock,
                    ReservedStock = i.ReservedStock,
                    MinStockLevel = 10,
                    BookName = i.Book.Title,
                    SKU = i.Book.Isbn,
                    AverageCostPrice = avgCost,
                    TotalInventoryValue = i.AvailableStock * avgCost

                };
            }).ToList();
            return result;
        }


        private async Task UpdateStocksInTransactionAsync(Dictionary<string, int> stockUpdates)
        {
            var bookIds = stockUpdates.Keys.ToList();


            var inventories = await _context.Inventories
                                            .Where(i => bookIds.Contains(i.BookId))
                                            .ToDictionaryAsync(i => i.BookId);

            foreach (var kvp in stockUpdates)
            {
                if (inventories.TryGetValue(kvp.Key, out var inventory))
                {
                  
                    inventory.AvailableStock += kvp.Value;
                }
                else if (kvp.Value > 0)
                {
                    
                    var newInventory = new Inventory
                    {
                        BookId = kvp.Key,
                        AvailableStock = kvp.Value,
                        ReservedStock = 0
                    };
                    _context.Inventories.Add(newInventory);
                }
              
            }
            await _context.SaveChangesAsync();
        }

    }
}
