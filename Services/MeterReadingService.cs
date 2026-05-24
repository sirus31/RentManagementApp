using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Models.TransactionalEntities;

using RentManagementApp.Services.Interfaces;

namespace RentManagementApp.Services
{
    public class MeterReadingService
        : IMeterReadingService
    {
        private readonly
            ApplicationDbContext _context;

        public MeterReadingService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<
            MeterReadingResponseDto>
            AddReadingAsync(
                CreateMeterReadingRequestDto request)
        {
            var meter = await _context.Meters
                .FirstOrDefaultAsync(m =>
                    m.Id == request.MeterId);

            if (meter == null)
            {
                throw new Exception(
                    "Meter not found");
            }

            var duplicateCycleExists =
                await _context.MeterReadings
                    .AnyAsync(r =>
                        r.MeterId ==
                            request.MeterId
                        &&
                        r.BillingYear ==
                            request.BillingYear
                        &&
                        r.BillingMonth ==
                            request.BillingMonth);

            if (duplicateCycleExists)
            {
                throw new Exception(
                    "Reading already exists for this billing cycle");
            }

            var latestReading =
                await _context.MeterReadings

                    .Where(r =>
                        r.MeterId ==
                            request.MeterId)

                    .OrderByDescending(r =>
                        r.ReadingDate)

                    .FirstOrDefaultAsync();

            if (latestReading != null)
            {
                if (request.ReadingValue <
                    latestReading.ReadingValue)
                {
                    throw new Exception(
                        "Reading cannot be less than previous reading");
                }

                if (request.ReadingDate <
                    latestReading.ReadingDate)
                {
                    throw new Exception(
                        "Reading date cannot go backward");
                }
            }

            var meterReading =
                new MeterReading
                {
                    MeterId =
                        request.MeterId,

                    ReadingValue =
                        request.ReadingValue,

                    ReadingDate =
                        DateTime.SpecifyKind(
                            request.ReadingDate,
                            DateTimeKind.Utc),

                    BillingYear =
                        request.BillingYear,

                    BillingMonth =
                        request.BillingMonth,

                    Notes =
                        request.Notes
                };

            await _context.MeterReadings
                .AddAsync(meterReading);

            await _context.SaveChangesAsync();

            return new MeterReadingResponseDto
            {
                Id =
                    meterReading.Id,

                MeterId =
                    meter.Id,

                MeterNumber =
                    meter.MeterNumber,

                ReadingValue =
                    meterReading.ReadingValue,

                ReadingDate =
                    meterReading.ReadingDate,

                BillingYear =
                    meterReading.BillingYear,

                BillingMonth =
                    meterReading.BillingMonth
                        .ToString(),

                Notes =
                    meterReading.Notes
            };
        }

        public async Task<
            List<MeterReadingResponseDto>>
            GetMeterReadingsAsync(
                int meterId)
        {
            return await _context.MeterReadings

                .Include(r => r.Meter)

                .Where(r =>
                    r.MeterId == meterId)

                .OrderBy(r =>
                    r.ReadingDate)

                .Select(r =>
                    new MeterReadingResponseDto
                    {
                        Id = r.Id,

                        MeterId = r.MeterId,

                        MeterNumber =
                            r.Meter.MeterNumber,

                        ReadingValue =
                            r.ReadingValue,

                        ReadingDate =
                            r.ReadingDate,

                        BillingYear =
                            r.BillingYear,

                        BillingMonth =
                            r.BillingMonth
                                .ToString(),

                        Notes =
                            r.Notes
                    })
                .ToListAsync();
        }

        public async Task<
            MeterReadingResponseDto?>
            GetLatestReadingAsync(
                int meterId)
        {
            var reading =
                await _context.MeterReadings

                    .Include(r => r.Meter)

                    .Where(r =>
                        r.MeterId == meterId)

                    .OrderByDescending(r =>
                        r.ReadingDate)

                    .FirstOrDefaultAsync();

            if (reading == null)
            {
                return null;
            }

            return new MeterReadingResponseDto
            {
                Id = reading.Id,

                MeterId = reading.MeterId,

                MeterNumber =
                    reading.Meter.MeterNumber,

                ReadingValue =
                    reading.ReadingValue,

                ReadingDate =
                    reading.ReadingDate,

                BillingYear =
                    reading.BillingYear,

                BillingMonth =
                    reading.BillingMonth
                        .ToString(),

                Notes =
                    reading.Notes
            };
        }
    }
}