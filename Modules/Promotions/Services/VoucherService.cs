using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Promotions.Constants;
using StoreManagement.API.Modules.Promotions.Dtos.Requests;
using StoreManagement.API.Modules.Promotions.Dtos.Responses;
using StoreManagement.API.Modules.Promotions.ErrrorCode;
using StoreManagement.API.Modules.Promotions.Repository;
using TimeZoneConverter;

namespace StoreManagement.API.Modules.Promotions.Services
{
    public class VoucherService
    {
        private readonly VoucherRepository _vourcherRepository;

        public VoucherService(VoucherRepository vocherRepository)
        {
            _vourcherRepository = vocherRepository;
        }
       
        public async Task<VoucherResponse> CreateVoucher(CreateVoucherRequest request)
        {
            var checkCode = await _vourcherRepository.CheckVoucherByCodeAsync(request.Code);
            if (checkCode) throw new AppException(VoucherErrorCode.VourcherExisted);

            var newVoucher = new Voucher
            {
                Type = request.Type,
                Code = request.Code,
                DiscountValue = request.DiscountValue,
                EndDate = request.EndDate,
                IsActive = request.IsActive,
                MaxDiscountValue = request.MaxDiscountValue,
                UsageCount = request.MaxUses,
                MinOrderValue = request.MinOrderValue,
                Name = request.Name,
                StartDate = request.StartDate,
            };

            if ( request.TargetIds.Any())
            {

                await CheckTargetIds(request.TargetIds, request.Type);

                var newTargets = request.TargetIds.Where(t => !string.IsNullOrEmpty(t)).Select(t => new VoucherTarget
                {
                    TargetId = t,
                    TargetType = request.Type
                }).ToList();
                newVoucher.Targets = newTargets;    
            }
            var res = await _vourcherRepository.CreateVoucherAsync(newVoucher);
            return ToVourcherResponse(res);
           
        }

        public async Task<VoucherResponse> UpdateVourcher(UpdateVoucherRequest request,string id)
        {
            var voucher = await _vourcherRepository.GetVoucherByIdAsync(id);
            if (voucher == null) throw new AppException(VoucherErrorCode.VourcherNotExisted);

            if(voucher.Code != request.Code)
            {
                var checkCode = await _vourcherRepository.CheckVoucherByCodeAsync(request.Code);
                if (checkCode) throw new AppException(VoucherErrorCode.VoucherCodeExisted);
            }
            voucher.StartDate = request.StartDate;
            voucher.EndDate = request.EndDate;
            voucher.Name = request.Name;
            voucher.Code = request.Code;
            voucher.IsActive = request.IsActive;
            voucher.DiscountValue = request.DiscountValue;
            voucher.MaxDiscountValue = request.MaxDiscountValue;
            voucher.MinOrderValue = request.MinOrderValue;
            voucher.Type = request.Type;
            if(request.TargetIds.Any())
            {
                await CheckTargetIds(request.TargetIds, request.Type);
            }
            var newTargets = request.TargetIds.Where(t =>!string.IsNullOrEmpty(t)).Select(t => new VoucherTarget
            {
                TargetId = t,
                TargetType = request.Type
            }).ToList();
            voucher.Targets = newTargets;
            var newVoucher = await _vourcherRepository.UpdateVoucherAsync(voucher);
            return ToVourcherResponse(newVoucher);
            

        }

        public async Task<PaginationResponse<VoucherResponse>> GetPageVouchers(PaginationRequest request)
        {
            var voucherEntities = await _vourcherRepository.GetPageVouchersAsync(request.PageNumber, request.PageSize);
            var vouchers = voucherEntities.Select(v=>ToVourcherResponse(v)).ToList();
            return new PaginationResponse<VoucherResponse>(vouchers, vouchers.Count, request.PageNumber, request.PageSize);
        }

