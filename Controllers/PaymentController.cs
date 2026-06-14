using Microsoft.AspNetCore.Mvc;

using RentManagementApp.DTOs.Requests;

using RentManagementApp.Services.Interfaces;


namespace RentManagementApp.Controllers
{
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



        [HttpPost]
        public async Task<IActionResult>
            CreatePayment(
                CreatePaymentRequestDto request)
        {
            var payment =
                await _paymentService
                    .CreatePaymentAsync(
                        request);


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
                        billId);


            return Ok(payments);
        }



        [HttpGet]
        public async Task<IActionResult>
            GetAllPayments()
        {
            var payments =
                await _paymentService
                    .GetAllPaymentsAsync();


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
                    .GetPaymentFiltersAsync();



            return Ok(filters);
        }
    }
}