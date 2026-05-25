using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Models.Enums;
using RentManagementApp.Models.RelationshipEntities;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Services
{
    public class MeterAssignmentService
        : IMeterAssignmentService
    {
        private readonly ApplicationDbContext _context;

        public MeterAssignmentService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MeterAssignmentResponseDto>
            AssignMeterAsync(
                AssignMeterRequestDto request)
        {
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t =>
                    t.Id == request.TenantId);

            if (tenant == null)
            {
                throw new Exception(
                    "Tenant not found");
            }

            var meter = await _context.Meters
                .FirstOrDefaultAsync(m =>
                    m.Id == request.MeterId);

            if (meter == null)
            {
                throw new Exception(
                    "Meter not found");
            }

            if (!meter.IsActive)
            {
                throw new Exception(
                    "Meter is inactive");
            }

            var tenantHasActiveRoom =
                await _context.TenantRooms
                    .AnyAsync(tr =>
                        tr.TenantId ==
                            request.TenantId
                        &&
                        tr.EndDate == null);

            if (!tenantHasActiveRoom)
            {
                throw new Exception(
                    "Tenant has no active rooms");
            }

            var duplicateAssignment =
                await _context.TenantMeters
                    .AnyAsync(tm =>
                        tm.TenantId ==
                            request.TenantId
                        &&
                        tm.MeterId ==
                            request.MeterId
                        &&
                        tm.EndDate == null);

            if (duplicateAssignment)
            {
                throw new Exception(
                    "Tenant already assigned to this meter");
            }

            if (meter.MeterType ==
                MeterType.Private)
            {
                var privateMeterOccupied =
                    await _context.TenantMeters
                        .AnyAsync(tm =>
                            tm.MeterId ==
                                request.MeterId
                            &&
                            tm.EndDate == null);

                if (privateMeterOccupied)
                {
                    throw new Exception(
                        "Private meter already assigned");
                }
            }

            var tenantMeter =
                new TenantMeter
                {
                    TenantId =
                        request.TenantId,

                    MeterId =
                        request.MeterId,

                    StartDate =
                        DateTime.SpecifyKind(
                            request.StartDate,
                            DateTimeKind.Utc)
                };

            await _context.TenantMeters
                .AddAsync(tenantMeter);

            await _context.SaveChangesAsync();

            return new MeterAssignmentResponseDto
            {
                TenantMeterId =
                    tenantMeter.Id,

                TenantId =
                    tenant.Id,

                TenantName =
                    tenant.FullName,

                MeterId =
                    meter.Id,

                MeterNumber =
                    meter.MeterNumber,

                MeterType =
                    meter.MeterType.ToString(),

                StartDate =
                    tenantMeter.StartDate,

                EndDate =
                    tenantMeter.EndDate
            };
        }

        public async Task
            RemoveMeterAssignmentAsync(
                RemoveMeterAssignmentRequestDto request)
        {
            var tenantMeter =
                await _context.TenantMeters
                    .FirstOrDefaultAsync(tm =>
                        tm.Id ==
                            request.TenantMeterId);

            if (tenantMeter == null)
            {
                throw new Exception(
                    "Assignment not found");
            }

            if (tenantMeter.EndDate != null)
            {
                throw new Exception(
                    "Assignment already removed");
            }

            tenantMeter.EndDate =
                DateTime.SpecifyKind(
                    request.EndDate,
                    DateTimeKind.Utc);

            await _context.SaveChangesAsync();
        }

        public async Task<
            List<MeterAssignmentResponseDto>>
            GetActiveAssignmentsAsync()
        {
            return await _context.TenantMeters
                .Include(tm => tm.Tenant)
                .Include(tm => tm.Meter)

                .Where(tm =>
                    tm.EndDate == null)

                .Select(tm =>
                    new MeterAssignmentResponseDto
                    {
                        TenantMeterId =
                            tm.Id,

                        TenantId =
                            tm.TenantId,

                        TenantName =
                            tm.Tenant.FullName,

                        MeterId =
                            tm.MeterId,

                        MeterNumber =
                            tm.Meter.MeterNumber,

                        MeterType =
                            tm.Meter.MeterType
                                .ToString(),

                        StartDate =
                            tm.StartDate,

                        EndDate =
                            tm.EndDate
                    })
                .ToListAsync();
        }
    }
}