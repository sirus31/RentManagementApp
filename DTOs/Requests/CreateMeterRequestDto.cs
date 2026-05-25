using RentManagementApp.Models.Enums;

namespace RentManagementApp.DTOs.Requests
{
    public class CreateMeterRequestDto
    {
        public int HouseId { get; set; }

        public string MeterNumber { get; set; }
            = string.Empty;

        public MeterType MeterType { get; set; }

        public decimal InitialReading { get; set; }
    }
}