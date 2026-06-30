using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Models.MasterEntities;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Services
{
    public class MeterService
        : IMeterService
    {
        private readonly
            ApplicationDbContext _context;

        public MeterService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MeterResponseDto>
            CreateMeterAsync(
                CreateMeterRequestDto request, int userId)
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

            var meterExists =
                await _context.Meters
                    .AnyAsync(m =>
                        m.MeterNumber ==
                            request.MeterNumber);

            if (meterExists)
            {
                throw new Exception(
                    "Meter number already exists");
            }

            if (request.InitialReading < 0)
            {
                throw new Exception("Invalid reading");
            }

            var meter = new Meter
            {
                HouseId =
                    request.HouseId,

                MeterNumber =
                    request.MeterNumber,

                MeterType =
                    request.MeterType,

                InitialReading =
                    request.InitialReading,

                IsActive = true
            };

            await _context.Meters
                .AddAsync(meter);

            await _context.SaveChangesAsync();

            return new MeterResponseDto
            {
                Id = meter.Id,

                HouseId =
                    meter.HouseId,

                MeterNumber =
                    meter.MeterNumber,

                MeterType =
                    meter.MeterType,

                InitialReading =
                    meter.InitialReading,

                IsActive =
                    meter.IsActive
            };
        }

        public async Task<List<MeterResponseDto>>
            GetAllMetersAsync(int userId)
        {
            var meters = await _context.Meters
                .Where(m =>
                    m.House.UserId == userId)
                .Select(m =>
                    new MeterResponseDto
                    {
                        Id = m.Id,

                        HouseId =
                            m.HouseId,

                        MeterNumber =
                            m.MeterNumber,

                        MeterType =
                            m.MeterType,

                        InitialReading =
                            m.InitialReading,

                        IsActive =
                            m.IsActive
                    })
                .ToListAsync();

            return meters;
        }

        public async Task<List<MeterResponseDto>>
            GetActiveMetersAsync(int userId)
        {
            var meters = await _context.Meters
                .Where(m =>
                    m.IsActive
                    && m.House.UserId == userId)
                .Select(m =>
                    new MeterResponseDto
                    {
                        Id = m.Id,

                        HouseId =
                            m.HouseId,

                        MeterNumber =
                            m.MeterNumber,

                        MeterType =
                            m.MeterType,

                        InitialReading =
                            m.InitialReading,

                        IsActive =
                            m.IsActive
                    })
                .ToListAsync();

            return meters;
        }

        public async Task<MeterResponseDto>
            GetMeterByIdAsync(
                int meterId, int userId)
        {
            var meter = await _context.Meters
                .FirstOrDefaultAsync(m =>
                    m.Id == meterId
                    && m.House.UserId == userId);

            if (meter == null)
            {
                throw new Exception(
                    "Meter not found");
            }

            return new MeterResponseDto
            {
                Id = meter.Id,

                HouseId =
                    meter.HouseId,

                MeterNumber =
                    meter.MeterNumber,

                MeterType =
                    meter.MeterType,

                InitialReading =
                    meter.InitialReading,

                IsActive =
                    meter.IsActive
            };
        }

        public async Task<List<MeterResponseDto>> GetMetersByHouseAsync(
            int houseId, int userId
        )
        {

            var meters =
                await _context.Meters
                .Where(m =>
                    m.HouseId == houseId
                    && m.House.UserId == userId
                )
                .Select(m =>
                    new MeterResponseDto
                    {
                        Id = m.Id,

                        HouseId = m.HouseId,

                        MeterNumber = m.MeterNumber,

                        MeterType = m.MeterType,

                        InitialReading = m.InitialReading,

                        IsActive = m.IsActive
                    }
                )
                .ToListAsync();


            return meters;

        }
        public async Task<MeterResponseDto>
            UpdateMeterAsync(
                int meterId,
                UpdateMeterRequestDto request, int userId)
        {
            var meter = await _context.Meters
                .FirstOrDefaultAsync(m =>
                    m.Id == meterId
                    && m.House.UserId == userId);

            if (meter == null)
            {
                throw new Exception(
                    "Meter not found");
            }

            var duplicateMeter =
                await _context.Meters
                    .AnyAsync(m =>
                        m.MeterNumber ==
                            request.MeterNumber
                        &&
                        m.Id != meterId);

            if (duplicateMeter)
            {
                throw new Exception(
                    "Meter number already exists");
            }

            meter.MeterNumber =
                request.MeterNumber;

            meter.MeterType =
                request.MeterType;

            meter.InitialReading =
                request.InitialReading;

            meter.IsActive =
                request.IsActive;

            await _context.SaveChangesAsync();

            return new MeterResponseDto
            {
                Id = meter.Id,

                HouseId =
                    meter.HouseId,

                MeterNumber =
                    meter.MeterNumber,

                MeterType =
                    meter.MeterType,

                InitialReading =
                    meter.InitialReading,

                IsActive =
                    meter.IsActive
            };
        }

        public async Task<MeterResponseDto>
            DeactivateMeterAsync(
                int meterId, int userId)
        {
            var meter = await _context.Meters
                .FirstOrDefaultAsync(m =>
                    m.Id == meterId
                    && m.House.UserId == userId);

            if (meter == null)
            {
                throw new Exception(
                    "Meter not found");
            }

            meter.IsActive = false;

            await _context.SaveChangesAsync();

            return new MeterResponseDto
            {
                Id = meter.Id,

                HouseId =
                    meter.HouseId,

                MeterNumber =
                    meter.MeterNumber,

                MeterType =
                    meter.MeterType,

                InitialReading =
                    meter.InitialReading,

                IsActive =
                    meter.IsActive
            };
        }

        public async Task<List<MeterOverviewResponseDto>> GetMeterOverviewByHouseAsync(int houseId, int userId)
        {
            return await _context.Meters


                .Where(m =>
                    m.HouseId == houseId
                    && m.House.UserId == userId
                )


                .Select(m =>
                    new MeterOverviewResponseDto
                    {
                        Id =
                            m.Id,


                        HouseId =
                            m.HouseId,


                        MeterNumber =
                            m.MeterNumber,


                        MeterType =
                            m.MeterType.ToString(),


                        InitialReading =
                            m.InitialReading,


                        IsActive =
                            m.IsActive,


                        AssignedTenants =
                            m.TenantMeters

                                .Where(tm =>
                                    tm.EndDate == null)

                                .Select(tm =>
                                    tm.Tenant.FullName)

                                .ToList()
                    })


                .ToListAsync();
        }
    }
}
