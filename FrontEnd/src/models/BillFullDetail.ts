export interface BillFullDetail {
  billId: number;

  billNumber: string;

  tenantName: string;

  billingMonth: string;

  billingYear: number;

  rentAmount: number;

  electricityAmount: number;

  garbageAmount: number;

  extraChargeAmount: number;

  previousDueAmount: number;

  totalAmount: number;

  amountPaid: number;

  pendingAmount: number;

  details: BillDetail[];

  payments: Payment[];

  rooms: string[];
}

export interface BillDetail {
  detailType: string;

  description: string;

  previousReading: number;

  currentReading: number;

  unitsConsumed: number;

  rate: number;

  amount: number;

  tenantUnits: number;

  sharedTenantCount: number;
}

export interface Payment {
  id: number;

  billId: number;

  billNumber: string;

  amount: number;

  paymentDate: string;

  paymentMode: string;

  notes: string;
}
