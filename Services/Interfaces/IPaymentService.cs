using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto>
            CreatePaymentAsync(
                CreatePaymentRequestDto request);


        Task<List<PaymentResponseDto>>
            GetBillPaymentsAsync(
                int billId);


        Task<List<PaymentResponseDto>>
            GetAllPaymentsAsync();

        Task<PaymentDashboardResponseDto>
            GetPaymentDashboardAsync(
                int? houseId,
                int? tenantId,
                int? month,
                int? year
            );

        Task<PaymentFilterResponseDto> GetPaymentFiltersAsync();
    }
}