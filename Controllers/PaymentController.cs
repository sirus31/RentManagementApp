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
    public class PaymentController
        : ControllerBase
    {
        private readonly IPaymentService _paymentService;


        public PaymentController(
            IPaymentService paymentService)
        {
            _paymentService =
                paymentService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("UserId")?.Value ?? "0");
        }


        [HttpPost]
        public async Task<IActionResult>
            CreatePayment(
                CreatePaymentRequestDto request)
        {
            var payment =
                await _paymentService
                    .CreatePaymentAsync(
                        request, GetUserId());


            return Ok(payment);
        }



        [HttpGet("bill/{billId}")]
        public async Task<IActionResult>
            GetBillPayments(
                int billId)
        {
            var payments =
                await _paymentService
                    .GetBillPaymentsAsync(
                        billId, GetUserId());


            return Ok(payments);
        }



        [HttpGet]
        public async Task<IActionResult>
            GetAllPayments()
        {
            var payments =
                await _paymentService
                    .GetAllPaymentsAsync(GetUserId());


            return Ok(payments);
        }


        [HttpGet("dashboard")]

        public async Task<IActionResult>
            GetPaymentDashboard(
                int? houseId,
                int? tenantId,
                int? month,
                int? year)
        {
            var dashboard =
                await _paymentService
                    .GetPaymentDashboardAsync(
                        GetUserId(),
                        houseId,
                        tenantId,
                        month,
                        year
                    );


            return Ok(
                dashboard);
        }

        [HttpGet("filters")]

        public async Task<IActionResult> GetPaymentFilters()
        {

            var filters =
                await _paymentService
                    .GetPaymentFiltersAsync(GetUserId());



            return Ok(filters);
        }
    }
}
