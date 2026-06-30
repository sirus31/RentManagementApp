using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IFloorService
    {
        Task<FloorResponseDto>
            CreateFloorAsync(
                CreateFloorRequestDto request, int userId);

        Task<List<FloorResponseDto>>
            GetAllFloorsAsync(int userId);

        Task<List<FloorResponseDto>>
            GetFloorsByHouseAsync(
                int houseId, int userId);
    }
}
