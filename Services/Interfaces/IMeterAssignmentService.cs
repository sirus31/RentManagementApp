using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IMeterAssignmentService
    {
        Task<MeterAssignmentResponseDto>
            AssignMeterAsync(
                AssignMeterRequestDto request, int userId);

        Task RemoveMeterAssignmentAsync(
            RemoveMeterAssignmentRequestDto request, int userId);

        Task<List<MeterAssignmentResponseDto>>
            GetActiveAssignmentsAsync(int userId);
    }
}
