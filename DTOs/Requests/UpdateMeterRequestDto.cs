using RentManagementApp.Models.Enums;

namespace RentManagementApp.DTOs.Requests
{
    public class UpdateMeterRequestDto
    {
        public string MeterNumber { get; set; }
            = string.Empty;

        public MeterType MeterType { get; set; }

        public decimal InitialReading { get; set; }

        public bool IsActive { get; set; }
    }
}