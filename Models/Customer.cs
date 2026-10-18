namespace CustomerBeneficiaryManagement.Models
{

    
        public class Customer
    {
        public int CustomerID { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Nationality { get; set; } = string.Empty;

        public string MobileNo { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool Status { get; set; }

        public int IDTypeID { get; set; }

        public string IDNo { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}
 
