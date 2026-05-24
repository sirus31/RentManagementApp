using RentManagementApp.Models.Enums;

namespace RentManagementApp.DTOs.Requests
{
    public class CreateMeterReadingRequestDto
    {
        public int MeterId { get; set; }

        public decimal ReadingValue { get; set; }

        public DateTime ReadingDate { get; set; }

        public int BillingYear { get; set; }

        public BillingMonth BillingMonth { get; set; }

        public string? Notes { get; set; }
    }
}