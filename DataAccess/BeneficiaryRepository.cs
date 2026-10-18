using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using CustomerBeneficiaryManagement.Models;

namespace CustomerBeneficiaryManagement.DataAccess
{
    public class BeneficiaryRepository
    {
        private readonly string _connectionString;

        public BeneficiaryRepository(IConfiguration configuration)
        {
            _connectionString = configuration
                .GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Database connection string is missing.");
        }

        
        public async Task<int> AddBeneficiaryAsync(
            Beneficiary beneficiary)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                "usp_Beneficiary_Insert", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int)
                .Value = beneficiary.CustomerID;

            cmd.Parameters.Add("@BeneficiaryName", SqlDbType.NVarChar, 150)
                .Value = beneficiary.BeneficiaryName;

            cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 100)
                .Value = beneficiary.Country;

            cmd.Parameters.Add("@BankName", SqlDbType.NVarChar, 150)
                .Value = beneficiary.BankName;

            cmd.Parameters.Add("@AccountNumber", SqlDbType.NVarChar, 50)
                .Value = beneficiary.AccountNumber;

            await con.OpenAsync();

            object? result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        
        public async Task<bool> UpdateBeneficiaryAsync(
            Beneficiary beneficiary)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                "usp_Beneficiary_Update", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@BeneficiaryID", SqlDbType.Int)
                .Value = beneficiary.BeneficiaryID;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int)
                .Value = beneficiary.CustomerID;

            cmd.Parameters.Add("@BeneficiaryName", SqlDbType.NVarChar, 150)
                .Value = beneficiary.BeneficiaryName;

            cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 100)
                .Value = beneficiary.Country;

            cmd.Parameters.Add("@BankName", SqlDbType.NVarChar, 150)
                .Value = beneficiary.BankName;

            cmd.Parameters.Add("@AccountNumber", SqlDbType.NVarChar, 50)
                .Value = beneficiary.AccountNumber;

            await con.OpenAsync();

            int rowsAffected = Convert.ToInt32(
                await cmd.ExecuteScalarAsync());

            return rowsAffected > 0;
        }

       
        public async Task<Beneficiary?> GetBeneficiaryByIdAsync(
            int beneficiaryID)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                "usp_Beneficiary_GetById", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@BeneficiaryID", SqlDbType.Int)
                .Value = beneficiaryID;

            await con.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                return MapBeneficiary(reader);

            return null;
        }

        
        public async Task<List<Beneficiary>>
            GetBeneficiariesByCustomerIdAsync(int customerID)
        {
            var beneficiaries = new List<Beneficiary>();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                "usp_Beneficiary_GetByCustomer", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CustomerID", SqlDbType.Int)
                .Value = customerID;

            await con.OpenAsync();

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                beneficiaries.Add(MapBeneficiary(reader));

            return beneficiaries;
        }

      
        public async Task<bool> UpdateBeneficiaryStatusAsync(
            int beneficiaryID, bool status)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                "usp_Beneficiary_UpdateStatus", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@BeneficiaryID", SqlDbType.Int)
                .Value = beneficiaryID;

            cmd.Parameters.Add("@Status", SqlDbType.Bit)
                .Value = status;

            await con.OpenAsync();

            int rowsAffected = Convert.ToInt32(
                await cmd.ExecuteScalarAsync());

            return rowsAffected > 0;
        }

        
        private static Beneficiary MapBeneficiary(SqlDataReader reader)
        {
            return new Beneficiary
            {
                BeneficiaryID = reader.GetInt32(
                    reader.GetOrdinal("BeneficiaryID")),

                CustomerID = reader.GetInt32(
                    reader.GetOrdinal("CustomerID")),

                BeneficiaryName = reader.GetString(
                    reader.GetOrdinal("BeneficiaryName")),

                Country = reader.GetString(
                    reader.GetOrdinal("Country")),

                BankName = reader.GetString(
                    reader.GetOrdinal("BankName")),

                AccountNumber = reader.GetString(
                    reader.GetOrdinal("AccountNumber")),

                Status = reader.GetBoolean(
                    reader.GetOrdinal("Status")),

                CreatedDate = reader.GetDateTime(
                    reader.GetOrdinal("CreatedDate"))
            };
        }
    }
}