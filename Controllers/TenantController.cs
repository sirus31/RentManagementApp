using Microsoft.AspNetCore.Mvc;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant(
            CreateTenantRequestDto request)
        {
            var response = await _tenantService
                .CreateTenantAsync(request);

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            var tenants = await _tenantService
                .GetAllTenantsAsync();

            return Ok(tenants);
        }


        [HttpGet("active-rooms")]
        public async Task<IActionResult> GetTenantsWithActiveRooms()
        {
            var tenants =
                await _tenantService
                    .GetTenantsWithActiveRoomsAsync();

            return Ok(tenants);
        }


        [HttpGet("occupancy-history")]
        public async Task<IActionResult> GetTenantOccupancyHistory()
        {
            var tenants =
                await _tenantService
                    .GetTenantOccupancyHistoryAsync();

            return Ok(tenants);
        }


        [HttpPost("assign-room")]
        public async Task<IActionResult> AssignRoom(AssignRoomRequestDto request)
        {
            await _tenantService.AssignRoomAsync(request);

            return Ok("Room assigned successfully");
        }

        [HttpPost("{tenantId}/vacate")]
        public async Task<IActionResult> VacateTenant(int tenantId)
        {
            var result =
                await _tenantService
                    .VacateTenantAsync(
                        tenantId);


            return Ok(result);
        }
    }
}