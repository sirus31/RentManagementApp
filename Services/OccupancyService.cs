using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Models.MasterEntities;
using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Services
{
    public class OccupancyService : IOccupancyService
    {
        private readonly ApplicationDbContext _context;

        public OccupancyService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OccupancyResponseDto>
            AssignRoomAsync(
                AssignRoomRequestDto request, int userId)
        {
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t =>
                    t.Id == request.TenantId);

            if (tenant == null)
            {
                throw new Exception(
                    "Tenant not found");
            }

            var room = await _context.Rooms
                .Include(r => r.Floor)
                    .ThenInclude(f => f.House)
                .FirstOrDefaultAsync(r =>
                    r.Id == request.RoomId);

            if (room == null || room.Floor.House.UserId != userId)
            {
                throw new Exception(
                    "Room not found");
            }

            var roomAlreadyOccupied =
                await _context.TenantRooms
                    .AnyAsync(tr =>
                        tr.RoomId == request.RoomId
                        &&
                        tr.EndDate == null);

            if (roomAlreadyOccupied)
            {
                throw new Exception(
                    "Room is already occupied");
            }

            var tenantRoom = new TenantRoom
            {
                TenantId = request.TenantId,

                RoomId = request.RoomId,

                StartDate = DateTime.SpecifyKind(
                    request.StartDate,
                    DateTimeKind.Utc)
            };

            await _context.TenantRooms
                .AddAsync(tenantRoom);

            tenant.IsActive = true;

            await _context.SaveChangesAsync();

            return new OccupancyResponseDto
            {
                TenantRoomId = tenantRoom.Id,

                TenantId = tenant.Id,

                TenantName = tenant.FullName,

                RoomId = room.Id,

                RoomNumber = room.RoomNumber,

                StartDate = tenantRoom.StartDate,

                EndDate = tenantRoom.EndDate
            };
        }

        public async Task VacateRoomAsync(
            VacateRoomRequestDto request, int userId)
        {
            var tenantRoom =
                await _context.TenantRooms
                    .Include(tr => tr.Tenant)
                    .Include(tr => tr.Room)
                        .ThenInclude(r => r.Floor)
                            .ThenInclude(f => f.House)
                    .FirstOrDefaultAsync(tr =>
                        tr.Id == request.TenantRoomId);

            if (tenantRoom == null || tenantRoom.Room.Floor.House.UserId != userId)
            {
                throw new Exception(
                    "Occupancy record not found");
            }

            if (tenantRoom.EndDate != null)
            {
                throw new Exception(
                    "Room already vacated");
            }

            tenantRoom.EndDate = DateTime.SpecifyKind(
                request.EndDate,
                DateTimeKind.Utc);

            var tenantStillOccupiesRooms =
                await _context.TenantRooms
                    .AnyAsync(tr =>
                        tr.TenantId ==
                            tenantRoom.TenantId
                        &&
                        tr.EndDate == null
                        &&
                        tr.Id != tenantRoom.Id);

            if (!tenantStillOccupiesRooms)
            {
                tenantRoom.Tenant.IsActive = false;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<OccupancyResponseDto>>
            GetActiveOccupanciesAsync(int userId)
        {
            return await _context.TenantRooms
                .Include(tr => tr.Tenant)
                .Include(tr => tr.Room)
                    .ThenInclude(r => r.Floor)

                .Where(tr => tr.EndDate == null
                    && tr.Room.Floor.House.UserId == userId)

                .Select(tr =>
                    new OccupancyResponseDto
                    {
                        TenantRoomId = tr.Id,

                        TenantId = tr.TenantId,

                        TenantName =
                            tr.Tenant.FullName,

                        RoomId = tr.RoomId,

                        RoomNumber =
                            tr.Room.RoomNumber,

                        StartDate =
                            tr.StartDate,

                        EndDate =
                            tr.EndDate
                    })
                .ToListAsync();
        }


        public async Task<OccupancyResponseDto>
    MoveInTenantAsync(
        MoveInTenantRequestDto request, int userId)
        {
            if (
                request.RoomIds == null
                ||
                !request.RoomIds.Any()
            )
            {
                throw new Exception(
                    "At least one room is required");
            }


            using var transaction =
                await _context.Database
                    .BeginTransactionAsync();


            try
            {
                var tenant =
                    new Tenant
                    {
                        FullName =
                            request.FullName,

                        PhoneNumber =
                            request.PhoneNumber,

                        MonthlyRent =
                            request.MonthlyRent,

                        IsActive =
                            true
                    };


                await _context.Tenants
                    .AddAsync(tenant);


                await _context
                    .SaveChangesAsync();



                TenantRoom firstTenantRoom = null!;



                foreach (var roomId in request.RoomIds)
                {
                    var room =
                        await _context.Rooms
                            .Include(r => r.Floor)
                                .ThenInclude(f => f.House)
                            .FirstOrDefaultAsync(r =>
                                r.Id == roomId);


                    if (room == null || room.Floor.House.UserId != userId)
                    {
                        throw new Exception(
                            "Room not found");
                    }



                    var roomAlreadyOccupied =
                        await _context.TenantRooms
                            .AnyAsync(tr =>
                                tr.RoomId == roomId
                                &&
                                tr.EndDate == null);


                    if (roomAlreadyOccupied)
                    {
                        throw new Exception(
                            $"Room {room.RoomNumber} is already occupied");
                    }



                    var tenantRoom =
                        new TenantRoom
                        {
                            TenantId =
                                tenant.Id,


                            RoomId =
                                room.Id,


                            StartDate =
                                DateTime.SpecifyKind(
                                    request.MoveInDate,
                                    DateTimeKind.Utc)
                        };



                    await _context.TenantRooms
                        .AddAsync(tenantRoom);



                    if (firstTenantRoom == null)
                    {
                        firstTenantRoom =
                            tenantRoom;
                    }
                }



                await _context
                    .SaveChangesAsync();



                await transaction
                    .CommitAsync();



                var assignedRoom =
                    await _context.Rooms
                        .FirstAsync(r =>
                            r.Id ==
                            firstTenantRoom.RoomId);



                return new OccupancyResponseDto
                {
                    TenantRoomId =
                        firstTenantRoom.Id,


                    TenantId =
                        tenant.Id,


                    TenantName =
                        tenant.FullName,


                    RoomId =
                        assignedRoom.Id,


                    RoomNumber =
                        assignedRoom.RoomNumber,


                    StartDate =
                        firstTenantRoom.StartDate,


                    EndDate =
                        firstTenantRoom.EndDate
                };
            }


            catch
            {
                await transaction
                    .RollbackAsync();


                throw;
            }
        }
    }
}
