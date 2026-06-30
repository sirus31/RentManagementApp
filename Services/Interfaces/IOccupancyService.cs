using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IOccupancyService
    {
        Task<OccupancyResponseDto>
            AssignRoomAsync(
                AssignRoomRequestDto request, int userId);

        Task VacateRoomAsync(
            VacateRoomRequestDto request, int userId);

        Task<List<OccupancyResponseDto>>
            GetActiveOccupanciesAsync(int userId);

        Task<OccupancyResponseDto> MoveInTenantAsync(
        MoveInTenantRequestDto request, int userId
    );
    }
}
