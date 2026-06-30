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
                GenerateBillRequestDto request, int userId)
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

            var hasHouseAccess = tenant.TenantRooms.Any(tr =>
                tr.Room.Floor.House.UserId == userId);

            if (!hasHouseAccess)
            {
                throw new Exception(
                    "Tenant not found");
            }


            var billExists =
                await _context.Bills
                    .AnyAsync(b =>
                        b.TenantId == request.TenantId
                        &&
                        b.BillCycle.BillingYear == request.BillingYear
                        &&
                        b.BillCycle.BillingMonth == request.BillingMonth);


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

                    BillCycle =
                        new BillCycle
                        {
                            HouseId =
                                house.Id,

                            BillingYear =
                                request.BillingYear,

                            BillingMonth =
                                request.BillingMonth,

                            CycleType =
                                BillCycleType.Monthly,

                            Status =
                                BillStatus.Generated,

                            GeneratedDate =
                                DateTime.UtcNow
                        },

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
                        PaymentStatus.Unpaid,

                    BillDetails =
                        billDetails
                };


            await _context.Bills
                .AddAsync(bill);


            await _context.SaveChangesAsync();


            return await GetBillByIdAsync(
                bill.Id, userId);
        }

        public async Task<List<GenerateBillResultDto>>
            GenerateAllBillsAsync(
                GenerateAllBillsRequestDto request, int userId)
        {
            var results =
                new List<GenerateBillResultDto>();


            var tenants =
                await _context.Tenants

                    .Include(t => t.TenantRooms)
                        .ThenInclude(tr => tr.Room)
                            .ThenInclude(r => r.Floor)

                    .Where(t =>
                        t.TenantRooms.Any(tr =>
                            tr.EndDate == null
                            && tr.Room.Floor.House.UserId == userId))

                    .ToListAsync();


            foreach (var tenant in tenants)
            {
                var existingBill =
                    await _context.Bills
                        .FirstOrDefaultAsync(b =>
                            b.TenantId == tenant.Id
                            &&
                            b.BillCycle.BillingYear == request.BillingYear
                            &&
                            b.BillCycle.BillingMonth == request.BillingMonth);


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
                            }, userId);


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
                int tenantId, int userId)
        {
            var bills =
                await _context.Bills

                    .Include(b => b.Tenant)

                    .Include(b => b.BillCycle)

                    .Include(b => b.BillDetails)

                    .Where(b =>
                        b.TenantId == tenantId
                        && b.BillCycle.House.UserId == userId)

                    .OrderByDescending(b =>
                        b.BillCycle.GeneratedDate)

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
                                b.BillCycle.BillingYear,

                            BillingMonth =
                                b.BillCycle.BillingMonth,

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
                                b.BillCycle.Status,

                            GeneratedDate =
                                b.BillCycle.GeneratedDate,

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
            GetAllBillsAsync(int userId)
        {
            var bills =
                await _context.Bills

                    .Include(b => b.Tenant)

                    .Include(b => b.BillCycle)

                    .Include(b => b.BillDetails)

                    .Where(b =>
                        b.BillCycle.House.UserId == userId)

                    .OrderByDescending(b =>
                        b.BillCycle.GeneratedDate)

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
                                b.BillCycle.BillingYear,

                            BillingMonth =
                                b.BillCycle.BillingMonth,

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
                                b.BillCycle.Status,

                            GeneratedDate =
                                b.BillCycle.GeneratedDate,

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
                int billId, int userId)
        {
            var bill =
                await _context.Bills

                    .Include(b => b.Tenant)

                    .Include(b => b.BillCycle)

                    .Include(b => b.BillDetails)

                    .FirstOrDefaultAsync(b =>
                        b.Id == billId
                        && b.BillCycle.House.UserId == userId);


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
                    bill.BillCycle.BillingYear,

                BillingMonth =
                    bill.BillCycle.BillingMonth,

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
                    bill.BillCycle.Status,

                GeneratedDate =
                    bill.BillCycle.GeneratedDate,

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
                int billId, int userId)
        {
            var bill =
                await _context.Bills
                    .Include(b => b.BillCycle)
                    .FirstOrDefaultAsync(b =>
                        b.Id == billId
                        && b.BillCycle.House.UserId == userId);


            if (bill == null)
            {
                throw new Exception(
                    "Bill not found");
            }


            if (bill.BillCycle.Status ==
                BillStatus.Cancelled)
            {
                throw new Exception(
                    "Cancelled bill cannot be finalized");
            }


            if (bill.BillCycle.Status ==
                BillStatus.Finalized)
            {
                throw new Exception(
                    "Bill already finalized");
            }


            bill.BillCycle.Status =
                BillStatus.Finalized;


            bill.BillCycle.FinalizedDate =
                DateTime.UtcNow;


            await _context.SaveChangesAsync();


            return await GetBillByIdAsync(
                bill.Id, userId);
        }

        public async Task<BillResponseDto>
            CancelBillAsync(
                int billId, int userId)
        {
            var bill =
                await _context.Bills
                    .Include(b => b.BillCycle)
                    .FirstOrDefaultAsync(b =>
                        b.Id == billId
                        && b.BillCycle.House.UserId == userId);


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


            bill.BillCycle.Status =
                BillStatus.Cancelled;


            await _context.SaveChangesAsync();


            return await GetBillByIdAsync(
                bill.Id, userId);
        }

        public async Task<List<BillCycleOverviewResponseDto>> GetBillCyclesByHouseAsync(int houseId, int userId)
        {
            var houseExists = await _context.Houses
                .AnyAsync(h =>
                    h.Id == houseId
                    && h.UserId == userId);

            if (!houseExists)
            {
                throw new Exception("House not found");
            }

            var cycles =
                await _context.BillCycles

                    .Include(bc =>
                        bc.Bills)

                        .ThenInclude(b =>
                            b.Tenant)

                            .ThenInclude(t =>
                                t.TenantRooms)

                                .ThenInclude(tr =>
                                    tr.Room)

                    .Where(bc =>
                        bc.HouseId == houseId)

                    .OrderByDescending(bc =>
                        bc.GeneratedDate)

                    .Select(bc =>
                        new BillCycleOverviewResponseDto
                        {
                            Id =
                                bc.Id,


                            BillingYear =
                                bc.BillingYear,


                            BillingMonth =
                                bc.BillingMonth,


                            CycleType =
                                bc.CycleType,


                            Status =
                                bc.Status,


                            GeneratedDate =
                                bc.GeneratedDate,




                            TotalBills =
                                bc.Bills.Count,



                            TotalAmount =
                                bc.Bills.Sum(
                                    b => b.TotalAmount),



                            PaidAmount =
                                bc.Bills.Sum(
                                    b => b.AmountPaid),



                            PendingAmount =
                                bc.Bills.Sum(
                                    b =>
                                        b.TotalAmount
                                        -
                                        b.AmountPaid),



                            PaidCount =
                                bc.Bills.Count(
                                    b =>
                                        b.PaymentStatus
                                        ==
                                        PaymentStatus.Paid),



                            PartialCount =
                                bc.Bills.Count(
                                    b =>
                                        b.PaymentStatus
                                        ==
                                        PaymentStatus.Partial),



                            PendingCount =
                                bc.Bills.Count(
                                    b =>
                                        b.PaymentStatus
                                        ==
                                        PaymentStatus.Unpaid),




                            Bills =
                                bc.Bills.Select(
                                    b =>
                                        new BillCycleTenantBillDto
                                        {
                                            BillId =
                                                b.Id,


                                            TenantName =
                                                b.Tenant.FullName,


                                            Rooms =
                                                b.Tenant.TenantRooms

                                                    .Where(tr =>
                                                        tr.EndDate == null)

                                                    .Select(tr =>
                                                        tr.Room.RoomNumber)

                                                    .ToList(),



                                            RentAmount =
                                                b.RentAmount,


                                            ElectricityAmount =
                                                b.ElectricityAmount,


                                            GarbageAmount =
                                                b.GarbageAmount,


                                            ExtraChargeAmount =
                                                b.ExtraChargeAmount,


                                            PreviousDueAmount =
                                                b.PreviousDueAmount,


                                            TotalAmount =
                                                b.TotalAmount,


                                            AmountPaid =
                                                b.AmountPaid,


                                            PendingAmount =
                                                b.TotalAmount
                                                -
                                                b.AmountPaid,


                                            PaymentStatus =
                                                b.PaymentStatus
                                        })

                                .ToList()
                        })


                    .ToListAsync();


            return cycles;
        }

        public async Task<BillFullDetailResponseDto?> GetBillDetailsAsync(
    int billId, int userId)
        {
            var bill = await _context.Bills


                .Include(bill =>
                    bill.Tenant)

                    .ThenInclude(tenant =>
                        tenant.TenantRooms)

                        .ThenInclude(tenantRoom =>
                            tenantRoom.Room)

                            .ThenInclude(room => room.Floor)


                .Include(bill =>
                    bill.BillCycle)
                        .ThenInclude(bc => bc.House)


                .Include(bill =>
                    bill.BillDetails)

                .Include(bill =>
                    bill.Payments)


                .FirstOrDefaultAsync(
                    bill =>
                        bill.Id == billId
                        && bill.BillCycle.House.UserId == userId
                );


            if (bill == null)
            {
                return null;
            }


            return new BillFullDetailResponseDto
            {
                BillId =
                    bill.Id,


                BillNumber =
                    bill.BillNumber,


                TenantName =
                    bill.Tenant.FullName,


                Rooms =
                    bill.Tenant.TenantRooms

                        .Where(tenantRoom =>
                            tenantRoom.EndDate == null)

                        .Select(tenantRoom =>
                            tenantRoom.Room.RoomNumber)

                        .ToList(),



                BillingMonth =
                    bill.BillCycle.BillingMonth.ToString(),


                BillingYear =
                    bill.BillCycle.BillingYear,



                RentAmount =
                    bill.RentAmount,


                ElectricityAmount =
                    bill.ElectricityAmount,


                GarbageAmount =
                    bill.GarbageAmount,


                ExtraChargeAmount =
                    bill.ExtraChargeAmount,


                PreviousDueAmount =
                    bill.PreviousDueAmount,


                TotalAmount =
                    bill.TotalAmount,


                AmountPaid =
                    bill.AmountPaid,


                PendingAmount =
                    bill.TotalAmount
                    -
                    bill.AmountPaid,


                Details =
                    bill.BillDetails

                        .Select(detail =>
                            new BillDetailResponseDto
                            {
                                DetailType =
                                    detail.DetailType,


                                Description =
                                    detail.Description,


                                PreviousReading =
                                    detail.PreviousReading,


                                CurrentReading =
                                    detail.CurrentReading,


                                UnitsConsumed =
                                    detail.UnitsConsumed,


                                TenantUnits =
                                    detail.TenantUnits,


                                SharedTenantCount =
                                    detail.SharedTenantCount,


                                Rate =
                                    detail.Rate,


                                Amount =
                                    detail.Amount
                            })

                        .ToList(),


                Payments =
                    bill.Payments

                        .OrderByDescending(payment =>
                            payment.PaymentDate)

                        .Select(payment =>
                            new PaymentResponseDto
                            {
                                Id =
                                    payment.Id,


                                BillId =
                                    payment.BillId,


                                BillNumber =
                                    bill.BillNumber,


                                Amount =
                                    payment.Amount,


                                PaymentDate =
                                    payment.PaymentDate,


                                PaymentMode =
                                    payment.PaymentMode,


                                Notes =
                                    payment.Notes
                            })

                        .ToList()
            };
        }

        public async Task<GenerateBillInfoResponseDto> GetGenerateBillInfoAsync(int houseId, int userId)
        {
            var house =
                await _context.Houses

                    .Include(house =>
                        house.Floors)

                        .ThenInclude(floor =>
                            floor.Rooms)

                            .ThenInclude(room =>
                                room.TenantRooms)

                                .ThenInclude(tenantRoom =>
                                    tenantRoom.Tenant)

                                    .ThenInclude(tenant =>
                                        tenant.TenantMeters)

                                        .ThenInclude(tenantMeter =>
                                            tenantMeter.Meter)


                    .Include(house =>
                        house.Meters)


                    .FirstOrDefaultAsync(
                        house =>
                            house.Id == houseId
                            && house.UserId == userId
                    );


            if (house == null)
            {
                throw new Exception(
                    "House not found"
                );
            }



            var tenants =
                house.Floors

                    .SelectMany(floor =>
                        floor.Rooms)

                    .SelectMany(room =>
                        room.TenantRooms)

                    .Where(tenantRoom =>
                        tenantRoom.EndDate == null)

                    .GroupBy(tenantRoom =>
                        tenantRoom.Tenant)

                    .Select(group =>
                        new GenerateBillTenantDto
                        {
                            TenantId =
                                group.Key.Id,


                            TenantName =
                                group.Key.FullName,


                            Rooms =
                                group

                                    .Select(tenantRoom =>
                                        tenantRoom.Room.RoomNumber)

                                    .ToList(),


                            MonthlyRent =
                                group.Key.MonthlyRent,


                            Meters =
                                group.Key.TenantMeters

                                    .Select(tenantMeter =>
                                        tenantMeter.Meter.MeterNumber)

                                    .ToList()
                        })

                    .ToList();




            var meters =
                house.Meters

                    .Select(meter =>
                        new GenerateBillMeterDto
                        {
                            MeterId =
                                meter.Id,


                            MeterNumber =
                                meter.MeterNumber,


                            MeterType =
                                meter.MeterType.ToString(),


                            PreviousReading =
                                _context.MeterReadings

                                    .Where(reading =>
                                        reading.MeterId
                                        ==
                                        meter.Id)


                                    .OrderByDescending(reading =>
                                        reading.ReadingDate)


                                    .Select(reading =>
                                        (decimal?)reading.ReadingValue)


                                    .FirstOrDefault()

                                    ??

                                    meter.InitialReading
                        })


                    .ToList();

            var allBills =
            await _context.Bills

                .Include(bill =>
                    bill.Tenant)

                .Include(bill =>
                    bill.BillCycle)

                .Where(bill =>
                    bill.BillCycle.HouseId == houseId)

                .ToListAsync();



            var previousDues =
                allBills

                    .OrderByDescending(bill =>
                        bill.BillCycle.BillingYear)

                    .ThenByDescending(bill =>
                        bill.BillCycle.BillingMonth)

                    .GroupBy(bill =>
                        bill.TenantId)

                    .Select(group =>
                        group.First())

                    .Where(bill =>
                        bill.TotalAmount
                        -
                        bill.AmountPaid
                        >
                        0)

                    .Select(bill =>
                        new GenerateBillPreviousDueDto
                        {
                            TenantId =
                                bill.TenantId,


                            TenantName =
                                bill.Tenant.FullName,


                            BillingMonth =
                                bill.BillCycle.BillingMonth.ToString(),


                            BillingYear =
                                bill.BillCycle.BillingYear,


                            DueAmount =
                                bill.TotalAmount
                                -
                                bill.AmountPaid
                        })

                    .ToList();

            return new GenerateBillInfoResponseDto
            {
                House =
                    new GenerateBillHouseDto
                    {
                        Id =
                            house.Id,


                        Name =
                            house.Name,


                        Address =
                            house.Address,


                        ElectricityRate =
                            house.ElectricityRate,


                        GarbageFee =
                            house.GarbageFee
                    },


                Tenants =
                    tenants,


                Meters =
                    meters,

                PreviousDues = previousDues
            };
        }

        public async Task<List<GenerateBillResultDto>> GenerateMonthlyBillAsync(GenerateMonthlyBillRequestDto request, int userId)
        {
            var results =
                new List<GenerateBillResultDto>();

            var houseExists = await _context.Houses
                .AnyAsync(h =>
                    h.Id == request.HouseId
                    && h.UserId == userId);

            if (!houseExists)
            {
                throw new Exception("House not found");
            }

            var existingCycle =
                await _context.BillCycles

                    .AnyAsync(bc =>
                        bc.HouseId == request.HouseId
                        &&
                        bc.BillingYear == request.BillingYear
                        &&
                        bc.BillingMonth == request.BillingMonth);

            if (existingCycle)
            {
                throw new Exception(
                    "Bills already generated for this month"
                );
            }

            foreach (var reading in request.MeterReadings)
            {
                var meter =
                    await _context.Meters
                        .FirstOrDefaultAsync(m =>
                            m.Id == reading.MeterId);

                if (meter == null || meter.House.UserId != userId)
                {
                    throw new Exception(
                        $"Meter {reading.MeterId} not found");
                }

                var meterReading =
                    new MeterReading
                    {
                        MeterId =
                            reading.MeterId,


                        ReadingValue =
                            reading.ReadingValue,


                        BillingMonth =
                            request.BillingMonth,


                        BillingYear =
                            request.BillingYear,


                        ReadingDate =
                            DateTime.UtcNow,


                        Notes = $"Auto generated bill reading - {request.BillingMonth} {request.BillingYear}"
                    };


                await _context.MeterReadings
                    .AddAsync(meterReading);
            }
            var billCycle =
                new BillCycle
                {
                    HouseId =
                        request.HouseId,


                    BillingMonth =
                        request.BillingMonth,


                    BillingYear =
                        request.BillingYear,


                    CycleType =
                        BillCycleType.Monthly,


                    Status =
                        BillStatus.Generated,


                    GeneratedDate =
                        DateTime.UtcNow
                };

            await _context.BillCycles
                .AddAsync(billCycle);

            await _context.SaveChangesAsync();

            var tenants =
                await _context.Tenants

                    .Include(t => t.TenantRooms)

                        .ThenInclude(tr => tr.Room)
                            .ThenInclude(r => r.Floor)

                    .Where(t =>
                        t.TenantRooms.Any(tr =>
                            tr.EndDate == null
                            &&
                            tr.Room.Floor.HouseId
                                == request.HouseId
                            &&
                            tr.Room.Floor.House.UserId == userId))

                    .ToListAsync();

            foreach (var tenant in tenants)
            {
                try
                {
                    decimal rentAmount =
                        tenant.MonthlyRent;

                    var house =
                        await _context.Houses

                            .FirstAsync(h =>
                                h.Id == request.HouseId);

                    decimal garbageAmount =
                        house.GarbageFee;

                    decimal electricityAmount =
                        0;

                    decimal extraChargeAmount =
                        0;

                    var latestBill =
                        await _context.Bills

                            .Include(b =>
                                b.BillCycle)

                            .Where(b =>
                                b.TenantId == tenant.Id)

                            .OrderByDescending(b =>
                                b.BillCycle.BillingYear)

                            .ThenByDescending(b =>
                                b.BillCycle.BillingMonth)

                            .FirstOrDefaultAsync();

                    decimal previousDueAmount =
                        latestBill == null
                            ? 0
                            : latestBill.TotalAmount
                              -
                              latestBill.AmountPaid;

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
                                    mr.MeterId
                                        ==
                                        tenantMeter.MeterId
                                    &&
                                    mr.BillingMonth
                                        ==
                                        request.BillingMonth
                                    &&
                                    mr.BillingYear
                                        ==
                                        request.BillingYear)

                                .FirstAsync();

                        var previousReading =
                            await _context.MeterReadings

                                .Where(mr =>
                                    mr.MeterId
                                        ==
                                        tenantMeter.MeterId
                                    &&
                                    mr.ReadingDate
                                        <
                                        currentReading.ReadingDate)

                                .OrderByDescending(
                                    mr => mr.ReadingDate)

                                .FirstOrDefaultAsync();

                        decimal previousValue =
                            previousReading?.ReadingValue
                            ??
                            tenantMeter.Meter.InitialReading;

                        decimal units =
                            currentReading.ReadingValue
                            -
                            previousValue;



                        if (currentReading.ReadingValue < previousValue)
                        {
                            throw new Exception(
                                $"Current reading for meter {tenantMeter.Meter.MeterNumber} cannot be less than previous reading"
                            );
                        }

                        decimal tenantUnits =
                            units;

                        int sharedTenantCount =
                            1;

                        if (
                            tenantMeter.Meter.MeterType
                            ==
                            MeterType.Shared
                        )
                        {
                            sharedTenantCount =
                                await _context.TenantMeters

                                    .CountAsync(tm =>
                                        tm.MeterId
                                            ==
                                            tenantMeter.MeterId
                                        &&
                                        tm.EndDate == null);



                            tenantUnits =
                                Math.Ceiling(
                                    units
                                    /
                                    sharedTenantCount
                                );
                        }

                        decimal amount =
                            tenantUnits
                            *
                            house.ElectricityRate;

                        electricityAmount +=
                            amount;

                        billDetails.Add(

                            new BillDetail
                            {
                                DetailType =
                                    BillDetailType.Electricity,

                                MeterId =
                                    tenantMeter.MeterId,

                                Description =
                                    tenantMeter.Meter.MeterNumber,

                                PreviousReading =
                                    previousValue,

                                CurrentReading =
                                    currentReading.ReadingValue,

                                UnitsConsumed =
                                    units,

                                TenantUnits =
                                    tenantUnits,

                                SharedTenantCount =
                                    sharedTenantCount,

                                Rate =
                                    house.ElectricityRate,

                                Amount =
                                    amount
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

                    foreach (var charge in request.ExtraCharges)
                    {

                        if (
                            charge.TenantIds.Contains(
                                tenant.Id
                            )
                        )
                        {

                            decimal splitAmount =
                                Math.Ceiling(
                                    charge.Amount
                                    /
                                    charge.TenantIds.Count
                                );


                            extraChargeAmount +=
                                splitAmount;


                            billDetails.Add(

                                new BillDetail
                                {
                                    DetailType =
                                        BillDetailType.Maintenance,


                                    Description =
                                        charge.ChargeName,


                                    Amount =
                                        splitAmount
                                });

                        }

                    }

                    decimal total =
                        rentAmount
                        +
                        electricityAmount
                        +
                        garbageAmount
                        +
                        extraChargeAmount
                        +
                        previousDueAmount;

                    var bill =
                        new Bill
                        {
                            BillNumber =
                                $"BILL-{request.BillingYear}-{Guid.NewGuid().ToString()[..6]}",

                            BillCycleId =
                                billCycle.Id,

                            TenantId =
                                tenant.Id,

                            RentAmount =
                                rentAmount,

                            ElectricityAmount =
                                electricityAmount,

                            GarbageAmount =
                                garbageAmount,

                            ExtraChargeAmount =
                                extraChargeAmount,

                            PreviousDueAmount =
                                previousDueAmount,

                            TotalAmount =
                                total,

                            AmountPaid =
                                0,

                            PaymentStatus =
                                PaymentStatus.Unpaid,

                            BillDetails =
                                billDetails
                        };



                    await _context.Bills
                        .AddAsync(bill);

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

                            BillCycleId = billCycle.Id

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
                                ex.Message,

                            BillCycleId =
                            billCycle.Id
                        });
                }
            }

            await _context.SaveChangesAsync();

            return results;
        }

        public async Task<BillCycleValidationResponseDto>
    ValidateBillCycleAsync(
        int houseId,
        int month,
        int year,
        int userId)
        {
            var existingCycle =
                await _context.BillCycles

                    .AnyAsync(bc =>
                        bc.HouseId == houseId
                        &&
                        (int)bc.BillingMonth == month
                        &&
                        bc.BillingYear == year);



            if (existingCycle)
            {
                return new BillCycleValidationResponseDto
                {
                    IsValid = false,


                    Message =
                        "Bill already generated for selected month and year"
                };
            }






            var latestCycle =
                await _context.BillCycles

                    .Where(bc =>
                        bc.HouseId == houseId)

                    .OrderByDescending(bc =>
                        bc.BillingYear)

                    .ThenByDescending(bc =>
                        bc.BillingMonth)

                    .FirstOrDefaultAsync();





            if (latestCycle != null)
            {

                bool isOlderCycle =
                    year < latestCycle.BillingYear
                    ||
                    (
                        year == latestCycle.BillingYear
                        &&
                        month < (int)latestCycle.BillingMonth
                    );



                if (isOlderCycle)
                {
                    return new BillCycleValidationResponseDto
                    {
                        IsValid = false,


                        Message =
                            $"Cannot generate bill before latest cycle {latestCycle.BillingMonth} {latestCycle.BillingYear}"
                    };
                }

            }




            return new BillCycleValidationResponseDto
            {
                IsValid = true
            };
        }
    }
}
