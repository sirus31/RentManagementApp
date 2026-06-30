using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Models.MasterEntities;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Services
{
    public class FloorService : IFloorService
    {
        private readonly ApplicationDbContext _context;

        public FloorService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FloorResponseDto>
            CreateFloorAsync(
                CreateFloorRequestDto request, int userId)
        {
            var house =
                await _context.Houses
                    .FirstOrDefaultAsync(h =>
                        h.Id == request.HouseId);

            if (house == null || house.UserId != userId)
            {
                throw new Exception(
                    "House not found");
            }

            var floor = new Floor
            {
                HouseId = request.HouseId,

                FloorNumber =
                    request.FloorNumber,

                Name = request.Name
            };

            await _context.Floors
                .AddAsync(floor);

            await _context.SaveChangesAsync();

            return new FloorResponseDto
            {
                Id = floor.Id,

                HouseId = floor.HouseId,

                FloorNumber =
                    floor.FloorNumber,

                Name = floor.Name
            };
        }

        public async Task<List<FloorResponseDto>>
            GetAllFloorsAsync(int userId)
        {
            return await _context.Floors
                .Where(f =>
                    f.House.UserId == userId)
                .Select(f => new FloorResponseDto
                {
                    Id = f.Id,

                    HouseId = f.HouseId,

                    FloorNumber =
                        f.FloorNumber,

                    Name = f.Name
                })
                .ToListAsync();
        }

        public async Task<List<FloorResponseDto>>
            GetFloorsByHouseAsync(
                int houseId, int userId)
        {
            return await _context.Floors
                .Where(f =>
                    f.HouseId == houseId
                    && f.House.UserId == userId)

                .Select(f => new FloorResponseDto
                {
                    Id = f.Id,

                    HouseId = f.HouseId,

                    FloorNumber =
                        f.FloorNumber,

                    Name = f.Name
                })
                .ToListAsync();
        }
    }
}
