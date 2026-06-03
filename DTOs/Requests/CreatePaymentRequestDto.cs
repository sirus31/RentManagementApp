namespace RentManagementApp.DTOs.Requests
{
    public class CreatePaymentRequestDto
    {
        public int BillId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMode { get; set; }
            = string.Empty;

        public string Notes { get; set; }
            = string.Empty;
    }
}