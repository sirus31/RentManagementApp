using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IMeterReadingService
    {
        Task<MeterReadingResponseDto>
            AddReadingAsync(
                CreateMeterReadingRequestDto request, int userId);

        Task<List<MeterReadingResponseDto>>
            GetMeterReadingsAsync(
                int meterId, int userId);

        Task<MeterReadingResponseDto?>
            GetLatestReadingAsync(
                int meterId, int userId);
    }
}
