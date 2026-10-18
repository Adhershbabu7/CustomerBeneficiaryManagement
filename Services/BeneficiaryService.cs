using CustomerBeneficiaryManagement.DataAccess;
using CustomerBeneficiaryManagement.Models;

namespace CustomerBeneficiaryManagement.Services
{
    public class BeneficiaryService
    {
        private readonly BeneficiaryRepository _repository;

        public BeneficiaryService(BeneficiaryRepository repository)
        {
            _repository = repository;
        }

        // 1. Add Beneficiary
        public async Task<int> AddBeneficiaryAsync(
            Beneficiary beneficiary)
        {
            ValidateBeneficiary(beneficiary);

            if (beneficiary.CustomerID <= 0)
                throw new ArgumentException("Invalid Customer ID.");

            return await _repository.AddBeneficiaryAsync(beneficiary);
        }

        // 2. Update Beneficiary
        public async Task<bool> UpdateBeneficiaryAsync(
            Beneficiary beneficiary)
        {
            if (beneficiary.BeneficiaryID <= 0)
                throw new ArgumentException("Invalid Beneficiary ID.");

            ValidateBeneficiary(beneficiary);

            if (beneficiary.CustomerID <= 0)
                throw new ArgumentException("Invalid Customer ID.");

            return await _repository.UpdateBeneficiaryAsync(beneficiary);
        }

        // 3. Get Beneficiary by ID
        public async Task<Beneficiary?> GetBeneficiaryByIdAsync(
            int beneficiaryID)
        {
            if (beneficiaryID <= 0)
                throw new ArgumentException("Invalid Beneficiary ID.");

            return await _repository.GetBeneficiaryByIdAsync(
                beneficiaryID);
        }

        // 4. Get Beneficiaries by Customer ID
        public async Task<List<Beneficiary>>
            GetBeneficiariesByCustomerIdAsync(int customerID)
        {
            if (customerID <= 0)
                throw new ArgumentException("Invalid Customer ID.");

            return await _repository
                .GetBeneficiariesByCustomerIdAsync(customerID);
        }

        // 5. Activate / Deactivate Beneficiary
        public async Task<bool> UpdateBeneficiaryStatusAsync(
            int beneficiaryID, bool status)
        {
            if (beneficiaryID <= 0)
                throw new ArgumentException("Invalid Beneficiary ID.");

            return await _repository.UpdateBeneficiaryStatusAsync(
                beneficiaryID, status);
        }

        // Validate required fields
        private static void ValidateBeneficiary(
            Beneficiary beneficiary)
        {
            if (string.IsNullOrWhiteSpace(beneficiary.BeneficiaryName))
                throw new ArgumentException(
                    "Beneficiary name is required.");

            if (string.IsNullOrWhiteSpace(beneficiary.Country))
                throw new ArgumentException("Country is required.");

            if (string.IsNullOrWhiteSpace(beneficiary.BankName))
                throw new ArgumentException("Bank name is required.");

            if (string.IsNullOrWhiteSpace(beneficiary.AccountNumber))
                throw new ArgumentException(
                    "Account number is required.");
        }
    }
}