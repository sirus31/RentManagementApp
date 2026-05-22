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

        [HttpGet("with-rooms")]
        public async Task<IActionResult> GetTenantsWithRooms()
        {
            var tenants = await _tenantService
                .GetTenantsWithRoomsAsync();

            return Ok(tenants);
        }

        [HttpPost("assign-room")]
        public async Task<IActionResult> AssignRoom(AssignRoomRequestDto request)
        {
            await _tenantService.AssignRoomAsync(request);

            return Ok("Room assigned successfully");
        }
    }
}