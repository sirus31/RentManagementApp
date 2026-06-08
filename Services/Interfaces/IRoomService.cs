using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IRoomService
    {
        Task<RoomResponseDto>
            CreateRoomAsync(
                CreateRoomRequestDto request);

        Task<List<RoomResponseDto>>
            GetAllRoomsAsync();

        Task<List<RoomResponseDto>>
            GetRoomsByFloorAsync(
                int floorId);

        Task<List<RoomResponseDto>>
            GetAvailableRoomsByHouseAsync(
                int houseId);
    }
}