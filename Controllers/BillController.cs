using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BillController
        : ControllerBase
    {
        private readonly
            IBillService
                _billService;

        public BillController(
            IBillService billService)
        {
            _billService = billService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value ?? "0");
        }

        [HttpPost("generate")]
        public async Task<IActionResult>
            GenerateBill(
                GenerateBillRequestDto request)
        {
            var response =
                await _billService
                    .GenerateBillAsync(request, GetUserId());

            return Ok(response);
        }

        [HttpGet("tenant/{tenantId}")]
        public async Task<IActionResult>
            GetTenantBills(
                int tenantId)
        {
            var bills =
                await _billService
                    .GetTenantBillsAsync(
                        tenantId, GetUserId());

            return Ok(bills);
        }

        [HttpGet]
        public async Task<IActionResult>
            GetAllBills()
        {
            var bills =
                await _billService
                    .GetAllBillsAsync(GetUserId());

            return Ok(bills);
        }

        [HttpGet("{billId}")]
        public async Task<IActionResult>
            GetBillById(
                int billId)
        {
            var bill =
                await _billService
                    .GetBillByIdAsync(
                        billId, GetUserId());

            return Ok(bill);
        }

        [HttpPatch("{billId}/finalize")]
        public async Task<IActionResult>
            FinalizeBill(
                int billId)
        {
            var bill =
                await _billService
                    .FinalizeBillAsync(
                        billId, GetUserId());

            return Ok(bill);
        }

        [HttpPatch("{billId}/cancel")]
        public async Task<IActionResult>
            CancelBill(
                int billId)
        {
            var bill =
                await _billService
                    .CancelBillAsync(
                        billId, GetUserId());

            return Ok(bill);
        }

        [HttpPost("generate-all")]
        public async Task<IActionResult>
            GenerateAllBills(
                GenerateAllBillsRequestDto request)
        {
            var result =
                await _billService
                    .GenerateAllBillsAsync(
                        request, GetUserId());


            return Ok(result);
        }

        [HttpGet("house/{houseId}/cycles")]
        public async Task<IActionResult>
            GetBillCyclesByHouse(
                int houseId)
        {
            var cycles =
                await _billService
                    .GetBillCyclesByHouseAsync(
                        houseId, GetUserId());

            return Ok(cycles);
        }

        [HttpGet("{billId}/details")]
        public async Task<ActionResult<BillFullDetailResponseDto>> GetBillDetails(int billId)
        {
            var result =
                await _billService.GetBillDetailsAsync(billId, GetUserId());


            if (result == null)
            {
                return NotFound(
                    "Bill not found"
                );
            }


            return Ok(result);
        }

        [HttpGet("generate-info/{houseId}")]
        public async Task<ActionResult<GenerateBillInfoResponseDto>>
    GetGenerateBillInfo(
        int houseId
    )
        {
            var result =
                await _billService.GetGenerateBillInfoAsync(
                    houseId, GetUserId()
                );


            return Ok(result);
        }

        [HttpPost("generate-monthly")]
        public async Task<IActionResult>
    GenerateMonthlyBill(
        GenerateMonthlyBillRequestDto request)
        {
            try
            {
                var result =
                    await _billService
                        .GenerateMonthlyBillAsync(
                            request, GetUserId()
                        );


                return Ok(result);
            }


            catch (Exception ex)
            {
                return BadRequest(
                    ex.Message
                );
            }
        }

        [HttpGet("validate-cycle")]
        public async Task<IActionResult>
    ValidateBillCycle(
        int houseId,
        int month,
        int year)
        {
            var result =
                await _billService
                    .ValidateBillCycleAsync(
                        houseId,
                        month,
                        year, GetUserId());


            return Ok(result);
        }
    }
}
