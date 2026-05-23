using System.ComponentModel.DataAnnotations;

namespace RentManagementApp.DTOs.Requests
{
    public class AssignMeterRequestDto
    {
        [Required]
        public int TenantId { get; set; }

        [Required]
        public int MeterId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
    }
}