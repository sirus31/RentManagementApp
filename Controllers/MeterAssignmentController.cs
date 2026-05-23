using Microsoft.AspNetCore.Mvc;

using RentManagementApp.DTOs.Requests;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
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

        [HttpPost("assign")]
        public async Task<IActionResult>
            AssignMeter(
                AssignMeterRequestDto request)
        {
            var response =
                await _meterAssignmentService
                    .AssignMeterAsync(request);

            return Ok(response);
        }

        [HttpPost("remove")]
        public async Task<IActionResult>
            RemoveAssignment(
                RemoveMeterAssignmentRequestDto request)
        {
            await _meterAssignmentService
                .RemoveMeterAssignmentAsync(
                    request);

            return Ok(
                "Assignment removed successfully");
        }

        [HttpGet("active")]
        public async Task<IActionResult>
            GetActiveAssignments()
        {
            var assignments =
                await _meterAssignmentService
                    .GetActiveAssignmentsAsync();

            return Ok(assignments);
        }
    }
}