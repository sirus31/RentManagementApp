export type BillCycleOverview = {
  id: number;

  billingYear: number;

  billingMonth: number;

  cycleType: number;

  status: number;

  generatedDate: string;

  totalBills: number;

  totalAmount: number;

  paidAmount: number;

  pendingAmount: number;

  paidCount: number;

  partialCount: number;

  pendingCount: number;

  bills: TenantBill[];
};

export type TenantBill = {
  billId: number;

  tenantName: string;

  rooms: string[];

  rentAmount: number;

  electricityAmount: number;

  garbageAmount: number;

  extraChargeAmount: number;

  previousDueAmount: number;

  totalAmount: number;

  amountPaid: number;

  pendingAmount: number;

  paymentStatus: number;
};
