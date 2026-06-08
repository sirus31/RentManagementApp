using Microsoft.AspNetCore.Mvc;

using RentManagementApp.DTOs.Requests;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
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

        [HttpPost("assign")]
        public async Task<IActionResult>
            AssignRoom(
                AssignRoomRequestDto request)
        {
            var response =
                await _occupancyService
                    .AssignRoomAsync(request);

            return Ok(response);
        }

        [HttpPost("vacate")]
        public async Task<IActionResult>
            VacateRoom(
                VacateRoomRequestDto request)
        {
            await _occupancyService
                .VacateRoomAsync(request);

            return Ok(
                "Room vacated successfully");
        }

        [HttpGet("active")]
        public async Task<IActionResult>
            GetActiveOccupancies()
        {
            var occupancies =
                await _occupancyService
                    .GetActiveOccupanciesAsync();

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
                        .MoveInTenantAsync(request);


                return Ok(result);
            }

            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message
                );
            }
        }

        // [HttpPost("move-in")]
        // public async Task<IActionResult> MoveInTenant(MoveInTenantRequestDto request)
        // {
        //     try
        //     {
        //         await _occupancyService
        //             .MoveInTenantAsync(
        //                 request);


        //         return Ok(
        //             "Tenant moved in successfully");
        //     }

        //     catch (Exception ex)
        //     {
        //         return BadRequest(
        //             ex.Message);
        //     }
        // }
    }
}