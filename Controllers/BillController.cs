using Microsoft.AspNetCore.Mvc;

using RentManagementApp.DTOs.Requests;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Controllers
{
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

        [HttpPost("generate")]
        public async Task<IActionResult>
            GenerateBill(
                GenerateBillRequestDto request)
        {
            var response =
                await _billService
                    .GenerateBillAsync(request);

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
                        tenantId);

            return Ok(bills);
        }
    }
}