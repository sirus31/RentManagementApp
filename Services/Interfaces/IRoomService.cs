using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IRoomService
    {
        Task<RoomResponseDto>
            CreateRoomAsync(
                CreateRoomRequestDto request, int userId);

        Task<List<RoomResponseDto>>
            GetAllRoomsAsync(int userId);

        Task<List<RoomResponseDto>>
            GetRoomsByFloorAsync(
                int floorId, int userId);

        Task<List<RoomResponseDto>>
            GetAvailableRoomsByHouseAsync(
                int houseId, int userId);

        Task<List<RoomOverviewByFloorResponseDto>>
            GetRoomOverviewByHouseAsync(
                int houseId, int userId);
    }
}
