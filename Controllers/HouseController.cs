using Microsoft.AspNetCore.Mvc;

using RentManagementApp.DTOs.Requests;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
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
            var response =
                await _houseService
                    .CreateHouseAsync(request);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAllHouses()
        {
            var houses =
                await _houseService
                    .GetAllHousesAsync();

            return Ok(houses);
        }
    }
}