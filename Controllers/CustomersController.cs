
using Microsoft.AspNetCore.Mvc;
using CustomerBeneficiaryManagement.Models;
using CustomerBeneficiaryManagement.Services;
using CustomerBeneficiaryManagement.Logging;

namespace CustomerBeneficiaryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly ApiLogger _apiLogger;

        public CustomersController(CustomerService customerService, ApiLogger apiLogger)
        {
            _customerService = customerService;
            _apiLogger = apiLogger;
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            await _apiLogger.LogAsync("POST /api/customers - Request received");

            try
            {
                int customerID = await _customerService.AddCustomerAsync(customer);

                await _apiLogger.LogAsync($"Customer created successfully. CustomerID: {customerID}");

                return CreatedAtAction(
                    nameof(GetCustomerById),
                    new { id = customerID },
                    new
                    {
                        message = "Customer created successfully.",
                        customerID
                    });
            }
            catch (ArgumentException ex)
            {
                await _apiLogger.LogAsync($"Customer creation failed: {ex.Message}", "WARN");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            await _apiLogger.LogAsync($"PUT /api/customers/{id} - Request received");

            try
            {
                customer.CustomerID = id;

                bool updated = await _customerService.UpdateCustomerAsync(customer);

                if (!updated)
                {
                    await _apiLogger.LogAsync($"Customer not found for update. CustomerID: {id}", "WARN");
                    return NotFound(new { message = "Customer not found." });
                }

                await _apiLogger.LogAsync($"Customer updated successfully. CustomerID: {id}");

                return Ok(new { message = "Customer updated successfully." });
            }
            catch (ArgumentException ex)
            {
                await _apiLogger.LogAsync($"Customer update failed. CustomerID: {id}. Error: {ex.Message}", "WARN");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            await _apiLogger.LogAsync($"GET /api/customers/{id} - Request received");

            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);

                if (customer == null)
                {
                    await _apiLogger.LogAsync($"Customer not found. CustomerID: {id}", "WARN");
                    return NotFound(new { message = "Customer not found." });
                }

                await _apiLogger.LogAsync($"Customer retrieved successfully. CustomerID: {id}");

                return Ok(customer);
            }
            catch (ArgumentException ex)
            {
                await _apiLogger.LogAsync($"Customer retrieval failed. CustomerID: {id}. Error: {ex.Message}", "WARN");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            await _apiLogger.LogAsync("GET /api/customers - Request received");

            var customers = await _customerService.GetAllCustomersAsync();

            await _apiLogger.LogAsync("Customer list retrieved successfully");

            return Ok(customers);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateCustomerStatus(int id, [FromBody] bool status)
        {
            await _apiLogger.LogAsync($"PATCH /api/customers/{id}/status - Request received");

            try
            {
                bool updated = await _customerService.UpdateCustomerStatusAsync(id, status);

                if (!updated)
                {
                    await _apiLogger.LogAsync($"Customer not found for status update. CustomerID: {id}", "WARN");
                    return NotFound(new { message = "Customer not found." });
                }

                string message = status
                    ? "Customer activated successfully."
                    : "Customer deactivated successfully.";

                await _apiLogger.LogAsync($"Customer status updated. CustomerID: {id}, Status: {status}");

                return Ok(new { message });
            }
            catch (ArgumentException ex)
            {
                await _apiLogger.LogAsync($"Customer status update failed. CustomerID: {id}. Error: {ex.Message}", "WARN");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
