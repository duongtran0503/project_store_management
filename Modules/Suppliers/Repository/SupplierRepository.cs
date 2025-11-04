using StoreManagement.API.Shared.Data;

namespace StoreManagement.API.Modules.Suppliers.Repository
{
    public class SupplierRepository
    {
        private readonly ApplicationDbContext _context;
        public SupplierRepository(ApplicationDbContext context) { _context = context; }

    }
}
