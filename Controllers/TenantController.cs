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
    }
}