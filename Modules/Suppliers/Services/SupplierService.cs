using StoreManagement.API.Modules.Suppliers.Repository;

namespace StoreManagement.API.Modules.Suppliers.Services
{
    public class SupplierService
    {
        private readonly SupplierRepository _supplierRepository;
        public SupplierService(SupplierRepository supplierRepository) { 
          _supplierRepository = supplierRepository;
        }
    }
}
