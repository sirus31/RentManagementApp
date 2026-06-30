using Microsoft.EntityFrameworkCore;

using RentManagementApp.Data;

using RentManagementApp.DTOs.Requests;
using RentManagementApp.DTOs.Responses;

using RentManagementApp.Models.Enums;
using RentManagementApp.Models.TransactionalEntities;

using RentManagementApp.Services.Interfaces;


namespace RentManagementApp.Services
{
    public class PaymentService
        : IPaymentService
    {
        private readonly ApplicationDbContext _context;


        public PaymentService(
            ApplicationDbContext context)
        {
            _context = context;
        }



        public async Task<PaymentResponseDto>
            CreatePaymentAsync(
                CreatePaymentRequestDto request, int userId)
        {

            var bill = await _context.Bills

                .Include(b =>
                    b.BillCycle).ThenInclude(bc => bc.House)

                .FirstOrDefaultAsync(b =>
                    b.Id == request.BillId);



            if (bill == null)
            {
                throw new Exception(
                    "Bill not found");
            }

            if (bill.BillCycle.House.UserId != userId)
            {
                throw new Exception(
                    "Bill not found");
            }



            if (bill.BillCycle.Status ==
                BillStatus.Cancelled)
            {
                throw new Exception(
                    "Cannot pay cancelled bill");
            }



            if (bill.AmountPaid + request.Amount >
                bill.TotalAmount)
            {
                throw new Exception(
                    "Payment exceeds remaining bill amount");
            }




            var payment = new Payment
            {
                BillId =
                    request.BillId,


                Amount =
                    request.Amount,


                PaymentMode =
                    request.PaymentMode,


                Notes =
                    request.Notes,


                PaymentDate =
                    DateTime.UtcNow
            };



            await _context.Payments
                .AddAsync(payment);



            bill.AmountPaid +=
                request.Amount;




            if (bill.AmountPaid ==
                bill.TotalAmount)
            {
                bill.PaymentStatus =
                    PaymentStatus.Paid;
            }


            else if (bill.AmountPaid > 0)
            {
                bill.PaymentStatus =
                    PaymentStatus.Partial;
            }


            else
            {
                bill.PaymentStatus =
                    PaymentStatus.Unpaid;
            }




            await _context.SaveChangesAsync();



            return new PaymentResponseDto
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
            };
        }


        public async Task<List<PaymentResponseDto>>
            GetBillPaymentsAsync(
                int billId, int userId)
        {

            var billExists = await _context.Bills
                .AnyAsync(b =>
                    b.Id == billId
                    && b.BillCycle.House.UserId == userId);

            if (!billExists)
            {
                throw new Exception("Bill not found");
            }

            var payments =
                await _context.Payments


                    .Include(p =>
                        p.Bill)


                    .Where(p =>
                        p.BillId == billId)


                    .OrderByDescending(p =>
                        p.PaymentDate)


                    .Select(p =>
                        new PaymentResponseDto
                        {
                            Id =
                                p.Id,


                            BillId =
                                p.BillId,


                            BillNumber =
                                p.Bill.BillNumber,


                            Amount =
                                p.Amount,


                            PaymentDate =
                                p.PaymentDate,


                            PaymentMode =
                                p.PaymentMode,


                            Notes =
                                p.Notes
                        })


                    .ToListAsync();



            return payments;
        }


        public async Task<List<PaymentResponseDto>>
            GetAllPaymentsAsync(int userId)
        {

            var payments =
                await _context.Payments


                    .Include(p =>
                        p.Bill)
                        .ThenInclude(b => b.BillCycle)


                    .Where(p =>
                        p.Bill.BillCycle.House.UserId == userId)

                    .OrderByDescending(p =>
                        p.PaymentDate)


                    .Select(p =>
                        new PaymentResponseDto
                        {
                            Id =
                                p.Id,


                            BillId =
                                p.BillId,


                            BillNumber =
                                p.Bill.BillNumber,


                            Amount =
                                p.Amount,


                            PaymentDate =
                                p.PaymentDate,


                            PaymentMode =
                                p.PaymentMode,


                            Notes =
                                p.Notes
                        })


                    .ToListAsync();



            return payments;
        }

