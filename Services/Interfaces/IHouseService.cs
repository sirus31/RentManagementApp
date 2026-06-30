using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

namespace RentManagementApp.Services.Interfaces
{
    public interface IHouseService
    {
        Task<HouseResponseDto>
            CreateHouseAsync(
                CreateHouseRequestDto request);

        Task<List<HouseResponseDto>>
            GetAllHousesAsync(int? userId = null);
    }
}