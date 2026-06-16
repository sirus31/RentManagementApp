namespace RentManagementApp.DTOs.Responses
{
    public class PaymentDashboardResponseDto
    {
        public decimal TotalCollected { get; set; }


        public decimal TotalPending { get; set; }


        public int TotalTransactions { get; set; }



        public List<PendingPaymentDto> PendingPayments { get; set; }
            = new();



        public List<RecentPaymentDto> RecentPayments { get; set; }
            = new();
    }





    public class PendingPaymentDto
    {
        public int BillId { get; set; }


        public string TenantName { get; set; }
            = string.Empty;


        public string BillingMonth { get; set; }
            = string.Empty;


        public int BillingYear { get; set; }


        public decimal TotalAmount { get; set; }


        public decimal AmountPaid { get; set; }


        public decimal PendingAmount { get; set; }

        public bool CanReceivePayment { get; set; }
    }






    public class RecentPaymentDto
    {
        public int PaymentId { get; set; }


        public string TenantName { get; set; }
            = string.Empty;


        public string BillingMonth { get; set; }
            = string.Empty;


        public int BillingYear { get; set; }


        public decimal Amount { get; set; }


        public DateTime PaymentDate { get; set; }


        public string PaymentMode { get; set; }
            = string.Empty;
    }
}