namespace RentManagementApp.DTOs.Responses
{
    public class BillCycleValidationResponseDto
    {
        public bool IsValid { get; set; }


        public string Message { get; set; }
            = string.Empty;
    }
}