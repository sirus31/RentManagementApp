namespace RentManagementApp.DTOs.Responses
{
    public class TenantResponseDto
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public decimal MonthlyRent { get; set; }

        public bool IsActive { get; set; }
    }
}