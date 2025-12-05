using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Authentication.Services;
using StoreManagement.API.Modules.Inventories.Services;
using StoreManagement.API.Modules.Orders.Constants;
using StoreManagement.API.Modules.Orders.Dtos.Requests;
using StoreManagement.API.Modules.Orders.Dtos.Responses;
using StoreManagement.API.Modules.Orders.ErrorCode;
using StoreManagement.API.Modules.Orders.Repository;
using TimeZoneConverter;

namespace StoreManagement.API.Modules.Orders.Services
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _invoiceRepository;
        private readonly AuthTokenService _authTokenService;
        private readonly InventoryService _inventoryService;
        public InvoiceService(InvoiceRepository invoiceRepository,AuthTokenService authTokenService,
            InventoryService inventoryService
            )
        {
            _invoiceRepository = invoiceRepository;
            _authTokenService = authTokenService;
            _inventoryService = inventoryService;   
        }
        public async Task<InvoiceDetailResponse> CreateInvoicePOS(CreateInvoiceRequest request)
        {
            return await CreateInvoice(request,InvoiceOrderTypeConstant.POS);

        }

        public async Task<InvoiceDetailResponse> CreateInvoiceOnline(CreateInvoiceRequest request)
        {

            return await CreateInvoice(request, InvoiceOrderTypeConstant.ONLINE);
          
        }

        public async Task<PaginationResponse<InvoiceResponse>> GetPageInvoices(PaginationRequest request)
        {
            var invoiceEntities =await _invoiceRepository.GetPageInvoices(request.PageNumber, request.PageSize);
            var invoices = invoiceEntities.Select(i => MapToInvoiceResponse(i)).ToList();
            return new PaginationResponse<InvoiceResponse>(invoices, invoices.Count, request.PageNumber, request.PageSize);
        }

        //================================================================
        //== internal function support                                    ==
        //================================================================
        private async Task<InvoiceDetailResponse> CreateInvoice(CreateInvoiceRequest request, string OrderType)
        {

          

            if (!request.Details.Any())
            {
                throw new AppException(InvoiceErrorCode.InvoiceDetailNotNull);
            }
            var listPriceBooksOrder = await _invoiceRepository.GetPriceBooksOrderAsync(request.Details.Select(d => d.BookId).ToList());
            var stockReductions = request.Details.ToDictionary(d => d.BookId, d => d.Quantity);
            await _inventoryService.CheckStockAvailability(stockReductions);
            decimal subtotal = request.Details.Sum(d => d.Quantity * listPriceBooksOrder[d.BookId]);
            decimal totalDiscount = request.Details.Sum(d => d.TotalDiscount);
            decimal totalAmount = subtotal - totalDiscount;
            decimal finalAmount = totalAmount - request.DiscountAmount;
          
            var invoice = new Invoice
            {
               
                CustomerId = request.CustomerId,
                PaymentTime = DateTime.UtcNow,
                OrderType = request.OrderType,
                Subtotal = subtotal,
                VoucherId = request.VoucherId,
                TotalAmount = totalAmount,
                DiscountAmount = request.DiscountAmount,
                PaymentStatus = request.PaymentStatus,
                PaymentNote = request.PaymentNote,
                Status = request.Status,
                FinalAmount = finalAmount,
                AmountPaid = request.AmountPaid,
                PaymentMethod = request.PaymentMethod,
            };
            if (OrderType == InvoiceOrderTypeConstant.POS)
            {
                var cashierStaffId = _authTokenService.GetCurrentUserId();
                if (cashierStaffId == null)
                {

                    throw new AppException(InvoiceErrorCode.StaffInventoryInValid);
                }
                invoice.CashierStaffId = cashierStaffId;
            }
            if(OrderType == InvoiceOrderTypeConstant.ONLINE)
            {
                if(request.CustomerId==null)
                {
                    throw new AppException(InvoiceErrorCode.CustomerInventoryInValid);
                }
                invoice.CustomerId = request.CustomerId;
            }
          
            var invoiceDetails = request.Details.Select(detailRequest => new InvoiceDetail
            {
                BookId = detailRequest.BookId,
                Quantity = detailRequest.Quantity,
                VoucherId = detailRequest.VoucherId,
                UnitPrice = listPriceBooksOrder[detailRequest.BookId],
                TotalDiscount = detailRequest.TotalDiscount,
            }).ToList();


            var createdInvoice = await _invoiceRepository.CreateInvoiceAsync(
                invoice,
                invoiceDetails,
                stockReductions
            );


            createdInvoice.InvoiceDetails = invoiceDetails;
            return await MapToInvoiceResponseAsync(createdInvoice);

        }

        private InvoiceResponse MapToInvoiceResponse(Invoice invoice)
        {
            decimal changeDue = invoice.AmountPaid > invoice.FinalAmount
                                ? invoice.AmountPaid - invoice.FinalAmount
                                : 0;

            decimal totalDiscountCalculated = invoice.DiscountAmount +
                                              invoice.InvoiceDetails.Sum(d => d.TotalDiscount);
            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(invoice.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(invoice.UpdatedAt, vietnamTimeZone);
          
            var resuslt = new InvoiceResponse
            {
                Id = invoice.Id,
                CreatedAt = createdAtVN,
                UpdatedAt = updatedAtVN,
                OrderType = invoice.OrderType,
                Subtotal = invoice.Subtotal,
                TotalDiscount = totalDiscountCalculated,
                FinalAmount = invoice.FinalAmount,
                Status = invoice.Status,
                PaymentMethod = invoice.PaymentMethod,
                PaymentStatus = invoice.PaymentStatus,
                PaymentTime = invoice.PaymentTime,
                AmountPaid = invoice.AmountPaid,
                ChangeDue = changeDue,
                PaymentNote = invoice.PaymentNote,
                Details = invoice.InvoiceDetails.Select(d =>
                {

                    return new SummaryInvoiceDetailDTO
                    {
                        Id = d.Id,
                        BookId = d.BookId,
                        BookTitle = d.Book.Title,
                        Quantity = d.Quantity
                    };
                }).ToList()
            };
            if (invoice.OrderType == InvoiceOrderTypeConstant.POS && invoice.CashierStaffId != null)
            {
                var staff = invoice.CashierStaff;
                if (staff != null)
                {
                    resuslt.Staff = new StaffDTO
                    {
                        Id = staff.Id,
                        FullName = staff.PositionName,
                    };
                }

            }
            else if (invoice.OrderType == InvoiceOrderTypeConstant.ONLINE && invoice.CustomerId != null)
            {
                var customer = invoice.Customer;
                if (customer != null)
                {
                    resuslt.Customer = new CustomerDTO
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Address = customer.Address,
                        Phone = customer.Phone
                    };
                }
            }

            return resuslt;
        }

        private async Task<InvoiceDetailResponse> MapToInvoiceResponseAsync(Invoice invoice)
        {
            decimal changeDue = invoice.AmountPaid > invoice.FinalAmount
                                ? invoice.AmountPaid - invoice.FinalAmount
                                : 0;

            decimal totalDiscountCalculated = invoice.DiscountAmount +
                                              invoice.InvoiceDetails.Sum(d => d.TotalDiscount);
            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(invoice.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(invoice.UpdatedAt, vietnamTimeZone);
            var listInfoBookOrder = await _invoiceRepository.GetListinfoBookOrder(invoice.InvoiceDetails.Select(d => d.BookId).ToList());
            var resuslt = new InvoiceDetailResponse
            {
                Id = invoice.Id,
                CreatedAt = createdAtVN,
                UpdatedAt = updatedAtVN,
                OrderType = invoice.OrderType,
                Subtotal = invoice.Subtotal,
                TotalDiscount = totalDiscountCalculated,
                FinalAmount = invoice.FinalAmount,
                PaymentMethod = invoice.PaymentMethod,
                PaymentStatus = invoice.PaymentStatus,
                PaymentTime = invoice.PaymentTime,
                Status = invoice.Status,
                AmountPaid = invoice.AmountPaid,
                ChangeDue = changeDue,
                PaymentNote = invoice.PaymentNote,
                Details  = invoice.InvoiceDetails.Select(d =>
                {
                    d.Book = listInfoBookOrder[d.BookId] ?? default!;
                    return MapToInvoiceDetailDTO(d);
                }).ToList()
            };
            if(invoice.OrderType==InvoiceOrderTypeConstant.POS && invoice.CashierStaffId!=null)
            {
                var staff =invoice.CashierStaff!=null ?invoice.CashierStaff: await _invoiceRepository.GetInfoStaff(invoice.CashierStaffId);
                if(staff!=null)
                {
                    resuslt.Staff = new StaffDTO
                    {
                        Id = staff.Id,
                        FullName = staff.PositionName,
                    };
                }

            } else if(invoice.OrderType==InvoiceOrderTypeConstant.ONLINE && invoice.CustomerId!=null)
            {
                var customer = invoice.Customer!=null ?invoice.Customer: await _invoiceRepository.GetInfoCustomerOrder(invoice.CustomerId);
                 if(customer!=null)
                {
                    resuslt.Customer = new CustomerDTO
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Address =customer.Address,
                        Phone =customer.Phone
                    };
                }
            }

                return resuslt;
        }
        private  InvoiceDetailDTO MapToInvoiceDetailDTO(InvoiceDetail detail)
        {
           
            decimal itemSubtotal = detail.Quantity * detail.UnitPrice;
            decimal finalItemAmount = itemSubtotal - detail.TotalDiscount;

          
            var book = detail.Book;

            return new InvoiceDetailDTO
            {
                Id = detail.Id,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice,
                TotalDiscount = detail.TotalDiscount,

             
                ItemSubtotal = itemSubtotal,
                FinalItemAmount = finalItemAmount,

             
                BookId = detail.BookId,
                BookImage = book?.Image ?? "default_image_url",
                BookTitle = book?.Title ?? "N/A",

              
                Voucher = detail.Voucher != null ? new VoucherDTO
                {
                   VoucherCode = detail.Voucher.Code,   
                   DiscountValue =detail.Voucher.DiscountValue,
                   VoucherName = detail.Voucher.Name,
                   
                } : null
            };
        }
    }
}
