using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IBillService
    {
        Task<BillResponseDto>
            GenerateBillAsync(
                GenerateBillRequestDto request, int userId);

        Task<List<GenerateBillResultDto>>
            GenerateAllBillsAsync(
                GenerateAllBillsRequestDto request, int userId);

        Task<List<BillResponseDto>>
            GetTenantBillsAsync(
                int tenantId, int userId);

        Task<List<BillResponseDto>>
            GetAllBillsAsync(int userId);

        Task<BillResponseDto>
            GetBillByIdAsync(
                int billId, int userId);

        Task<BillResponseDto>
            FinalizeBillAsync(
                int billId, int userId);

        Task<BillResponseDto>
            CancelBillAsync(
                int billId, int userId);

        Task<List<BillCycleOverviewResponseDto>>
            GetBillCyclesByHouseAsync(
                int houseId, int userId);
        Task<BillFullDetailResponseDto?> GetBillDetailsAsync(int billId, int userId);

        Task<GenerateBillInfoResponseDto> GetGenerateBillInfoAsync(int houseId, int userId);

        Task<List<GenerateBillResultDto>> GenerateMonthlyBillAsync(GenerateMonthlyBillRequestDto request, int userId);

        Task<BillCycleValidationResponseDto>
    ValidateBillCycleAsync(
        int houseId,
        int month,
        int year,
        int userId);
    }
}
