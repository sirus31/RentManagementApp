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
    public class FloorController : ControllerBase
    {
        private readonly IFloorService _floorService;

        public FloorController(
            IFloorService floorService)
        {
            _floorService = floorService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value ?? "0");
        }

        [HttpPost]
        public async Task<IActionResult>
            CreateFloor(
                CreateFloorRequestDto request)
        {
            var response =
                await _floorService
                    .CreateFloorAsync(request, GetUserId());

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAllFloors()
        {
            var floors =
                await _floorService
                    .GetAllFloorsAsync(GetUserId());

            return Ok(floors);
        }

        [HttpGet("house/{houseId}")]
        public async Task<IActionResult>
            GetFloorsByHouse(
                int houseId)
        {
            var floors =
                await _floorService
                    .GetFloorsByHouseAsync(
                        houseId, GetUserId());

            return Ok(floors);
        }
    }
}
