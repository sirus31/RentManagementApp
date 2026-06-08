using Microsoft.AspNetCore.Mvc;

using RentManagementApp.DTOs.Requests;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterController
        : ControllerBase
    {
        private readonly
            IMeterService _meterService;

        public MeterController(
            IMeterService meterService)
        {
            _meterService = meterService;
        }

        [HttpPost]
        public async Task<IActionResult>
            CreateMeter(
                CreateMeterRequestDto request)
        {
            var response =
                await _meterService
                    .CreateMeterAsync(request);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAllMeters()
        {
            var response =
                await _meterService
                    .GetAllMetersAsync();

            return Ok(response);
        }

        [HttpGet("active")]
        public async Task<IActionResult>
            GetActiveMeters()
        {
            var response =
                await _meterService
                    .GetActiveMetersAsync();

            return Ok(response);
        }

        [HttpGet("{meterId}")]
        public async Task<IActionResult>
            GetMeterById(
                int meterId)
        {
            var response =
                await _meterService
                    .GetMeterByIdAsync(
                        meterId);

            return Ok(response);
        }

        [HttpPut("{meterId}")]
        public async Task<IActionResult>
            UpdateMeter(
                int meterId,
                UpdateMeterRequestDto request)
        {
            var response =
                await _meterService
                    .UpdateMeterAsync(
                        meterId,
                        request);

            return Ok(response);
        }

        [HttpPatch("{meterId}/deactivate")]
        public async Task<IActionResult>
            DeactivateMeter(
                int meterId)
        {
            var response =
                await _meterService
                    .DeactivateMeterAsync(
                        meterId);

            return Ok(response);
        }

        [HttpGet("house/{houseId}")]

        public async Task<IActionResult>
            GetMetersByHouse(
                int houseId)
        {

            var meters =
                await _meterService
                .GetMetersByHouseAsync(
                    houseId
                );


            return Ok(meters);

        }

        [HttpGet("overview/{houseId}")]
        public async Task<IActionResult> GetMeterOverview(int houseId)
        {
            var meters =
                await _meterService
                    .GetMeterOverviewByHouseAsync(
                        houseId
                    );


            return Ok(meters);
        }
    }
}