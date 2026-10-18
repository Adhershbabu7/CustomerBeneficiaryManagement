namespace CustomerBeneficiaryManagement.Models
{
    public class Beneficiary
    {
        public int BeneficiaryID { get; set; }

        public int CustomerID { get; set; }

        public string BeneficiaryName { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string BankName { get; set; } = string.Empty;

        public string AccountNumber { get; set; } = string.Empty;

        public bool Status { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}