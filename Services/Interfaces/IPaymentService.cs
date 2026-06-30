using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto>
            CreatePaymentAsync(
                CreatePaymentRequestDto request, int userId);


        Task<List<PaymentResponseDto>>
            GetBillPaymentsAsync(
                int billId, int userId);


        Task<List<PaymentResponseDto>>
            GetAllPaymentsAsync(int userId);

        Task<PaymentDashboardResponseDto>
            GetPaymentDashboardAsync(
                int userId,
                int? houseId,
                int? tenantId,
                int? month,
                int? year
            );

        Task<PaymentFilterResponseDto> GetPaymentFiltersAsync(int userId);
    }
}
