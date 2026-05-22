using System.ComponentModel.DataAnnotations;

namespace RentManagementApp.DTOs.Requests
{
    public class CreateFloorRequestDto
    {
        [Required]
        public int HouseId { get; set; }

        [Required]
        public int FloorNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}