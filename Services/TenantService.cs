using Microsoft.EntityFrameworkCore;
using RentManagementApp.Data;
using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;
using RentManagementApp.Models.Enums;
using RentManagementApp.Models.MasterEntities;
using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Services
{
    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _context;

        public TenantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TenantResponseDto> CreateTenantAsync(
            CreateTenantRequestDto request, int userId)
        {
            var tenant = new Tenant
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                MonthlyRent = request.MonthlyRent,
                IsActive = true,
                JoinDate = DateTime.UtcNow
            };

            await _context.Tenants.AddAsync(tenant);

            await _context.SaveChangesAsync();

            var response = new TenantResponseDto
            {
                Id = tenant.Id,
                FullName = tenant.FullName,
                PhoneNumber = tenant.PhoneNumber,
                MonthlyRent = tenant.MonthlyRent,
                IsActive = tenant.IsActive
            };

            return response;
        }


        public async Task<List<TenantResponseDto>> GetAllTenantsAsync(int userId)
        {
            var tenants = await _context.Tenants
                .Where(t =>
                    t.TenantRooms.Any(tr =>
                        tr.Room.Floor.House.UserId == userId))
                .Select(t => new TenantResponseDto
                {
                    Id = t.Id,
                    FullName = t.FullName,
                    PhoneNumber = t.PhoneNumber,
                    MonthlyRent = t.MonthlyRent,
                    IsActive = t.IsActive
                })
                .ToListAsync();

            return tenants;
        }

        public async Task<List<TenantRoomSummaryResponseDto>> GetTenantsWithActiveRoomsAsync(int userId)
        {
            var tenants = await _context.Tenants
                .Include(t => t.TenantRooms)
                    .ThenInclude(tr => tr.Room)
                        .ThenInclude(r => r.Floor)
                .Where(t =>
                    t.TenantRooms.Any(tr =>
                        tr.EndDate == null
                        && tr.Room.Floor.House.UserId == userId))

                .Select(t => new TenantRoomSummaryResponseDto
                {
                    Id = t.Id,

                    FullName = t.FullName,

                    RoomNumbers = t.TenantRooms

                        .Where(tr => tr.EndDate == null
                            && tr.Room.Floor.House.UserId == userId)

                        .Select(tr => tr.Room.RoomNumber)

                        .ToList()
                })
                .ToListAsync();

            return tenants;
        }

        public async Task<List<TenantRoomSummaryResponseDto>> GetTenantOccupancyHistoryAsync(int userId)
        {
            var tenants = await _context.Tenants
                .Include(t => t.TenantRooms)
                    .ThenInclude(tr => tr.Room)
                        .ThenInclude(r => r.Floor)
                .Where(t =>
                    t.TenantRooms.Any(tr =>
                        tr.Room.Floor.House.UserId == userId))

                .Select(t => new TenantRoomSummaryResponseDto
                {
                    Id = t.Id,

                    FullName = t.FullName,

                    RoomNumbers = t.TenantRooms

                        .Where(tr => tr.Room.Floor.House.UserId == userId)

                        .Select(tr => tr.Room.RoomNumber)

                        .ToList()
                })
                .ToListAsync();

            return tenants;
        }
        public async Task AssignRoomAsync(AssignRoomRequestDto request, int userId)
        {
            var tenantExists = await _context.Tenants
                .AnyAsync(t => t.Id == request.TenantId);

            if (!tenantExists)
            {
                throw new Exception("Tenant not found");
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == request.RoomId);

            if (room == null || room.Floor.House.UserId != userId)
            {
                throw new Exception("Room not found");
            }

            var tenantRoom = new TenantRoom
            {
                TenantId = request.TenantId,
                RoomId = request.RoomId,
                StartDate = request.StartDate,
            };

            await _context.TenantRooms
                .AddAsync(tenantRoom);

            await _context.SaveChangesAsync();
        }

        public async Task<VacateTenantResponseDto> VacateTenantAsync(int tenantId, int userId)
        {
            var tenant =
                await _context.Tenants

                    .Include(t => t.TenantRooms)
                        .ThenInclude(tr => tr.Room)
                            .ThenInclude(r => r.Floor)
                                .ThenInclude(f => f.House)

                    .Include(t => t.TenantMeters)
                        .ThenInclude(tm => tm.Meter)

                    .Include(t => t.Bills)

                    .FirstOrDefaultAsync(t =>
                        t.Id == tenantId);


            if (tenant == null)
            {
                throw new Exception(
                    "Tenant not found");
            }

            if (!tenant.TenantRooms.Any(tr =>
                tr.Room.Floor.House.UserId == userId))
            {
                throw new Exception(
                    "Tenant not found");
            }


            if (!tenant.IsActive)
            {
                throw new Exception(
                    "Tenant already vacated");
            }


            var hasPendingBills =
                tenant.Bills.Any(b =>
                    b.PaymentStatus != PaymentStatus.Paid);


            if (hasPendingBills)
            {
                throw new Exception(
                    "Tenant has unpaid bills. Clear payment before vacating.");
            }



            var leavingDate =
                DateTime.UtcNow;



            foreach (var room in tenant.TenantRooms
                        .Where(tr => tr.EndDate == null))
            {
                room.EndDate =
                    leavingDate;
            }



            foreach (var meter in tenant.TenantMeters
                        .Where(tm => tm.EndDate == null))
            {
                meter.EndDate =
                    leavingDate;
            }



            tenant.IsActive =
                false;


            tenant.LeaveDate =
                leavingDate;



            await _context.SaveChangesAsync();



            return new VacateTenantResponseDto
            {
                TenantId =
                    tenant.Id,

                TenantName =
                    tenant.FullName,

                LeaveDate =
                    leavingDate,

                Message =
                    "Tenant vacated successfully"
            };
        }

        public async Task<TenantResponseDto?> GetTenantByIdAsync(int id, int userId)
        {
            var tenant = await _context.Tenants
                .Include(t => t.TenantRooms)
                    .ThenInclude(tr => tr.Room)
                        .ThenInclude(r => r.Floor)
                            .ThenInclude(f => f.House)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tenant == null)
            {
                return null;
            }

            if (tenant.TenantRooms.Any() &&
                !tenant.TenantRooms.Any(tr =>
                    tr.Room.Floor.House.UserId == userId))
            {
                return null;
            }

            return new TenantResponseDto
            {
                Id = tenant.Id,

                FullName = tenant.FullName,

                PhoneNumber = tenant.PhoneNumber,

                MonthlyRent = tenant.MonthlyRent,

                IsActive = tenant.IsActive
            };
        }

        public async Task<List<TenantOverviewResponseDto>>
    GetTenantOverviewAsync(
        int houseId, int userId
    )
        {
            return await _context.Tenants

                .Include(t => t.TenantRooms)
                    .ThenInclude(tr => tr.Room)
                        .ThenInclude(r => r.Floor)

                .Include(t => t.TenantMeters)
                    .ThenInclude(tm => tm.Meter)

                .Where(t =>
                    t.TenantRooms.Any(tr =>
                        tr.Room.Floor.HouseId == houseId
                        && tr.Room.Floor.House.UserId == userId
                    )
                )

                .Select(t => new TenantOverviewResponseDto
                {

                    TenantId = t.Id,


                    TenantName = t.FullName,


                    PhoneNumber = t.PhoneNumber,


                    MonthlyRent = t.MonthlyRent,


                    IsActive = t.IsActive,



                    Rooms = t.TenantRooms

                        .Where(tr =>
                            tr.EndDate == null
                            &&
                            tr.Room.Floor.HouseId == houseId
                            &&
                            tr.Room.Floor.House.UserId == userId
                        )

                        .Select(tr =>
                            tr.Room.RoomNumber
                        )

                        .ToList(),



                    Meters = t.TenantMeters

                        .Where(tm =>
                            tm.EndDate == null
                            &&
                            tm.Meter.House.UserId == userId
                        )

                        .Select(tm =>
                            tm.Meter.MeterNumber
                        )

                        .ToList()

                })

                .ToListAsync();
        }
    }
}
