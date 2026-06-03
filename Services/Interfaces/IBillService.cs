using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IBillService
    {
        Task<BillResponseDto>
            GenerateBillAsync(
                GenerateBillRequestDto request);

        Task<List<GenerateBillResultDto>>
            GenerateAllBillsAsync(
                GenerateAllBillsRequestDto request);

        Task<List<BillResponseDto>>
            GetTenantBillsAsync(
                int tenantId);
        
        Task<List<BillResponseDto>>
            GetAllBillsAsync();

        Task<BillResponseDto>
            GetBillByIdAsync(
                int billId);

        Task<BillResponseDto>
            FinalizeBillAsync(
                int billId);
        
        Task<BillResponseDto>
            CancelBillAsync(
                int billId);
            
    }
}