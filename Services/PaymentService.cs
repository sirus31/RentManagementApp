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
                CreatePaymentRequestDto request)
        {

            var bill = await _context.Bills

                .Include(b =>
                    b.BillCycle)

                .FirstOrDefaultAsync(b =>
                    b.Id == request.BillId);



            if (bill == null)
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



            if (bill.BillCycle.Status !=
                BillStatus.Finalized)
            {
                throw new Exception(
                    "Bill must be finalized before payment");
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
                int billId)
        {

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
            GetAllPaymentsAsync()
        {

            var payments =
                await _context.Payments


                    .Include(p =>
                        p.Bill)


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
    }
}