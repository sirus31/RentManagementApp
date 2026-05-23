using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IMeterAssignmentService
    {
        Task<MeterAssignmentResponseDto>
            AssignMeterAsync(
                AssignMeterRequestDto request);

        Task RemoveMeterAssignmentAsync(
            RemoveMeterAssignmentRequestDto request);

        Task<List<MeterAssignmentResponseDto>>
            GetActiveAssignmentsAsync();
    }
}