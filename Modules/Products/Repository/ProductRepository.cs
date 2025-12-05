using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Modules.Products.Constants;
using StoreManagement.API.Modules.Products.Dtos.Request;
using StoreManagement.API.Modules.Products.Dtos.Response;
using StoreManagement.API.Modules.Promotions.Constants;
using StoreManagement.API.Shared.Data;
using TimeZoneConverter;

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
            var initialInventory = new Inventory
            {
                BookId = b.Id, 
                AvailableStock = 0, 
                ReservedStock = 0,
               
            };
         
            _context.Inventories.Add(initialInventory);
            await _context.SaveChangesAsync();
            b.Inventory = initialInventory;
            return b;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _context.Books
             .Include(b => b.Category)
                 .Include(b => b.Author)
                 .Include(b => b.Publisher)
                 .Include(b => b.Inventory)
                .AsNoTracking().ToListAsync();
        }

        public async Task<bool> CheckBookByISBN(string isbn)
        {
            return await _context.Books.AsNoTracking().IgnoreQueryFilters().AnyAsync(b => b.Isbn == isbn);
        }

        public async Task<Book?> GetBookByIdAsync(string id)
        {
          return await   _context.Books
                .IgnoreQueryFilters()
          .AsNoTracking()
          .Include(b => b.Category)
                 .Include(b => b.Author)
                 .Include(b => b.Publisher)
                 .Include(b => b.Inventory)
          .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<(List<Book> books, int totalCount)> GetPagedBooksDeletedAsync(int pageNumber, int pageSize)
        {

            var totalCount = await _context.Books.IgnoreQueryFilters().Where(b => b.IsDeleted == true).CountAsync();


            var books = await _context.Books
                .IgnoreQueryFilters()
               .Where(b=>b.IsDeleted==true)
               .Include(b => b.Category)
                 .Include(b => b.Author)
                 .Include(b => b.Publisher)
                 .Include(b => b.Inventory)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (books, totalCount);
        }
        public async Task<(List<Book> books, int totalCount)> GetPagedBooksAsync(int pageNumber, int pageSize)
        {
           
        

        
            var books = await _context.Books
              
            .Include(b => b.Category)
                 .Include(b => b.Author)
                 .Include(b => b.Publisher)
                 .Include(b => b.Inventory)
                 .OrderByDescending(b=>b.CreatedAt)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (books, books.Count);
        }

        public async Task<List<BookResponse>> GetProductListWithVouchersAsync(int pageNumber,int pageSize)
        {
            var now = DateTime.UtcNow;
            var activeVoucherTargets = await _context.voucherTargets
                .AsNoTracking()
        .Include(vt => vt.Voucher)
        .Where(vt => (vt.TargetType == VoucherTargetConstants.ProductTarget ||
                      vt.TargetType == VoucherTargetConstants.CategoryTarget) &&
                     vt.Voucher.IsActive == true &&
                    vt.Voucher.UsageCount>0&&
                     !vt.Voucher.IsDeleted &&
                     vt.Voucher.StartDate <= now &&
                     vt.Voucher.EndDate >= now)
        .ToListAsync();

            var productVoucherLookup = activeVoucherTargets
        .Where(vt => vt.TargetType == VoucherTargetConstants.ProductTarget)
        .ToLookup(vt => vt.TargetId, vt => vt.Voucher);

            var categoryVoucherLookup = activeVoucherTargets
        .Where(vt => vt.TargetType == VoucherTargetConstants.CategoryTarget)
        .ToLookup(vt => vt.TargetId, vt => vt.Voucher);

            var books = await _context.Books
                .AsNoTracking()
        .Include(b => b.Category) 
        .Include(b=>b.Author)
        .Include(b=>b.Publisher)
        .Include(b => b.Inventory)
        .Skip((pageNumber-1)*pageSize)
        .Take(pageSize)
        .ToListAsync();
            List<BookResponse> results = new List<BookResponse>();
            foreach(var book in books)
            {
                var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
                DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(book.CreatedAt, vietnamTimeZone);
                DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(book.UpdatedAt, vietnamTimeZone);
                var bookRes = new BookResponse
                {
                    Id = book.Id,
                    CreatedAt = createdAtVN,
                    UpdatedAt = updatedAtVN,
                    Title = book.Title,
                    Author = book.Author.Name,
                    AuthorId = book.AuthorId,
                    Publisher = book.Publisher.Name,
                    PublisherId = book.PublisherId,
                    Isbn = book.Isbn,
                    Image = book.Image,
                    CategoryId = book.CategoryId,
                    RetailPrice = book.RetailPrice,
                    Status = book.Status,   
                     StockCanBeSold = book.Inventory.StockCanBeSold,
                     
                    Description ="",
             
                    CategoryName = book.Category.CategoryName
                };
                var productVouchers = productVoucherLookup[book.Id];
                var categoryVouchers = categoryVoucherLookup[book.CategoryId];
                var allCandidateVouchers = productVouchers.Concat(categoryVouchers)
            .DistinctBy(v => v.Id)
            .ToList();
                AppliedVoucher bestVoucherDto = default!;
                decimal maxDiscountAmount = 0m;
                foreach (var voucher in allCandidateVouchers)
                {
                    
                    decimal currentDiscount = CalculateDiscountAmount(voucher, book.RetailPrice);

                    if (currentDiscount > maxDiscountAmount)
                    {
                        maxDiscountAmount = currentDiscount;

                       
                        bestVoucherDto = new AppliedVoucher
                        {
                            VoucherId = voucher.Id,
                            Name = voucher.Name,
                            Code = voucher.Code,
                            DiscountValue = voucher.DiscountValue,
                            Type = voucher.Type,
                            DiscountPrice = maxDiscountAmount,
                        };
                    }
                }

                bookRes.ActiveVoucher = bestVoucherDto != null ? bestVoucherDto : default!;
                

                results.Add(bookRes);

            }
            return results;

        }

        public async Task<BookResponse?> GetProductByIdWithVouchersAsync(string bookId)
        {
            var now = DateTime.UtcNow;

          
            var activeVoucherTargets = await _context.voucherTargets
                .AsNoTracking()
                .Include(vt => vt.Voucher)
                .Where(vt => (vt.TargetType == VoucherTargetConstants.ProductTarget ||
                              vt.TargetType == VoucherTargetConstants.CategoryTarget) &&
                             vt.Voucher.IsActive == true &&
                             vt.Voucher.UsageCount > 0 &&
                             !vt.Voucher.IsDeleted &&
                             vt.Voucher.StartDate <= now &&
                             vt.Voucher.EndDate >= now)
                .ToListAsync();

          
            var productVoucherLookup = activeVoucherTargets
                .Where(vt => vt.TargetType == VoucherTargetConstants.ProductTarget)
                .ToLookup(vt => vt.TargetId, vt => vt.Voucher);

            var categoryVoucherLookup = activeVoucherTargets
                .Where(vt => vt.TargetType == VoucherTargetConstants.CategoryTarget)
                .ToLookup(vt => vt.TargetId, vt => vt.Voucher);

  
            var book = await _context.Books
                .AsNoTracking()
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Inventory)
                .Where(b => b.Id == bookId) 
                .FirstOrDefaultAsync(); 
            if (book == null)
            {
                return null; 
            }

            
            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(book.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(book.UpdatedAt, vietnamTimeZone);

            var bookRes = new BookResponse
            {
                Id = book.Id,
                CreatedAt = createdAtVN,
                UpdatedAt = updatedAtVN,
                Title = book.Title,
                Author = book.Author.Name,
                AuthorId = book.AuthorId,
                Publisher = book.Publisher.Name,
                PublisherId = book.PublisherId,
                Isbn = book.Isbn,
                Image = book.Image,
                CategoryId = book.CategoryId,
                RetailPrice = book.RetailPrice,
                Status = book.Status,
                StockCanBeSold = book.Inventory.StockCanBeSold,
              
                Description = "", 
                CategoryName = book.Category.CategoryName
            };

            var productVouchers = productVoucherLookup[book.Id];
            var categoryVouchers = categoryVoucherLookup[book.CategoryId];

            
            var allCandidateVouchers = productVouchers.Concat(categoryVouchers)
                .DistinctBy(v => v.Id)
                .ToList();

            AppliedVoucher bestVoucherDto = default!;
            decimal maxDiscountAmount = 0m;

           
            foreach (var voucher in allCandidateVouchers)
            {
               
                decimal currentDiscount = CalculateDiscountAmount(voucher, book.RetailPrice);

                if (currentDiscount > maxDiscountAmount)
                {
                    maxDiscountAmount = currentDiscount;

                    bestVoucherDto = new AppliedVoucher
                    {
                        VoucherId = voucher.Id,
                        Name = voucher.Name,
                        Code = voucher.Code,
                        DiscountValue = voucher.DiscountValue,
                        Type = voucher.Type,
                        DiscountPrice = maxDiscountAmount,
                    };
                }
            }

            bookRes.ActiveVoucher = bestVoucherDto != null ? bestVoucherDto : default!;

            return bookRes;
        }
        private decimal CalculateDiscountAmount(Voucher voucher, decimal productPrice)
        {

            if (productPrice < voucher.MinOrderValue)
            {
                return 0m;
            }



            decimal discountPercentage = voucher.DiscountValue / 100m;
            decimal potentialDiscount = productPrice * discountPercentage;

            decimal finalDiscount = 0m;


            if ( voucher.MaxDiscountValue > 0)
            {

                finalDiscount = Math.Min(potentialDiscount, voucher.MaxDiscountValue);
            }
            else
            {
              
                finalDiscount = potentialDiscount;
            }

          
            finalDiscount = Math.Min(finalDiscount, productPrice);

            return finalDiscount;
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
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(term) ||
                    b.Author.Name.ToLower().Contains(term)
                );
            }
            if (!string.IsNullOrEmpty(request.AuthorName))
            {
                query = query.Where(b => b.Author.Name.ToLower().Contains(request.AuthorName.ToLower()));
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
                    ? query.OrderByDescending(b => b.CreatedAt)
                    : query.OrderBy(b => b.CreatedAt);
            }

            var books = await query
                 .Include(b => b.Category)
                 .Include(b=>b.Author)
                 .Include(b=>b.Publisher)
                 .Include(b => b.Inventory)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            var totalCount = books.Count();
            return (books, totalCount);
        }

        public async Task<List<SuggestionsResponse>> GetSuggestionsAsync(FilterProductRequest request)
        {
            var query = _context.Books.AsNoTracking().AsQueryable();
            query = query.Take(10);
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                query = query.Where(au => au.Title.ToLower().Contains(term));
            }
            return await query
         .Select(au => new SuggestionsResponse
         {
             Id = au.Id,
             Title = au.Title,
             Image = au.Image,
             
         })
         .ToListAsync();
        }
    }
}
