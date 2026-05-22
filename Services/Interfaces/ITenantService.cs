using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface ITenantService
    {
        Task<TenantResponseDto> CreateTenantAsync(
            CreateTenantRequestDto request);

        Task<List<TenantResponseDto>> GetAllTenantsAsync();

        Task<List<TenantWithRoomsResponseDto>> GetTenantsWithRoomsAsync();

        Task AssignRoomAsync(AssignRoomRequestDto request);
    }
}