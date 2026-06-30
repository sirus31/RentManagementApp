using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Models.MasterEntities;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RoomResponseDto>
            CreateRoomAsync(
                CreateRoomRequestDto request, int userId)
        {
            var floor =
                await _context.Floors
                    .Include(f => f.House)
                    .FirstOrDefaultAsync(f =>
                        f.Id == request.FloorId);

            if (floor == null || floor.House.UserId != userId)
            {
                throw new Exception(
                    "Floor not found");
            }

            var room = new Room
            {
                FloorId = request.FloorId,

                RoomNumber =
                    request.RoomNumber
            };

            await _context.Rooms
                .AddAsync(room);

            await _context.SaveChangesAsync();

            return new RoomResponseDto
            {
                Id = room.Id,

                FloorId = room.FloorId,

                RoomNumber =
                    room.RoomNumber
            };
        }

        public async Task<List<RoomResponseDto>>
            GetAllRoomsAsync(int userId)
        {
            return await _context.Rooms
                .Where(r =>
                    r.Floor.House.UserId == userId)
                .Select(r => new RoomResponseDto
                {
                    Id = r.Id,

                    FloorId = r.FloorId,

                    RoomNumber =
                        r.RoomNumber
                })
                .ToListAsync();
        }

        public async Task<List<RoomResponseDto>>
            GetRoomsByFloorAsync(
                int floorId, int userId)
        {
            return await _context.Rooms
                .Where(r =>
                    r.FloorId == floorId
                    && r.Floor.House.UserId == userId)

                .Select(r => new RoomResponseDto
                {
                    Id = r.Id,

                    FloorId = r.FloorId,

                    RoomNumber =
                        r.RoomNumber
                })
                .ToListAsync();
        }

        public async Task<List<RoomResponseDto>>
            GetAvailableRoomsByHouseAsync(
                int houseId, int userId)
        {
            return await _context.Rooms

                .Include(r => r.Floor)

                .Where(r =>
                    r.Floor.HouseId == houseId
                    &&
                    r.Floor.House.UserId == userId
                    &&
                    !_context.TenantRooms
                        .Any(tr =>
                            tr.RoomId == r.Id
                            &&
                            tr.EndDate == null
                        )
                )

                .Select(r =>
                    new RoomResponseDto
                    {
                        Id = r.Id,

                        FloorId = r.FloorId,

                        RoomNumber = r.RoomNumber
                    })

                .ToListAsync();
        }

        public async Task<List<RoomOverviewByFloorResponseDto>> GetRoomOverviewByHouseAsync(int houseId, int userId)
        {
            var floors =
                await _context.Floors

                    .Include(f =>
                        f.Rooms)

                        .ThenInclude(r =>
                            r.TenantRooms)

                            .ThenInclude(tr =>
                                tr.Tenant)

                    .Where(f =>
                        f.HouseId == houseId
                        && f.House.UserId == userId)

                    .Select(f =>
                        new RoomOverviewByFloorResponseDto
                        {
                            FloorId =
                                f.Id,


                            FloorName =
                                f.Name,


                            TotalRooms =
                                f.Rooms.Count,


                            OccupiedRooms =
                                f.Rooms.Count(r =>
                                    r.TenantRooms.Any(tr =>
                                        tr.EndDate == null)),


                            VacantRooms =
                                f.Rooms.Count(r =>
                                    !r.TenantRooms.Any(tr =>
                                        tr.EndDate == null)),


                            Rooms =
                                f.Rooms

                                    .Select(r =>
                                        new RoomOverviewResponseDto
                                        {
                                            RoomId =
                                                r.Id,


                                            RoomNumber =
                                                r.RoomNumber,


                                            IsOccupied =
                                                r.TenantRooms.Any(tr =>
                                                    tr.EndDate == null),


                                            TenantName =
                                                r.TenantRooms

                                                    .Where(tr =>
                                                        tr.EndDate == null)

                                                    .Select(tr =>
                                                        tr.Tenant.FullName)

                                                    .FirstOrDefault()
                                        })

                                    .ToList()
                        })

                    .ToListAsync();


            return floors;
        }
    }
}
