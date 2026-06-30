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

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value ?? "0");
        }

        [HttpPost]
        public async Task<IActionResult>
            AddReading(
                CreateMeterReadingRequestDto request)
        {
            var response =
                await _meterReadingService
                    .AddReadingAsync(request, GetUserId());

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
                        meterId, GetUserId());

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
                        meterId, GetUserId());

            if (reading == null)
            {
                return NotFound(
                    "No readings found");
            }

            return Ok(reading);
        }
    }
}
