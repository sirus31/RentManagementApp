using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IMeterService
    {
        Task<MeterResponseDto>
            CreateMeterAsync(
                CreateMeterRequestDto request);

        Task<List<MeterResponseDto>>
            GetAllMetersAsync();

        Task<List<MeterResponseDto>>
            GetActiveMetersAsync();

        Task<MeterResponseDto>
            GetMeterByIdAsync(
                int meterId);

        Task<MeterResponseDto>
            UpdateMeterAsync(
                int meterId,
                UpdateMeterRequestDto request);

        Task<MeterResponseDto>
            DeactivateMeterAsync(
                int meterId);
    }
}