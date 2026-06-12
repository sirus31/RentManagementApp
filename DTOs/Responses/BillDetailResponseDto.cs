using RentManagementApp.Models.Enums;


namespace RentManagementApp.DTOs.Responses
{
    public class BillDetailResponseDto
    {
        public BillDetailType DetailType { get; set; }


        public string Description { get; set; }
            = string.Empty;


        public decimal PreviousReading { get; set; }


        public decimal CurrentReading { get; set; }


        public decimal UnitsConsumed { get; set; }


        public decimal TenantUnits { get; set; }


        public int SharedTenantCount { get; set; }


        public decimal Rate { get; set; }


        public decimal Amount { get; set; }
    }
}