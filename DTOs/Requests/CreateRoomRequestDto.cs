using System.ComponentModel.DataAnnotations;

namespace RentManagementApp.DTOs.Requests
{
    public class CreateRoomRequestDto
    {
        [Required]
        public int FloorId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomNumber { get; set; } = null!;
    }
}