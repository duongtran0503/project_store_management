using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Authentication.Services;
using StoreManagement.API.Modules.Inventories.Constants;
using StoreManagement.API.Modules.Inventories.Dtos.Requests;
using StoreManagement.API.Modules.Inventories.Dtos.Response;
using StoreManagement.API.Modules.Inventories.ErrorCode;
using StoreManagement.API.Modules.Inventories.Repository;
using TimeZoneConverter;

namespace StoreManagement.API.Modules.Inventories.Services
{
    public class InventoryService
    {
        private readonly InventoryRepository _inventoryRepository;
        private readonly AuthTokenService _authTokenService;
        public InventoryService(InventoryRepository inventoryRepository
            ,AuthTokenService authTokenService
            ) { 
         _inventoryRepository = inventoryRepository;
            _authTokenService = authTokenService;
        }

        public async Task<UpdateGRNStatusResponse> UpdateGRNStatusReceipt(string id,UpdateGRNStatusRequest request)
        {

            var oldReceipt = await _inventoryRepository.GetInventoryReceiptWithDetailsAsync(id);

            if (oldReceipt == null)
            {
                throw new AppException(InventoryErrorCode.ReceiptNotFound);
            }
            if(oldReceipt.GRNStatus==GRNStatusConstant.COMPLETED || oldReceipt.GRNStatus==GRNStatusConstant.CANCELLED
                
                )
            {
                throw new AppException(InventoryErrorCode.ReceiptNotFound);
            } 

            oldReceipt.GRNStatus = request.Status;
            if(request.Status ==GRNStatusConstant.COMPLETED)
            {
                oldReceipt.ReceiptDate = DateTime.UtcNow;
            }
           var newReceipt =  await _inventoryRepository.UpdateGRNStatusAsync(oldReceipt);
            return new UpdateGRNStatusResponse { Id = newReceipt.Id ,Status = newReceipt.GRNStatus};

        }

       


        public async Task<PaginationResponse<InventoryReceiptItemResponse>> GetPageReceiptHistory(PaginationRequest request)
        {
            var list = await _inventoryRepository.GetPagedReceiptDetails(request.PageNumber,request.PageSize);
            return new PaginationResponse<InventoryReceiptItemResponse>(list,list.Count,request.PageNumber, request.PageSize);
        }


        public async Task<PaginationResponse<InventoryResponse>> GetPageInventory(PaginationRequest request)
        {
            var list = await _inventoryRepository.GetpageInventory(request.PageNumber, request.PageSize);
            return new PaginationResponse<InventoryResponse>(list, list.Count, request.PageNumber, request.PageSize);
        }


