using Microsoft.EntityFrameworkCore;
using StoreManagement.API.Common.Entities;
using StoreManagement.API.Modules.Promotions.Constants;
using StoreManagement.API.Modules.Promotions.Dtos.Requests;
using StoreManagement.API.Modules.Promotions.Dtos.Responses;
using StoreManagement.API.Shared.Data;
using TimeZoneConverter;

namespace StoreManagement.API.Modules.Promotions.Repository
{
    public class VoucherRepository
    {
        private readonly ApplicationDbContext _context;
        public VoucherRepository(ApplicationDbContext context) { 
         _context = context;
        }

        public async Task<Voucher> CreateVoucherAsync(Voucher v)
        {
           _context.Vouchers.Add(v);
            await _context.SaveChangesAsync();
            return v;
        }

        public async Task<bool> CheckVoucherByCodeAsync(string code)
        {
            return await _context.Vouchers.IgnoreQueryFilters().AnyAsync(x => x.Code == code);   
        }

        public async Task<List<Voucher>> GetPageVouchersAsync(int pageNumber,int pageSize)
        {
            return await _context.Vouchers
           
                .Skip((pageNumber-1)*pageSize).Take(pageSize).AsNoTracking().ToListAsync();
        }

        public async Task<Voucher?> GetVoucherByIdAsync(string id)
        {
            return await _context.Vouchers.IgnoreQueryFilters()
                .Include(v=>v.Targets)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<DetailVoucherResponse?> GetDetailVoucherAsync(string id)
        {
          
            var voucherWithTargetId =   await _context.Vouchers.IgnoreQueryFilters()
                .Include(v => v.Targets)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (voucherWithTargetId == null) return null;

            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(voucherWithTargetId.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(voucherWithTargetId.UpdatedAt, vietnamTimeZone);
            var result = new DetailVoucherResponse { 
            Id = voucherWithTargetId.Id,
            ApplyTarget =voucherWithTargetId.Type,
            Code = voucherWithTargetId.Code,
            CreatedAt = createdAtVN,
            DiscountValue = voucherWithTargetId.DiscountValue,
            EndDate = voucherWithTargetId.EndDate,
            IsActive = voucherWithTargetId.IsActive,
            IsDeleted = voucherWithTargetId.IsDeleted,
            MaxDiscountValue = voucherWithTargetId.MaxDiscountValue,
            MaxUses = voucherWithTargetId.UsageCount,
            MinOrderValue = voucherWithTargetId.MinOrderValue,
            Name = voucherWithTargetId.Name,
            StartDate = voucherWithTargetId.StartDate,
            UpdatedAt = updatedAtVN,

            };
            var targetIds = voucherWithTargetId.Targets
        .Select(t => t.TargetId)
        .ToList();

            List<VoucherItemTargetResponse> itemTargets = new List<VoucherItemTargetResponse>();
            if (voucherWithTargetId.Type == VoucherTargetConstants.ProductTarget && voucherWithTargetId.Targets.Any())
            {
                var products = await _context.Books
             .Where(b => targetIds.Contains(b.Id)) 
             .Select(b => new VoucherItemTargetResponse
             {
                 Id = b.Id,
                 title = b.Title 
             })
             .ToListAsync();

                itemTargets.AddRange(products);
            }
            else if (voucherWithTargetId.Type == VoucherTargetConstants.CategoryTarget && voucherWithTargetId.Targets.Any()) {
                var categories = await _context.Categories
                .Where(c => targetIds.Contains(c.Id))
                .Select(c => new VoucherItemTargetResponse
                {
                    Id = c.Id,
                    title = c.CategoryName 
                })
                .ToListAsync();

                itemTargets.AddRange(categories);

            }
            result.TargetDetail = itemTargets;

            return result;

        }
        public async Task<Voucher> UpdateVoucherAsync(Voucher v)
        {
            _context.Vouchers.Update(v);
            await _context.SaveChangesAsync();
            return v;
        }

        public async Task<List<Voucher>> FilterVoucherAsync(FilterVoucherRequest request)
        {
            var query = _context.Vouchers.AsNoTracking().AsQueryable();
            if(!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                query = query.Where(v => v.Name.ToLower().Contains(term));
            }
            return await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        }
    
        public async Task<List<string>> GetNonExistingProductIdsAsync(IEnumerable<string> productIds)
        {
          
            var existingIds = await _context.Books
                .IgnoreQueryFilters()
                .Where(p => productIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

          
            var nonExistingIds = productIds.Except(existingIds, StringComparer.OrdinalIgnoreCase).ToList();
            return nonExistingIds;
        }
        public async Task<List<string>> GetNonExistingCategoryIdsAsync(IEnumerable<string> categoryIds)
        {

            var existingIds = await _context.Categories
                .IgnoreQueryFilters()
                .Where(p => categoryIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();


            var nonExistingIds = categoryIds.Except(existingIds, StringComparer.OrdinalIgnoreCase).ToList();
            return nonExistingIds;
        }

        public async Task<Voucher?> GetVoucherByCode(string code)
        {
            return await _context.Vouchers.AsNoTracking().Include(v => v.Targets).FirstOrDefaultAsync(v => v.Code == code);
        }

        public async Task<List<SuggestionResponse>> GetSuggestionsAsync(FilterVoucherRequest request)
        {
            var query = _context.Vouchers.AsNoTracking().AsQueryable();
            query = query.Take(10);
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                string term = request.SearchTerm.ToLower();
                query = query.Where(au => au.Name.ToLower().Contains(term) || au.Code.ToLower().Contains(term));
            }
            return await query
         .Select(au => new SuggestionResponse
         {
             Id = au.Id,
             Title = au.Name
         })
         .Take(10)
         .ToListAsync();
        }
    }
}
