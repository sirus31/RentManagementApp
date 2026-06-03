namespace RentManagementApp.DTOs.Responses
{
    public class GenerateBillResultDto
    {
        public int TenantId { get; set; }


        public string TenantName { get; set; }
            = string.Empty;


        public string Status { get; set; }
            = string.Empty;


        public string Message { get; set; }
            = string.Empty;


        public int? BillId { get; set; }
    }
}