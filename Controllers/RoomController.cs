using Microsoft.AspNetCore.Mvc;

using RentManagementApp.DTOs.Requests;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(
            IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost]
        public async Task<IActionResult>
            CreateRoom(
                CreateRoomRequestDto request)
        {
            var response =
                await _roomService
                    .CreateRoomAsync(request);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAllRooms()
        {
            var rooms =
                await _roomService
                    .GetAllRoomsAsync();

            return Ok(rooms);
        }

        [HttpGet("floor/{floorId}")]
        public async Task<IActionResult>
            GetRoomsByFloor(
                int floorId)
        {
            var rooms =
                await _roomService
                    .GetRoomsByFloorAsync(
                        floorId);

            return Ok(rooms);
        }

        [HttpGet("house/{houseId}/available")]
        public async Task<IActionResult>
    GetAvailableRoomsByHouse(
        int houseId)
        {
            var rooms =
                await _roomService
                    .GetAvailableRoomsByHouseAsync(
                        houseId);

            return Ok(rooms);
        }
    }
}