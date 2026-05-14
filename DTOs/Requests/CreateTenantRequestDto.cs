using System.ComponentModel.DataAnnotations;

namespace RentManagementApp.DTOs.Requests
{
    public class CreateTenantRequestDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MonthlyRent { get; set; }
    }
}