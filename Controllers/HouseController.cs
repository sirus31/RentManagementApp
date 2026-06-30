using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RentManagementApp.DTOs.Requests;
using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HouseController : ControllerBase
    {
        private readonly IHouseService _houseService;

        public HouseController(
            IHouseService houseService)
        {
            _houseService = houseService;
        }

        [HttpPost]
        public async Task<IActionResult>
            CreateHouse(CreateHouseRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                              ?? User.FindFirst("UserId")?.Value;

            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
            {
                request.UserId = userId;
            }

            var response =
                await _houseService
                    .CreateHouseAsync(request);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAllHouses()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                              ?? User.FindFirst("UserId")?.Value;

            int? userId = null;
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            var houses =
                await _houseService
                    .GetAllHousesAsync(userId);

            return Ok(houses);
        }
    }
}