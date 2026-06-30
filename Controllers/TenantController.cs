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
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value ?? "0");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant(
            CreateTenantRequestDto request)
        {
            var response = await _tenantService
                .CreateTenantAsync(request, GetUserId());

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            var tenants = await _tenantService
                .GetAllTenantsAsync(GetUserId());

            return Ok(tenants);
        }


        [HttpGet("active-rooms")]
        public async Task<IActionResult> GetTenantsWithActiveRooms()
        {
            var tenants =
                await _tenantService
                    .GetTenantsWithActiveRoomsAsync(GetUserId());

            return Ok(tenants);
        }


        [HttpGet("occupancy-history")]
        public async Task<IActionResult> GetTenantOccupancyHistory()
        {
            var tenants =
                await _tenantService
                    .GetTenantOccupancyHistoryAsync(GetUserId());

            return Ok(tenants);
        }

        [HttpPost("assign-room")]
        public async Task<IActionResult> AssignRoom(AssignRoomRequestDto request)
        {
            await _tenantService.AssignRoomAsync(request, GetUserId());

            return Ok("Room assigned successfully");
        }

        [HttpPost("{tenantId}/vacate")]
        public async Task<IActionResult> VacateTenant(int tenantId)
        {
            var result =
                await _tenantService
                    .VacateTenantAsync(
                        tenantId, GetUserId());

            return Ok(result);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetTenantById(int id)
        {
            var tenant = await _tenantService.GetTenantByIdAsync(id, GetUserId());


            if (tenant == null)
            {
                return NotFound();
            }


            return Ok(tenant);
        }

        [HttpGet("overview/{houseId}")]

        public async Task<IActionResult> GetTenantOverview(int houseId)
        {
            var tenants =
                await _tenantService
                .GetTenantOverviewAsync(
                    houseId, GetUserId()
                );


            return Ok(tenants);
        }
    }
}
