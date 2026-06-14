export interface PaymentDashboard {
  totalCollected: number;

  totalPending: number;

  totalTransactions: number;

  pendingPayments: PendingPayment[];

  recentPayments: RecentPayment[];
}

export interface PendingPayment {
  billId: number;

  tenantName: string;

  billingMonth: string;

  billingYear: number;

  totalAmount: number;

  amountPaid: number;

  pendingAmount: number;
}

export interface RecentPayment {
  paymentId: number;

  tenantName: string;

  billingMonth: string;

  billingYear: number;

  amount: number;

  paymentDate: string;

  paymentMode: string;
}
