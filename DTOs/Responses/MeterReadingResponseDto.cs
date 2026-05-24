namespace RentManagementApp.DTOs.Responses
{
    public class MeterReadingResponseDto
    {
        public int Id { get; set; }

        public int MeterId { get; set; }

        public string MeterNumber { get; set; } = string.Empty;

        public decimal ReadingValue { get; set; }

        public DateTime ReadingDate { get; set; }

        public int BillingYear { get; set; }

        public string BillingMonth { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}