        public async Task<PaymentDashboardResponseDto>
    GetPaymentDashboardAsync(
        int userId,
        int? houseId,
        int? tenantId,
        int? month,
        int? year)
        {



            var billQuery =
                _context.Bills

                    .Include(bill =>
                        bill.Tenant)

                    .Include(bill =>
                        bill.BillCycle)

                    .Where(bill =>
                        bill.BillCycle.House.UserId == userId)

                    .AsQueryable();




            if (houseId.HasValue)
            {
                billQuery =
                    billQuery.Where(bill =>
                        bill.BillCycle.HouseId
                        ==
                        houseId.Value);
            }




            if (tenantId.HasValue)
            {
                billQuery =
                    billQuery.Where(bill =>
                        bill.TenantId
                        ==
                        tenantId.Value);
            }




            if (month.HasValue)
            {
                billQuery =
                    billQuery.Where(bill =>
                        (int)bill.BillCycle.BillingMonth
                        ==
                        month.Value);
            }




            if (year.HasValue)
            {
                billQuery =
                    billQuery.Where(bill =>
                        bill.BillCycle.BillingYear
                        ==
                        year.Value);
            }







            var latestBills =
     await billQuery


         .GroupBy(bill =>
             bill.TenantId)


         .Select(group =>
             group

                 .OrderByDescending(bill =>
                     bill.BillCycle.BillingYear)


                 .ThenByDescending(bill =>
                     bill.BillCycle.BillingMonth)


                 .First())


         .ToListAsync();



            var latestBillIds =
       await _context.Bills

           .GroupBy(bill =>
               bill.TenantId)

           .Select(group =>
               group

                   .OrderByDescending(bill =>
                       bill.BillCycle.BillingYear)

                   .ThenByDescending(bill =>
                       bill.BillCycle.BillingMonth)

                   .Select(bill =>
                       bill.Id)

                   .First())

           .ToListAsync();






            var pendingBills =
                latestBills


                    .Where(bill =>
                        bill.TotalAmount
                        -
                        bill.AmountPaid
                        >
                        0)


                    .Select(bill =>
                        new PendingPaymentDto
                        {
                            BillId =
                                bill.Id,


                            TenantName =
                                bill.Tenant.FullName,


                            BillingMonth =
                                bill.BillCycle
                                    .BillingMonth
                                    .ToString(),


                            BillingYear =
                                bill.BillCycle
                                    .BillingYear,


                            TotalAmount =
                                bill.TotalAmount,


                            AmountPaid =
                                bill.AmountPaid,


                            PendingAmount =
                                bill.TotalAmount
                                -
                                bill.AmountPaid,

                            CanReceivePayment =
                                latestBillIds.Contains(
                                    bill.Id
                                )


                        })



                    .ToList();







            var paymentQuery =
                _context.Payments

                    .Include(payment =>
                        payment.Bill)

                        .ThenInclude(bill =>
                            bill.Tenant)


                    .Include(payment =>
                        payment.Bill)

                        .ThenInclude(bill =>
                            bill.BillCycle)

                    .Where(payment =>
                        payment.Bill.BillCycle.House.UserId == userId)

                    .AsQueryable();






            if (houseId.HasValue)
            {
                paymentQuery =
                    paymentQuery.Where(payment =>
                        payment.Bill.BillCycle.HouseId
                        ==
                        houseId.Value);
            }




            if (tenantId.HasValue)
            {
                paymentQuery =
                    paymentQuery.Where(payment =>
                        payment.Bill.TenantId
                        ==
                        tenantId.Value);
            }




            if (month.HasValue)
            {
                paymentQuery =
                    paymentQuery.Where(payment =>
                        (int)payment.Bill.BillCycle.BillingMonth
                        ==
                        month.Value);
            }




            if (year.HasValue)
            {
                paymentQuery =
                    paymentQuery.Where(payment =>
                        payment.Bill.BillCycle.BillingYear
                        ==
                        year.Value);
            }







            var recentPayments =
                await paymentQuery


                    .OrderByDescending(payment =>
                        payment.PaymentDate)


                    .Select(payment =>
                        new RecentPaymentDto
                        {
                            PaymentId =
                                payment.Id,


                            TenantName =
                                payment.Bill
                                    .Tenant
                                    .FullName,


                            BillingMonth =
                                payment.Bill
                                    .BillCycle
                                    .BillingMonth
                                    .ToString(),


                            BillingYear =
                                payment.Bill
                                    .BillCycle
                                    .BillingYear,


                            Amount =
                                payment.Amount,


                            PaymentDate =
                                payment.PaymentDate,


                            PaymentMode =
                                payment.PaymentMode
                        })


                    .ToListAsync();








            return new PaymentDashboardResponseDto
            {
                TotalCollected =
                    recentPayments.Sum(payment =>
                        payment.Amount),


                TotalPending =
                    pendingBills.Sum(bill =>
                        bill.PendingAmount),


                TotalTransactions =
                    recentPayments.Count,


                PendingPayments =
                    pendingBills,


                RecentPayments =
                    recentPayments
            };
        }

        public async Task<PaymentFilterResponseDto> GetPaymentFiltersAsync(int userId)
        {

            var houses =
                await _context.Houses

                    .Where(h => h.UserId == userId)

                    .Select(house =>
                        new PaymentFilterHouseDto
                        {
                            Id =
                                house.Id,


                            Name =
                                house.Name
                        })


                    .ToListAsync();





            var tenants =
                await _context.Tenants


                    .Include(tenant =>
                        tenant.TenantRooms)

                        .ThenInclude(tenantRoom =>
                            tenantRoom.Room)

                            .ThenInclude(room =>
                                room.Floor)

                    .Where(tenant =>
                        tenant.TenantRooms.Any(tr =>
                            tr.Room.Floor.House.UserId == userId))

                    .Select(tenant =>
                        new PaymentFilterTenantDto
                        {
                            Id =
                                tenant.Id,


                            Name =
                                tenant.FullName,


                            HouseIds =
                                tenant.TenantRooms

                                    .Select(tenantRoom =>
                                        tenantRoom.Room
                                            .Floor
                                            .HouseId)

                                    .Distinct()

                                    .ToList()
                        })


                    .ToListAsync();

            var months =
                await _context.BillCycles

                    .Where(bc => bc.House.UserId == userId)


                    .Select(cycle =>
                        cycle.BillingMonth)


                    .Distinct()


                    .OrderBy(month =>
                        month)


                    .Select(month =>
                        new PaymentFilterMonthDto
                        {
                            Value =
                                (int)month,


                            Name =
                                month.ToString()
                        })


                    .ToListAsync();






            var years =
                await _context.BillCycles

                    .Where(bc => bc.House.UserId == userId)


                    .Select(cycle =>
                        cycle.BillingYear)


                    .Distinct()


                    .OrderByDescending(year =>
                        year)


                    .ToListAsync();


            return new PaymentFilterResponseDto
            {
                Houses =
            houses,


                Tenants =
            tenants,


                Months =
            months,


                Years =
            years
            };
        }
    }
}
