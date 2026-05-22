using System.ComponentModel.DataAnnotations;

namespace RentManagementApp.DTOs.Requests
{
    public class AssignRoomRequestDto
    {
        [Required]
        public int TenantId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
    }
}