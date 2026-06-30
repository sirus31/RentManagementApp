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
    public class MeterAssignmentController
        : ControllerBase
    {
        private readonly
            IMeterAssignmentService
                _meterAssignmentService;

        public MeterAssignmentController(
            IMeterAssignmentService
                meterAssignmentService)
        {
            _meterAssignmentService =
                meterAssignmentService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value ?? "0");
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignMeter(AssignMeterRequestDto request)
        {
            try
            {
                var response =
                    await _meterAssignmentService
                    .AssignMeterAsync(request, GetUserId());


                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message
                );
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult>
            RemoveAssignment(
                RemoveMeterAssignmentRequestDto request)
        {
            await _meterAssignmentService
                .RemoveMeterAssignmentAsync(
                    request, GetUserId());

            return Ok(
                "Assignment removed successfully");
        }

        [HttpGet("active")]
        public async Task<IActionResult>
            GetActiveAssignments()
        {
            var assignments =
                await _meterAssignmentService
                    .GetActiveAssignmentsAsync(GetUserId());

            return Ok(assignments);
        }
    }
}
