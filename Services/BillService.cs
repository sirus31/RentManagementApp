using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Models.Enums;
using RentManagementApp.Models.TransactionalEntities;

using RentManagementApp.Services.Interfaces;


namespace RentManagementApp.Services
{
    public class BillService
        : IBillService
    {
        private readonly
            ApplicationDbContext _context;


        public BillService(
            ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<BillResponseDto>
            GenerateBillAsync(
                GenerateBillRequestDto request)
        {
            var tenant = await _context.Tenants

                .Include(t => t.TenantRooms)

                    .ThenInclude(tr => tr.Room)

                        .ThenInclude(r => r.Floor)

                            .ThenInclude(f => f.House)

                .FirstOrDefaultAsync(t =>
                    t.Id == request.TenantId);


            if (tenant == null)
            {
                throw new Exception(
                    "Tenant not found");
            }


            var billExists =
                await _context.Bills
                    .AnyAsync(b =>
                        b.TenantId == request.TenantId
                        &&
                        b.BillingYear == request.BillingYear
                        &&
                        b.BillingMonth == request.BillingMonth);


            if (billExists)
            {
                throw new Exception(
                    "Bill already generated for this cycle");
            }


            var activeRoom =
                tenant.TenantRooms
                    .FirstOrDefault(tr =>
                        tr.EndDate == null);


            if (activeRoom == null)
            {
                throw new Exception(
                    "Tenant has no active room");
            }


            var house =
                activeRoom.Room.Floor.House;


            decimal rentAmount =
                tenant.MonthlyRent;


            decimal garbageAmount =
                house.GarbageFee;


            decimal electricityAmount = 0;


            var billDetails =
                new List<BillDetail>();


            billDetails.Add(
                new BillDetail
                {
                    DetailType =
                        BillDetailType.Rent,

                    Description =
                        "Monthly room rent",

                    Amount =
                        rentAmount
                });


            var tenantMeters =
                await _context.TenantMeters

                    .Include(tm => tm.Meter)

                    .Where(tm =>
                        tm.TenantId == tenant.Id
                        &&
                        tm.EndDate == null)

                    .ToListAsync();



            foreach (var tenantMeter in tenantMeters)
            {
                var currentReading =
                    await _context.MeterReadings

                        .Where(mr =>
                            mr.MeterId == tenantMeter.MeterId
                            &&
                            mr.BillingYear == request.BillingYear
                            &&
                            mr.BillingMonth == request.BillingMonth)

                        .OrderByDescending(mr =>
                            mr.ReadingDate)

                        .FirstOrDefaultAsync();


                if (currentReading == null)
                {
                    continue;
                }


                var previousReading =
                    await _context.MeterReadings

                        .Where(mr =>
                            mr.MeterId == tenantMeter.MeterId
                            &&
                            mr.ReadingDate <
                                currentReading.ReadingDate)

                        .OrderByDescending(mr =>
                            mr.ReadingDate)

                        .FirstOrDefaultAsync();



                decimal previousValue =
                    previousReading?.ReadingValue
                    ??
                    tenantMeter.Meter.InitialReading;



                decimal consumption =
                    currentReading.ReadingValue
                    -
                    previousValue;


                if (consumption < 0)
                {
                    throw new Exception(
                        "Invalid meter reading sequence");
                }


                if (tenantMeter.Meter.MeterType
                    == MeterType.Shared)
                {
                    var activeParticipants =
                        await _context.TenantMeters

                            .CountAsync(tm =>
                                tm.MeterId == tenantMeter.MeterId
                                &&
                                tm.EndDate == null);


                    consumption =
                        consumption / activeParticipants;
                }



                decimal meterCharge =
                    consumption
                    *
                    house.ElectricityRate;


                electricityAmount +=
                    meterCharge;



                billDetails.Add(
                    new BillDetail
                    {
                        DetailType =
                            BillDetailType.Electricity,

                        Description =
                            $"Meter {tenantMeter.Meter.MeterNumber}",

                        PreviousReading =
                            previousValue,

                        CurrentReading =
                            currentReading.ReadingValue,

                        UnitsConsumed =
                            consumption,

                        Rate =
                            house.ElectricityRate,

                        Amount =
                            meterCharge
                    });
            }



            billDetails.Add(
                new BillDetail
                {
                    DetailType =
                        BillDetailType.Garbage,

                    Description =
                        "Monthly garbage charge",

                    Amount =
                        garbageAmount
                });



            decimal totalAmount =
                rentAmount
                +
                electricityAmount
                +
                garbageAmount;



            var bill =
                new Bill
                {
                    BillNumber =
                        $"BILL-{request.BillingYear}-{Guid.NewGuid().ToString()[..6]}",

                    TenantId =
                        tenant.Id,

                    BillingYear =
                        request.BillingYear,

                    BillingMonth =
                        request.BillingMonth,

                    RentAmount =
                        rentAmount,

                    ElectricityAmount =
                        electricityAmount,

                    GarbageAmount =
                        garbageAmount,

                    TotalAmount =
                        totalAmount,

                    AmountPaid =
                        0,

                    PaymentStatus =
                        PaymentStatus.Pending,

                    BillStatus =
                        BillStatus.Generated,

                    GeneratedDate =
                        DateTime.UtcNow,

                    BillDetails =
                        billDetails
                };


            await _context.Bills
                .AddAsync(bill);


            await _context.SaveChangesAsync();


            return await GetBillByIdAsync(
                bill.Id);
        }

        public async Task<List<GenerateBillResultDto>>
            GenerateAllBillsAsync(
                GenerateAllBillsRequestDto request)
        {
            var results =
                new List<GenerateBillResultDto>();


            var tenants =
                await _context.Tenants

                    .Include(t => t.TenantRooms)

                    .Where(t =>
                        t.TenantRooms.Any(tr =>
                            tr.EndDate == null))

                    .ToListAsync();


            foreach (var tenant in tenants)
            {
                var existingBill =
                    await _context.Bills
                        .FirstOrDefaultAsync(b =>
                            b.TenantId == tenant.Id
                            &&
                            b.BillingYear == request.BillingYear
                            &&
                            b.BillingMonth == request.BillingMonth);


                if (existingBill != null)
                {
                    results.Add(
                        new GenerateBillResultDto
                        {
                            TenantId =
                                tenant.Id,

                            TenantName =
                                tenant.FullName,

                            Status =
                                "AlreadyExists",

                            Message =
                                "Bill already generated",

                            BillId =
                                existingBill.Id
                        });


                    continue;
                }



                try
                {
                    var bill =
                        await GenerateBillAsync(
                            new GenerateBillRequestDto
                            {
                                TenantId =
                                    tenant.Id,

                                BillingYear =
                                    request.BillingYear,

                                BillingMonth =
                                    request.BillingMonth
                            });


                    results.Add(
                        new GenerateBillResultDto
                        {
                            TenantId =
                                tenant.Id,

                            TenantName =
                                tenant.FullName,

                            Status =
                                "Generated",

                            Message =
                                "Bill generated successfully",

                            BillId =
                                bill.Id
                        });
                }


                catch (Exception ex)
                {
                    results.Add(
                        new GenerateBillResultDto
                        {
                            TenantId =
                                tenant.Id,

                            TenantName =
                                tenant.FullName,

                            Status =
                                "Failed",

                            Message =
                                ex.Message
                        });
                }
            }


            return results;
        }
        
        public async Task<List<BillResponseDto>>
            GetTenantBillsAsync(
                int tenantId)
        {
            var bills =
                await _context.Bills

                    .Include(b => b.Tenant)

                    .Include(b => b.BillDetails)

                    .Where(b =>
                        b.TenantId == tenantId)

                    .OrderByDescending(b =>
                        b.GeneratedDate)

                    .Select(b =>
                        new BillResponseDto
                        {
                            Id =
                                b.Id,

                            BillNumber =
                                b.BillNumber,

                            TenantName =
                                b.Tenant.FullName,

                            BillingYear =
                                b.BillingYear,

                            BillingMonth =
                                b.BillingMonth,

                            RentAmount =
                                b.RentAmount,

                            ElectricityAmount =
                                b.ElectricityAmount,

                            GarbageAmount =
                                b.GarbageAmount,

                            TotalAmount =
                                b.TotalAmount,

                            AmountPaid =
                                b.AmountPaid,

                            PaymentStatus =
                                b.PaymentStatus,

                            BillStatus =
                                b.BillStatus,

                            GeneratedDate =
                                b.GeneratedDate,

                            BillDetails =
                                b.BillDetails
                                    .Select(bd =>
                                        new BillDetailResponseDto
                                        {
                                            DetailType =
                                                bd.DetailType,

                                            Description =
                                                bd.Description,

                                            PreviousReading =
                                                bd.PreviousReading,

                                            CurrentReading =
                                                bd.CurrentReading,

                                            UnitsConsumed =
                                                bd.UnitsConsumed,

                                            Rate =
                                                bd.Rate,

                                            Amount =
                                                bd.Amount
                                        })
                                    .ToList()
                        })

                    .ToListAsync();


            return bills;
        }

        public async Task<List<BillResponseDto>>
            GetAllBillsAsync()
        {
            var bills =
                await _context.Bills

                    .Include(b => b.Tenant)

                    .Include(b => b.BillDetails)

                    .OrderByDescending(b =>
                        b.GeneratedDate)

                    .Select(b =>
                        new BillResponseDto
                        {
                            Id =
                                b.Id,

                            BillNumber =
                                b.BillNumber,

                            TenantName =
                                b.Tenant.FullName,

                            BillingYear =
                                b.BillingYear,

                            BillingMonth =
                                b.BillingMonth,

                            RentAmount =
                                b.RentAmount,

                            ElectricityAmount =
                                b.ElectricityAmount,

                            GarbageAmount =
                                b.GarbageAmount,

                            TotalAmount =
                                b.TotalAmount,

                            AmountPaid =
                                b.AmountPaid,

                            PaymentStatus =
                                b.PaymentStatus,

                            BillStatus =
                                b.BillStatus,

                            GeneratedDate =
                                b.GeneratedDate,

                            BillDetails =
                                b.BillDetails
                                    .Select(bd =>
                                        new BillDetailResponseDto
                                        {
                                            DetailType =
                                                bd.DetailType,

                                            Description =
                                                bd.Description,

                                            PreviousReading =
                                                bd.PreviousReading,

                                            CurrentReading =
                                                bd.CurrentReading,

                                            UnitsConsumed =
                                                bd.UnitsConsumed,

                                            Rate =
                                                bd.Rate,

                                            Amount =
                                                bd.Amount
                                        })
                                    .ToList()
                        })

                    .ToListAsync();


            return bills;
        }

        public async Task<BillResponseDto>
            GetBillByIdAsync(
                int billId)
        {
            var bill =
                await _context.Bills

                    .Include(b => b.Tenant)

                    .Include(b => b.BillDetails)

                    .FirstOrDefaultAsync(b =>
                        b.Id == billId);


            if (bill == null)
            {
                throw new Exception(
                    "Bill not found");
            }


            return new BillResponseDto
            {
                Id =
                    bill.Id,

                BillNumber =
                    bill.BillNumber,

                TenantName =
                    bill.Tenant.FullName,

                BillingYear =
                    bill.BillingYear,

                BillingMonth =
                    bill.BillingMonth,

                RentAmount =
                    bill.RentAmount,

                ElectricityAmount =
                    bill.ElectricityAmount,

                GarbageAmount =
                    bill.GarbageAmount,

                TotalAmount =
                    bill.TotalAmount,

                AmountPaid =
                    bill.AmountPaid,

                PaymentStatus =
                    bill.PaymentStatus,

                BillStatus =
                    bill.BillStatus,

                GeneratedDate =
                    bill.GeneratedDate,

                BillDetails =
                    bill.BillDetails
                        .Select(bd =>
                            new BillDetailResponseDto
                            {
                                DetailType =
                                    bd.DetailType,

                                Description =
                                    bd.Description,

                                PreviousReading =
                                    bd.PreviousReading,

                                CurrentReading =
                                    bd.CurrentReading,

                                UnitsConsumed =
                                    bd.UnitsConsumed,

                                Rate =
                                    bd.Rate,

                                Amount =
                                    bd.Amount
                            })
                        .ToList()
            };
        }

        public async Task<BillResponseDto>
            FinalizeBillAsync(
                int billId)
        {
            var bill =
                await _context.Bills
                    .FirstOrDefaultAsync(b =>
                        b.Id == billId);


            if (bill == null)
            {
                throw new Exception(
                    "Bill not found");
            }


            if (bill.BillStatus ==
                BillStatus.Cancelled)
            {
                throw new Exception(
                    "Cancelled bill cannot be finalized");
            }


            if (bill.BillStatus ==
                BillStatus.Finalized)
            {
                throw new Exception(
                    "Bill already finalized");
            }


            bill.BillStatus =
                BillStatus.Finalized;


            bill.FinalizedDate =
                DateTime.UtcNow;


            await _context.SaveChangesAsync();


            return await GetBillByIdAsync(
                bill.Id);
        }

        public async Task<BillResponseDto>
            CancelBillAsync(
                int billId)
        {
            var bill =
                await _context.Bills
                    .FirstOrDefaultAsync(b =>
                        b.Id == billId);


            if (bill == null)
            {
                throw new Exception(
                    "Bill not found");
            }


            if (bill.AmountPaid > 0)
            {
                throw new Exception(
                    "Paid bill cannot be cancelled");
            }


            bill.BillStatus =
                BillStatus.Cancelled;


            await _context.SaveChangesAsync();


            return await GetBillByIdAsync(
                bill.Id);
        }
    }
}