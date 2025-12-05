using StoreManagement.API.Common.Entities;
using StoreManagement.API.Common.Exceptions;
using StoreManagement.API.Common.Responses;
using StoreManagement.API.Modules.Users.Dtos.Requests;
using StoreManagement.API.Modules.Users.Dtos.Response;
using StoreManagement.API.Modules.Users.ErrorCode;
using StoreManagement.API.Modules.Users.Repository;
using TimeZoneConverter;

namespace StoreManagement.API.Modules.Users.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerService(CustomerRepository customerRepository) { 
         _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request)
        {
            var customer = await _customerRepository.GetCustomerByPhone(request.Phone);
            if(customer==null)
            {
                var newCus = await _customerRepository.CreateCustomerAsync(new Customer
                {
                    Address = request.Address,
                    Name = request.Name,
                    Phone = request.Phone,
                });
                return ToCustomerResponse(newCus);
            } 
            return ToCustomerResponse(customer);
        }

        public async Task<CustomerResponse> GetCustomerByPhone(string id)
        {
            var  customer = await _customerRepository.GetCustomerById(id);
            if (customer == null) throw new AppException(CustomerErrorCode.CustomerNotExisted);
            return ToCustomerResponse(customer);    
        }

        public async Task<PaginationResponse<CustomerResponse>> GetPageCustomer(PaginationRequest request)
        {
            var customerEntities = await _customerRepository.GetPageCustomer(request.PageNumber, request.PageSize);
            var custoemrs = customerEntities.Select(c=>ToCustomerResponse(c)).ToList();
            return new PaginationResponse<CustomerResponse>(custoemrs, custoemrs.Count, request.PageNumber, request.PageSize);
        }

        private CustomerResponse ToCustomerResponse(Customer customer) {

            var vietnamTimeZone = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
            DateTime createdAtVN = TimeZoneInfo.ConvertTimeFromUtc(customer.CreatedAt, vietnamTimeZone);
            DateTime updatedAtVN = TimeZoneInfo.ConvertTimeFromUtc(customer.UpdatedAt, vietnamTimeZone);
            return new CustomerResponse
            {
                Id = customer.Id,
                Address = customer.Address,
                CreatedAt = createdAtVN,
                UpdatedAt = updatedAtVN,
                Name = customer.Name,
                Phone= customer.Phone,

            };
        }
    }
}
