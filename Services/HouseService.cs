using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Models.MasterEntities;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Services
{
    public class HouseService : IHouseService
    {
        private readonly ApplicationDbContext _context;

        public HouseService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HouseResponseDto>CreateHouseAsync(CreateHouseRequestDto request)
        {
            var house = new House
            {
                UserId = request.UserId,

                Name = request.Name,

                Address = request.Address,

                ElectricityRate =
                    request.ElectricityRate,

                GarbageFee =
                    request.GarbageFee,

                CreatedAt = DateTime.UtcNow
            };

            await _context.Houses
                .AddAsync(house);

            await _context.SaveChangesAsync();

            return new HouseResponseDto
            {
                Id = house.Id,

                UserId = house.UserId,

                Name = house.Name,

                Address = house.Address,

                ElectricityRate =
                    house.ElectricityRate,

                GarbageFee =
                    house.GarbageFee,

                CreatedAt =
                    house.CreatedAt
            };
        }

        public async Task<List<HouseResponseDto>> GetAllHousesAsync()
        {
            return await _context.Houses
                .Select(h => new HouseResponseDto
                {
                    Id = h.Id,

                    UserId = h.UserId,

                    Name = h.Name,

                    Address = h.Address,

                    ElectricityRate =
                        h.ElectricityRate,

                    GarbageFee =
                        h.GarbageFee,

                    CreatedAt =
                        h.CreatedAt
                })
                .ToListAsync();
        }
    }
}