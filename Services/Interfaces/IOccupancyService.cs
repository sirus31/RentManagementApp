using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IOccupancyService
    {
        Task<OccupancyResponseDto>
            AssignRoomAsync(
                AssignRoomRequestDto request);

        Task VacateRoomAsync(
            VacateRoomRequestDto request);

        Task<List<OccupancyResponseDto>>
            GetActiveOccupanciesAsync();

        Task<OccupancyResponseDto> MoveInTenantAsync(
        MoveInTenantRequestDto request
    );
    }
}