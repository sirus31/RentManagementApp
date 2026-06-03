namespace RentManagementApp.DTOs.Responses
{
    public class VacateTenantResponseDto
    {
        public int TenantId { get; set; }


        public string TenantName { get; set; }
            = string.Empty;


        public DateTime LeaveDate { get; set; }


        public string Message { get; set; }
            = string.Empty;
    }
}