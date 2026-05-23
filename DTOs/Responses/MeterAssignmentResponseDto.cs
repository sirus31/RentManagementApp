namespace RentManagementApp.DTOs.Responses
{
    public class MeterAssignmentResponseDto
    {
        public int TenantMeterId { get; set; }

        public int TenantId { get; set; }

        public string TenantName { get; set; } = null!;

        public int MeterId { get; set; }

        public string MeterNumber { get; set; } = null!;

        public string MeterType { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}