        public async Task<VoucherResponse> GetVoucherByCode(string code)
        {
            var voucher = await _vourcherRepository.GetVoucherByCode(code);
            if (voucher == null) throw new AppException(VoucherErrorCode.VourcherNotExisted);
            return ToVourcherResponse(voucher);
        }

        public async Task<DeletedResponse> DeleteVoucher(string id)
        {
            var voucher = await _vourcherRepository.GetVoucherByIdAsync(id);
            if (voucher == null) throw new AppException(VoucherErrorCode.VourcherNotExisted);
            voucher.IsDeleted = true;
            if (voucher.Targets.Any())
            {
                foreach (var voucherTarget in voucher.Targets)
                {
                    voucherTarget.IsDeleted = true;
                }
            }
            await _vourcherRepository.UpdateVoucherAsync(voucher);
            return new DeletedResponse { Name = voucher.Name };
        }

        public async Task<VoucherResponse> RestoreVoucher(string id)
        {
            var voucher = await _vourcherRepository.GetVoucherByIdAsync(id);
            if (voucher == null) throw new AppException(VoucherErrorCode.VourcherNotExisted);
            voucher.IsDeleted = false;
            if(voucher.Targets.Any())
            {
                foreach(var voucherTarget in voucher.Targets)
                {
                    voucherTarget.IsDeleted = false;
                }
            }
            await _vourcherRepository.UpdateVoucherAsync(voucher);
            return ToVourcherResponse(voucher);

        }

        public async Task<PaginationResponse<VoucherResponse>> FilterVoucher(FilterVoucherRequest request)
        {
            var voucherEntities = await _vourcherRepository.FilterVoucherAsync(request);
            var vouchers = voucherEntities.Select(v => ToVourcherResponse(v)).ToList();
            return new PaginationResponse<VoucherResponse>(vouchers, vouchers.Count, request.PageNumber, request.PageSize);
        }
        private async Task CheckTargetIds(List<string> ids,string type)
        {
            if (type == VoucherTargetConstants.ProductTarget)
            {
                var nonExistingId = await _vourcherRepository.GetNonExistingProductIdsAsync(ids);
                if (nonExistingId.Any())
                {

                    throw new AppException(VoucherErrorCode.InvalidIdTargeted);
                }
            }
            if (type == VoucherTargetConstants.CategoryTarget)
            {
                var nonExistingId = await _vourcherRepository.GetNonExistingCategoryIdsAsync(ids);
                if (nonExistingId.Any())
                {

                    throw new AppException(VoucherErrorCode.InvalidIdTargeted);
                }
            }
        }
        public async Task<List<SuggestionResponse>> GetSuggestons(FilterVoucherRequest request)
        {
            return await _vourcherRepository.GetSuggestionsAsync(request);
                
        }

        public  async Task<DetailVoucherResponse> GetVoucherDetail(string id)
        {
             var voucher  = await _vourcherRepository.GetDetailVoucherAsync(id);
            if(voucher == null) throw new AppException(VoucherErrorCode.VourcherNotExisted);
            return voucher;
        }

        private VoucherResponse ToVourcherResponse(Voucher voucher) {
            var targetEntities = voucher.Targets;
            var targetIds = targetEntities.Select(x => new VoucherItemTargetResponse
            {
                Id=x.Id,
             
            }).ToList();
            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(voucher.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(voucher.UpdatedAt, vietnamTimeZone);

            return new VoucherResponse
            {

                Id = voucher.Id,
                ApplyTarget = voucher.Type,
                Code = voucher.Code,
                CreatedAt = updatedAtVN,
                DiscountValue = voucher.DiscountValue,
                EndDate = voucher.EndDate,
                IsActive = voucher.IsActive,
                IsDeleted = voucher.IsDeleted,
                MaxDiscountValue = voucher.MaxDiscountValue,
                MaxUses = voucher.UsageCount,
                MinOrderValue = voucher.MinOrderValue,
                Name = voucher.Name,
                StartDate = voucher.StartDate,
                TargetIds =targetIds,
                UpdatedAt = createdAtVN,
        };
        }

       
    }
}
