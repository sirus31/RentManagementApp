using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IFloorService
    {
        Task<FloorResponseDto>
            CreateFloorAsync(
                CreateFloorRequestDto request);

        Task<List<FloorResponseDto>>
            GetAllFloorsAsync();

        Task<List<FloorResponseDto>>
            GetFloorsByHouseAsync(
                int houseId);
    }
}