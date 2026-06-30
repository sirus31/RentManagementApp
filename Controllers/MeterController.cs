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

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value ?? "0");
        }

        [HttpPost]
        public async Task<IActionResult>
            CreateMeter(
                CreateMeterRequestDto request)
        {
            var response =
                await _meterService
                    .CreateMeterAsync(request, GetUserId());

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAllMeters()
        {
            var response =
                await _meterService
                    .GetAllMetersAsync(GetUserId());

            return Ok(response);
        }

        [HttpGet("active")]
        public async Task<IActionResult>
            GetActiveMeters()
        {
            var response =
                await _meterService
                    .GetActiveMetersAsync(GetUserId());

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
                        meterId, GetUserId());

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
                        request, GetUserId());

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
                        meterId, GetUserId());

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
                    houseId, GetUserId()
                );


            return Ok(meters);

        }

        [HttpGet("overview/{houseId}")]
        public async Task<IActionResult> GetMeterOverview(int houseId)
        {
            var meters =
                await _meterService
                    .GetMeterOverviewByHouseAsync(
                        houseId, GetUserId()
                    );


            return Ok(meters);
        }
    }
}
