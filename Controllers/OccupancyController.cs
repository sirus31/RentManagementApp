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
    public class OccupancyController
        : ControllerBase
    {
        private readonly
            IOccupancyService _occupancyService;

        public OccupancyController(
            IOccupancyService occupancyService)
        {
            _occupancyService =
                occupancyService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value ?? "0");
        }

        [HttpPost("assign")]
        public async Task<IActionResult>
            AssignRoom(
                AssignRoomRequestDto request)
        {
            var response =
                await _occupancyService
                    .AssignRoomAsync(request, GetUserId());

            return Ok(response);
        }

        [HttpPost("vacate")]
        public async Task<IActionResult>
            VacateRoom(
                VacateRoomRequestDto request)
        {
            await _occupancyService
                .VacateRoomAsync(request, GetUserId());

            return Ok(
                "Room vacated successfully");
        }

        [HttpGet("active")]
        public async Task<IActionResult>
            GetActiveOccupancies()
        {
            var occupancies =
                await _occupancyService
                    .GetActiveOccupanciesAsync(GetUserId());

            return Ok(occupancies);
        }

        [HttpPost("move-in")]
        public async Task<IActionResult>
    MoveInTenant(
        MoveInTenantRequestDto request)
        {
            try
            {
                var result =
                    await _occupancyService
                        .MoveInTenantAsync(request, GetUserId());


                return Ok(result);
            }

            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message
                );
            }
        }
    }
}
