using System.ComponentModel.DataAnnotations;

namespace RentManagementApp.DTOs.Requests
{
    public class RemoveMeterAssignmentRequestDto
    {
        [Required]
        public int TenantMeterId { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
