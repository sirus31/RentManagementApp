using Microsoft.AspNetCore.Mvc;

using RentManagementApp.DTOs.Requests;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
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

        [HttpPost]
        public async Task<IActionResult>
            CreateFloor(
                CreateFloorRequestDto request)
        {
            var response =
                await _floorService
                    .CreateFloorAsync(request);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAllFloors()
        {
            var floors =
                await _floorService
                    .GetAllFloorsAsync();

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
                        houseId);

            return Ok(floors);
        }
    }
}