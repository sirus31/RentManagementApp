using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

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

        public async Task<List<TenantWithRoomsResponseDto>> GetTenantsWithRoomsAsync()
        {
            var tenants = await _context.Tenants
                .Include(t => t.TenantRooms)
                    .ThenInclude(tr => tr.Room)
                .Select(t => new TenantWithRoomsResponseDto
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
    }
}