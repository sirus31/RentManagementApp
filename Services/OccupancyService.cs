using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

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
                AssignRoomRequestDto request)
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
                .FirstOrDefaultAsync(r =>
                    r.Id == request.RoomId);

            if (room == null)
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
            VacateRoomRequestDto request)
        {
            var tenantRoom =
                await _context.TenantRooms
                    .Include(tr => tr.Tenant)
                    .FirstOrDefaultAsync(tr =>
                        tr.Id == request.TenantRoomId);

            if (tenantRoom == null)
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
            GetActiveOccupanciesAsync()
        {
            return await _context.TenantRooms
                .Include(tr => tr.Tenant)
                .Include(tr => tr.Room)

                .Where(tr => tr.EndDate == null)

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
    }
}