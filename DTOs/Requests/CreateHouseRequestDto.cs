using System.ComponentModel.DataAnnotations;

namespace RentManagementApp.DTOs.Requests
{
    public class CreateHouseRequestDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(300)]
        public string Address { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal ElectricityRate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal GarbageFee { get; set; }
    }
}