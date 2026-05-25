using RentManagementApp.Models.Enums;

namespace RentManagementApp.DTOs.Responses
{
    public class MeterResponseDto
    {
        public int Id { get; set; }

        public int HouseId { get; set; }

        public string MeterNumber { get; set; }
            = string.Empty;

        public MeterType MeterType { get; set; }

        public decimal InitialReading { get; set; }

        public bool IsActive { get; set; }
    }
}