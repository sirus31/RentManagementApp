using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IMeterService
    {
        Task<MeterResponseDto>
            CreateMeterAsync(
                CreateMeterRequestDto request, int userId);

        Task<List<MeterResponseDto>>
            GetAllMetersAsync(int userId);

        Task<List<MeterResponseDto>>
            GetActiveMetersAsync(int userId);

        Task<MeterResponseDto>
            GetMeterByIdAsync(
                int meterId, int userId);

        Task<MeterResponseDto>
            UpdateMeterAsync(
                int meterId,
                UpdateMeterRequestDto request, int userId);

        Task<MeterResponseDto>
            DeactivateMeterAsync(int meterId, int userId);

        Task<List<MeterResponseDto>> GetMetersByHouseAsync(int houseId, int userId);

        Task<List<MeterOverviewResponseDto>> GetMeterOverviewByHouseAsync(int houseId, int userId);
    }
}
