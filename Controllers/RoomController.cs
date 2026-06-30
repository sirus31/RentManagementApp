using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using RentManagementApp.DTOs.Requests;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
    [Authorize]
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

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value ?? "0");
        }

        [HttpPost]
        public async Task<IActionResult>
            CreateRoom(
                CreateRoomRequestDto request)
        {
            var response =
                await _roomService
                    .CreateRoomAsync(request, GetUserId());

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAllRooms()
        {
            var rooms =
                await _roomService
                    .GetAllRoomsAsync(GetUserId());

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
                        floorId, GetUserId());

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
                        houseId, GetUserId());

            return Ok(rooms);
        }

        [HttpGet("overview/{houseId}")]
        public async Task<IActionResult>
            GetRoomOverviewByHouse(
                int houseId)
        {
            try
            {
                var rooms =
                    await _roomService
                        .GetRoomOverviewByHouseAsync(
                            houseId, GetUserId());


                return Ok(
                    rooms);
            }


            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message);
            }
        }
    }
}
