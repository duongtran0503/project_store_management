using Microsoft.AspNetCore.Mvc;
using StoreManagement.API.Modules.Suppliers.Services;

namespace StoreManagement.API.Modules.Suppliers.Controllers
{
    [ApiController]
    [Route("/api/suppliers")]
    public class SupplierController
    {
        private readonly SupplierService _supplierService;
        public SupplierController(SupplierService supplierService) { 
         _supplierService = supplierService;
        }

    }
}
