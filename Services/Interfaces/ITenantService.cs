using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface ITenantService
    {
        Task<TenantResponseDto> CreateTenantAsync(
            CreateTenantRequestDto request);

        Task<List<TenantResponseDto>> GetAllTenantsAsync();

        Task<List<TenantRoomSummaryResponseDto>> GetTenantsWithActiveRoomsAsync();

        Task<List<TenantRoomSummaryResponseDto>> GetTenantOccupancyHistoryAsync();

        Task AssignRoomAsync(AssignRoomRequestDto request);
    }
}