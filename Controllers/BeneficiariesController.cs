
using Microsoft.AspNetCore.Mvc;
using CustomerBeneficiaryManagement.Models;
using CustomerBeneficiaryManagement.Services;
using CustomerBeneficiaryManagement.Logging;

namespace CustomerBeneficiaryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeneficiariesController : ControllerBase
    {
        private readonly BeneficiaryService _beneficiaryService;
        private readonly ApiLogger _apiLogger;

        public BeneficiariesController(BeneficiaryService beneficiaryService, ApiLogger apiLogger)
        {
            _beneficiaryService = beneficiaryService;
            _apiLogger = apiLogger;
        }

        [HttpPost]
        public async Task<IActionResult> AddBeneficiary([FromBody] Beneficiary beneficiary)
        {
            await _apiLogger.LogAsync("POST /api/beneficiaries - Request received");

            try
            {
                int beneficiaryID = await _beneficiaryService.AddBeneficiaryAsync(beneficiary);

                await _apiLogger.LogAsync($"Beneficiary created successfully. BeneficiaryID: {beneficiaryID}");

                return StatusCode(201, new
                {
                    message = "Beneficiary created successfully.",
                    beneficiaryID
                });
            }
            catch (ArgumentException ex)
            {
                await _apiLogger.LogAsync($"Beneficiary creation failed: {ex.Message}", "WARN");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBeneficiary(int id, [FromBody] Beneficiary beneficiary)
        {
            await _apiLogger.LogAsync($"PUT /api/beneficiaries/{id} - Request received");

            try
            {
                beneficiary.BeneficiaryID = id;

                bool updated = await _beneficiaryService.UpdateBeneficiaryAsync(beneficiary);

                if (!updated)
                {
                    await _apiLogger.LogAsync($"Beneficiary not found for update. BeneficiaryID: {id}", "WARN");
                    return NotFound(new { message = "Beneficiary not found." });
                }

                await _apiLogger.LogAsync($"Beneficiary updated successfully. BeneficiaryID: {id}");

                return Ok(new { message = "Beneficiary updated successfully." });
            }
            catch (ArgumentException ex)
            {
                await _apiLogger.LogAsync($"Beneficiary update failed. BeneficiaryID: {id}. Error: {ex.Message}", "WARN");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBeneficiaryById(int id)
        {
            await _apiLogger.LogAsync($"GET /api/beneficiaries/{id} - Request received");

            try
            {
                var beneficiary = await _beneficiaryService.GetBeneficiaryByIdAsync(id);

                if (beneficiary == null)
                {
                    await _apiLogger.LogAsync($"Beneficiary not found. BeneficiaryID: {id}", "WARN");
                    return NotFound(new { message = "Beneficiary not found." });
                }

                await _apiLogger.LogAsync($"Beneficiary retrieved successfully. BeneficiaryID: {id}");

                return Ok(beneficiary);
            }
            catch (ArgumentException ex)
            {
                await _apiLogger.LogAsync($"Beneficiary retrieval failed. BeneficiaryID: {id}. Error: {ex.Message}", "WARN");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetBeneficiariesByCustomerId(int customerId)
        {
            await _apiLogger.LogAsync($"GET /api/beneficiaries/customer/{customerId} - Request received");

            try
            {
                var beneficiaries = await _beneficiaryService.GetBeneficiariesByCustomerIdAsync(customerId);

                await _apiLogger.LogAsync($"Beneficiaries retrieved for CustomerID: {customerId}");

                return Ok(beneficiaries);
            }
            catch (ArgumentException ex)
            {
                await _apiLogger.LogAsync($"Beneficiary retrieval failed for CustomerID: {customerId}. Error: {ex.Message}", "WARN");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateBeneficiaryStatus(int id, [FromBody] bool status)
        {
            await _apiLogger.LogAsync($"PATCH /api/beneficiaries/{id}/status - Request received");

            try
            {
                bool updated = await _beneficiaryService.UpdateBeneficiaryStatusAsync(id, status);

                if (!updated)
                {
                    await _apiLogger.LogAsync($"Beneficiary not found for status update. BeneficiaryID: {id}", "WARN");
                    return NotFound(new { message = "Beneficiary not found." });
                }

                string message = status
                    ? "Beneficiary activated successfully."
                    : "Beneficiary deactivated successfully.";

                await _apiLogger.LogAsync($"Beneficiary status updated. BeneficiaryID: {id}, Status: {status}");

                return Ok(new { message });
            }
            catch (ArgumentException ex)
            {
                await _apiLogger.LogAsync($"Beneficiary status update failed. BeneficiaryID: {id}. Error: {ex.Message}", "WARN");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
