using System.ComponentModel.DataAnnotations;

namespace RentManagementApp.DTOs.Requests
{
    public class MoveInTenantRequestDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;


        [Required]
        public string PhoneNumber { get; set; } = string.Empty;


        [Required]
        public decimal MonthlyRent { get; set; }


        [Required]
        public List<int> RoomIds { get; set; } = new();


        [Required]
        public DateTime MoveInDate { get; set; }
    }
}