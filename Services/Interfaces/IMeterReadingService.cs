using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IMeterReadingService
    {
        Task<MeterReadingResponseDto>
            AddReadingAsync(
                CreateMeterReadingRequestDto request);

        Task<List<MeterReadingResponseDto>>
            GetMeterReadingsAsync(
                int meterId);

        Task<MeterReadingResponseDto?>
            GetLatestReadingAsync(
                int meterId);
    }
}