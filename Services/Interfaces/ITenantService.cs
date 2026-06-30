using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface ITenantService
    {
        Task<TenantResponseDto> CreateTenantAsync(
            CreateTenantRequestDto request, int userId);

        Task<List<TenantResponseDto>> GetAllTenantsAsync(int userId);

        Task<List<TenantRoomSummaryResponseDto>> GetTenantsWithActiveRoomsAsync(int userId);

        Task<List<TenantRoomSummaryResponseDto>> GetTenantOccupancyHistoryAsync(int userId);

        Task AssignRoomAsync(AssignRoomRequestDto request, int userId);

        Task<VacateTenantResponseDto> VacateTenantAsync(int tenantId, int userId);

        Task<TenantResponseDto?> GetTenantByIdAsync(int id, int userId);

        Task<List<TenantOverviewResponseDto>>
            GetTenantOverviewAsync(
                int houseId, int userId
            );
    }
}
