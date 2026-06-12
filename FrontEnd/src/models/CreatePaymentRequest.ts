export interface CreatePaymentRequest {
  billId: number;

  amount: number;

  paymentMode: string;

  notes: string;
}
