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
            CreateTenantRequestDto request)
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


        public async Task<List<TenantResponseDto>> GetAllTenantsAsync()
        {
            var tenants = await _context.Tenants
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

        public async Task<List<TenantRoomSummaryResponseDto>> GetTenantsWithActiveRoomsAsync()
        {
            var tenants = await _context.Tenants
                .Include(t => t.TenantRooms)
                    .ThenInclude(tr => tr.Room)

                .Select(t => new TenantRoomSummaryResponseDto
                {
                    Id = t.Id,

                    FullName = t.FullName,

                    RoomNumbers = t.TenantRooms

                        .Where(tr => tr.EndDate == null)

                        .Select(tr => tr.Room.RoomNumber)

                        .ToList()
                })
                .ToListAsync();

            return tenants;
        }

        public async Task<List<TenantRoomSummaryResponseDto>> GetTenantOccupancyHistoryAsync()
        {
            var tenants = await _context.Tenants
                .Include(t => t.TenantRooms)
                    .ThenInclude(tr => tr.Room)

                .Select(t => new TenantRoomSummaryResponseDto
                {
                    Id = t.Id,

                    FullName = t.FullName,

                    RoomNumbers = t.TenantRooms

                        .Select(tr => tr.Room.RoomNumber)

                        .ToList()
                })
                .ToListAsync();

            return tenants;
        }
        public async Task AssignRoomAsync(AssignRoomRequestDto request)
        {
            var tenantExists = await _context.Tenants
                .AnyAsync(t => t.Id == request.TenantId);

            if (!tenantExists)
            {
                throw new Exception("Tenant not found");
            }

            var roomExists = await _context.Rooms
                .AnyAsync(r => r.Id == request.RoomId);

            if (!roomExists)
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

        public async Task<VacateTenantResponseDto> VacateTenantAsync(int tenantId)
        {
            var tenant =
                await _context.Tenants

                    .Include(t => t.TenantRooms)

                    .Include(t => t.TenantMeters)

                    .Include(t => t.Bills)

                    .FirstOrDefaultAsync(t =>
                        t.Id == tenantId);


            if (tenant == null)
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

        public async Task<TenantResponseDto?> GetTenantByIdAsync(int id)
        {
            var tenant = await _context.Tenants

                .FirstOrDefaultAsync(t => t.Id == id);



            if (tenant == null)
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
    }
}