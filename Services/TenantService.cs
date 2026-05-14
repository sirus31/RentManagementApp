using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Models.MasterEntities;

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
    }
}