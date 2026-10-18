using CustomerBeneficiaryManagement.DataAccess;
using CustomerBeneficiaryManagement.Models;

namespace CustomerBeneficiaryManagement.Services
{
    public class CustomerService
    {
        private readonly CustomerRepository _repository;

        public CustomerService(CustomerRepository repository)
        {
            _repository = repository;
        }

        // 1. Add Customer
        public async Task<int> AddCustomerAsync(Customer customer)
        {
            ValidateCustomer(customer);

            return await _repository.AddCustomerAsync(customer);
        }

        // 2. Update Customer
        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            if (customer.CustomerID <= 0)
                throw new ArgumentException("Invalid Customer ID.");

            ValidateCustomer(customer);

            return await _repository.UpdateCustomerAsync(customer);
        }

        // 3. Get Customer by ID
        public async Task<Customer?> GetCustomerByIdAsync(int customerID)
        {
            if (customerID <= 0)
                throw new ArgumentException("Invalid Customer ID.");

            return await _repository.GetCustomerByIdAsync(customerID);
        }

        // 4. Get All Customers
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _repository.GetAllCustomersAsync();
        }

        // 5. Activate / Deactivate Customer
        public async Task<bool> UpdateCustomerStatusAsync(
            int customerID, bool status)
        {
            if (customerID <= 0)
                throw new ArgumentException("Invalid Customer ID.");

            return await _repository.UpdateCustomerStatusAsync(
                customerID, status);
        }

        // Basic business validation
        private static void ValidateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FullName))
                throw new ArgumentException("Full name is required.");

            if (string.IsNullOrWhiteSpace(customer.Nationality))
                throw new ArgumentException("Nationality is required.");

            if (string.IsNullOrWhiteSpace(customer.MobileNo))
                throw new ArgumentException("Mobile number is required.");

            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Email is required.");

            if (customer.IDTypeID <= 0)
                throw new ArgumentException("A valid ID type is required.");

            if (string.IsNullOrWhiteSpace(customer.IDNo))
                throw new ArgumentException("ID number is required.");
        }
    }

}