        public async Task<InventoryReceiptResponse> CreateInventoryReceipt(CreateInventoryReceiptRequest request)
        {

            var staffId = _authTokenService.GetCurrentUserId();
            if (staffId == null) throw new AppException(InventoryErrorCode.StaffInventoryInValid);
            if (!request.Details.Any())
            {
                throw new AppException(InventoryErrorCode.InventoryDetailNotNull);
            }

          
            decimal totalCost = request.Details.Sum(d => d.QuantityReceived * d.UnitCost);

            var supplier = await _inventoryRepository.GetSupplierByIdAsync(request.SupplierId);
            if(supplier == null)  throw new AppException(InventoryErrorCode.InvaliedSupplier); 

            var receipt = new InventoryReceipt
            {
                ReceiptDate = DateTime.UtcNow,
                ReceivingStaffId = staffId,
                
                SupplierId = supplier.Id,
                TotalCost = totalCost,
                GRNStatus = GRNStatusConstant.DRAFT,
            };

            var receiptDetails = new List<ReceiptDetail>();
          
            var stockUpdates = new Dictionary<string, int>();

            foreach (var detailRequest in request.Details)
            {
                var detail = new ReceiptDetail
                {
                   
                    BookId = detailRequest.BookId,
                    QuantityReceived = detailRequest.QuantityReceived,
                    UnitCost = detailRequest.UnitCost,
                    TotalLineCost = detailRequest.QuantityReceived * detailRequest.UnitCost,
                };
                receiptDetails.Add(detail);

              
                stockUpdates.Add(detailRequest.BookId, detailRequest.QuantityReceived);
            }

           
            var createdReceipt = await _inventoryRepository.CreateInventoryReceiptAsync(
                receipt,
                receiptDetails,
                stockUpdates
            );
            createdReceipt.ReceiptDetails = receiptDetails;

            return MapToResponseDTO(createdReceipt);
        }
        public async Task<InventoryReceiptResponse> UpdateInventoryReceipt(string receiptId, UpdateInventoryReceiptRequest request)
        {
           

           
            var oldReceipt = await _inventoryRepository.GetInventoryReceiptWithDetailsAsync(receiptId);

            if (oldReceipt == null)
            {
                throw new AppException(InventoryErrorCode.ReceiptNotFound);
            }

            if(oldReceipt.GRNStatus!= GRNStatusConstant.DRAFT)
            {
                throw new AppException(InventoryErrorCode.NOTALLOWEDIT);
            }

          

           
            var staffId = _authTokenService.GetCurrentUserId();
          

            if (!request.Details.Any())
            {
                throw new AppException(InventoryErrorCode.InventoryDetailNotNull);
            }

       
            decimal newTotalCost = request.Details.Sum(d => d.QuantityReceived * d.UnitCost);

           
            var undoStockUpdates = oldReceipt.ReceiptDetails
                .ToDictionary(d => d.BookId, d => d.QuantityReceived * -1);

          
            var newStockUpdates = request.Details
                .ToDictionary(d => d.BookId, d => d.QuantityReceived);

           
            var newReceiptDetails = request.Details.Select(detailRequest => new ReceiptDetail
            {
                
                BookId = detailRequest.BookId,
                QuantityReceived = detailRequest.QuantityReceived,
                UnitCost = detailRequest.UnitCost,
                TotalLineCost = detailRequest.QuantityReceived * detailRequest.UnitCost,
            }).ToList();

          
            oldReceipt.SupplierId = request.SupplierId;
            oldReceipt.TotalCost = newTotalCost;
            

         
            var updatedReceipt = await _inventoryRepository.UpdateInventoryReceiptAsync(
                oldReceipt,
                newReceiptDetails,
                undoStockUpdates,
                newStockUpdates
            );

          
            updatedReceipt.ReceiptDetails = newReceiptDetails; 
            return MapToResponseDTO(updatedReceipt);
        }

        public async Task<PaginationResponse<InventoryReceiptResponse>> GetPageInventoryReceipt(PaginationRequest request)
        {
            var inventoryReceipt = await _inventoryRepository.GetPageIventoryReceiptAsync(request.PageNumber, request.PageSize);
            var list = inventoryReceipt.Select(i => MapToResponseDTO(i)).ToList();
            return new PaginationResponse<InventoryReceiptResponse>(list, list.Count, request.PageNumber, request.PageSize);
        }

        public async Task CheckStockAvailability(Dictionary<string, int> request) {
            await _inventoryRepository.CheckStockAvailabilityAsync(request);
        }

        public async Task<InventoryReceiptResponse> GetInventoryReceiptById(string id)
        {
            var inventoryReceipt = await _inventoryRepository.GetInventoryReceiptWithDetailsAsync(id);
            if (inventoryReceipt == null) throw new AppException(InventoryErrorCode.InventoryReceiptNotExisted);
            return MapToResponseDTO(inventoryReceipt);  
        }

        private InventoryReceiptResponse MapToResponseDTO(InventoryReceipt receipt)
        {
           
            if (receipt.ReceiptDetails == null)
            {
             
                receipt.ReceiptDetails = new List<ReceiptDetail>();
            }
            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(receipt.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(receipt.UpdatedAt, vietnamTimeZone);
            return new InventoryReceiptResponse
            {
                Id = receipt.Id,
                ReceiptDate = receipt.ReceiptDate,
                TotalCost = receipt.TotalCost,
                SupplierId = receipt.SupplierId,
                ReceivingStaffId = receipt.ReceivingStaffId,
                ReceivingStaffName =receipt.ReceivingStaff.PositionName,
                SupplierName = receipt.Supplier.SupplierName,
                GRNStatus = receipt.GRNStatus,
                CreatedAt = createdAtVN,
                UpdatedAt = updatedAtVN,
                Details = receipt.ReceiptDetails.Select(detail => new ReceiptDetailResponse
                {
                    BookId = detail.BookId,
                    QuantityReceived = detail.QuantityReceived,
                    UnitCost = detail.UnitCost,
                    TotalLineCost = detail.TotalLineCost,
                    BookImage = detail.Book.Image,
                    BookName =detail.Book.Title,
                }).ToList()
            };
        }
    }
}
