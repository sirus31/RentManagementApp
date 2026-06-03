namespace RentManagementApp.DTOs.Responses
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }

        public int BillId { get; set; }

        public string BillNumber { get; set; }
            = string.Empty;

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMode { get; set; }
            = string.Empty;

        public string Notes { get; set; }
            = string.Empty;
    }
}