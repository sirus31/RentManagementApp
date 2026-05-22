using System.ComponentModel.DataAnnotations;

namespace RentManagementApp.DTOs.Requests
{
    public class VacateRoomRequestDto
    {
        [Required]
        public int TenantRoomId { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}