using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Modules.Suppliers.Dtos.Requests;
using StoreManagement.API.Modules.Suppliers.Dtos.Responses;
using StoreManagement.API.Modules.Suppliers.ErrorCode;
using StoreManagement.API.Modules.Suppliers.Repository;
using System.Security.Cryptography.Xml;

namespace StoreManagement.API.Modules.Suppliers.Services
{
    public class SupplierService
    {
        private readonly SupplierRepository _supplierRepository;
        public SupplierService(SupplierRepository supplierRepository) { 
          _supplierRepository = supplierRepository;
        }

        public async Task<SupplierResponse> CreateSupplier(CreateSupplierRequest request)
        {
            var check =await _supplierRepository.CheckSupplierByPhone(request.Phone);
            if (check) throw new AppException(SupplierErrorCode.SupplierExsisted);

            var supplier = await _supplierRepository.CreateSupplierAsync(new Supplier
            {
                Address = request.Address,
                ContactPerson = request.ContactPerson,
                Phone = request.Phone,
                SupplierName = request.SupplierName,
            });
            return ToSupplierResponse(supplier);
        }

        public async Task<PaginationResponse<SupplierResponse>> GetSuppliers(PaginationRequest request)
        {
            var (supplierEntities, total) = await _supplierRepository.GetPageSupplierAsync(request.PageSize, request.PageNumber);

            var suppliers = supplierEntities.Select(s=>ToSupplierResponse(s)).ToList();
            return new PaginationResponse<SupplierResponse>(suppliers,total,request.PageNumber,request.PageSize);
        }

        public async Task<SupplierResponse> UpdateSupplier(UpdateSupplierRequest request, string id)
        {
            var supplier = await _supplierRepository.GetSupplierByIdAsync(id);
            if (supplier == null) throw new AppException(SupplierErrorCode.SupplierNotExsisted);
            if(supplier.Phone !=request.Phone)
            {
                var checkPhone = await _supplierRepository.CheckSupplierByPhone(request.Phone);
                if (checkPhone) throw new AppException(SupplierErrorCode.SupplierPhoneExsisted);
            }
            supplier.Phone = request.Phone;
            supplier.ContactPerson = request.ContactPerson;
            supplier.SupplierName = request.SupplierName;
            supplier.Address = request.Address;
            var update = await _supplierRepository.UpdateSupplierAsync(supplier);
            return ToSupplierResponse(supplier);
        }

        public async Task<PaginationResponse<SupplierResponse>> SearchSupplier(SearchSupplierRequest request)
        {
            var supplierEntities = await _supplierRepository.SearchSupplierAsync(request);
            var suppliers = supplierEntities.Select(s=>ToSupplierResponse(s)).ToList();
            var total = suppliers.Count();
            return new PaginationResponse<SupplierResponse>(suppliers, total,request.PageNumber,request.PageSize);
        }

        private SupplierResponse ToSupplierResponse(Supplier supplier) => new SupplierResponse
        {
            Address = supplier.Address,
            ContactPerson = supplier.ContactPerson,
            Phone = supplier.Phone,
            SupplierName = supplier.SupplierName,
            Id = supplier.Id,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt,
        };
    }

    
}
