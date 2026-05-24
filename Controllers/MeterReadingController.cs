using Microsoft.AspNetCore.Mvc;

using RentManagementApp.DTOs.Requests;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingController
        : ControllerBase
    {
        private readonly
            IMeterReadingService
                _meterReadingService;

        public MeterReadingController(
            IMeterReadingService
                meterReadingService)
        {
            _meterReadingService =
                meterReadingService;
        }

        [HttpPost]
        public async Task<IActionResult>
            AddReading(
                CreateMeterReadingRequestDto request)
        {
            var response =
                await _meterReadingService
                    .AddReadingAsync(request);

            return Ok(response);
        }

        [HttpGet("{meterId}")]
        public async Task<IActionResult>
            GetMeterReadings(
                int meterId)
        {
            var readings =
                await _meterReadingService
                    .GetMeterReadingsAsync(
                        meterId);

            return Ok(readings);
        }

        [HttpGet("latest/{meterId}")]
        public async Task<IActionResult>
            GetLatestReading(
                int meterId)
        {
            var reading =
                await _meterReadingService
                    .GetLatestReadingAsync(
                        meterId);

            if (reading == null)
            {
                return NotFound(
                    "No readings found");
            }

            return Ok(reading);
        }
    }
}