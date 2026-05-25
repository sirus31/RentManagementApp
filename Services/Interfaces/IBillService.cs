using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IBillService
    {
        Task<BillResponseDto>
            GenerateBillAsync(
                GenerateBillRequestDto request);

        Task<List<BillResponseDto>>
            GetTenantBillsAsync(
                int tenantId);
    }
}