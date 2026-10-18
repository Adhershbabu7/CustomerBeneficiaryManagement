
using Microsoft.Data.SqlClient;
using System.Data;
using CustomerBeneficiaryManagement.Models;

namespace CustomerBeneficiaryManagement.DataAccess
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
        public async Task<int> AddCustomerAsync(Customer customer)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd=new SqlCommand("usp_Customer_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FullName", customer.FullName);
            cmd.Parameters.AddWithValue("@Nationality", customer.Nationality);
            cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
            cmd.Parameters.AddWithValue("@Email", customer.Email);
            cmd.Parameters.AddWithValue("@IDTypeID", customer.IDTypeID);
            cmd.Parameters.AddWithValue("@IDNo", customer.IDNo);

            await con.OpenAsync();
            object? result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);

        }
        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                "usp_Customer_Update", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
            cmd.Parameters.AddWithValue("@FullName", customer.FullName);
            cmd.Parameters.AddWithValue("@Nationality", customer.Nationality);
            cmd.Parameters.AddWithValue("@MobileNo", customer.MobileNo);
            cmd.Parameters.AddWithValue("@Email", customer.Email);
            cmd.Parameters.AddWithValue("@IDTypeID", customer.IDTypeID);
            cmd.Parameters.AddWithValue("@IDNo", customer.IDNo);

            await con.OpenAsync();

            int rowsAffected = Convert.ToInt32(
                await cmd.ExecuteScalarAsync());

            return rowsAffected > 0;
        }
        // 3. Get Customer by ID
        public async Task<Customer?> GetCustomerByIdAsync(int customerID)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                "usp_Customer_GetById", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerID", customerID);

            await con.OpenAsync();

            using SqlDataReader reader =
                await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapCustomer(reader);
            }

            return null;
        }

        // 4. Get All Customers
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var customers = new List<Customer>();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                "usp_Customer_GetAll", con);

            cmd.CommandType = CommandType.StoredProcedure;

            await con.OpenAsync();

            using SqlDataReader reader =
                await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                customers.Add(MapCustomer(reader));
            }

            return customers;
        }

        // 5. Activate / Deactivate Customer
        public async Task<bool> UpdateCustomerStatusAsync(
            int customerID, bool status)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(
                "usp_Customer_UpdateStatus", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Status", status);

            await con.OpenAsync();

            int rowsAffected = Convert.ToInt32(
                await cmd.ExecuteScalarAsync());

            return rowsAffected > 0;
        }

        // Convert SQL row into Customer object
        private static Customer MapCustomer(SqlDataReader reader)
        {
            return new Customer
            {
                CustomerID = reader.GetInt32(
                    reader.GetOrdinal("CustomerID")),

                FullName = reader.GetString(
                    reader.GetOrdinal("FullName")),

                Nationality = reader.GetString(
                    reader.GetOrdinal("Nationality")),

                MobileNo = reader.GetString(
                    reader.GetOrdinal("MobileNo")),

                Email = reader.GetString(
                    reader.GetOrdinal("Email")),

                Status = reader.GetBoolean(
                    reader.GetOrdinal("Status")),

                IDTypeID = reader.GetInt32(
                    reader.GetOrdinal("IDTypeID")),

                IDNo = reader.GetString(
                    reader.GetOrdinal("IDNo")),

                CreatedDate = reader.GetDateTime(
                    reader.GetOrdinal("CreatedDate"))
            };
        }

    }
}
