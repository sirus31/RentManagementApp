using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface ITenantService
    {
        Task<TenantResponseDto> CreateTenantAsync(
            CreateTenantRequestDto request);
    }
}