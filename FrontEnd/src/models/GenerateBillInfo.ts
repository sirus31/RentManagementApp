export interface GenerateBillInfo {
  house: GenerateBillHouse;

  tenants: GenerateBillTenant[];

  meters: GenerateBillMeter[];

  previousDues: GenerateBillPreviousDue[];
}

export interface GenerateBillHouse {
  id: number;

  name: string;

  address: string;

  electricityRate: number;

  garbageFee: number;
}

export interface GenerateBillTenant {
  tenantId: number;

  tenantName: string;

  rooms: string[];

  monthlyRent: number;

  meters: string[];
}

export interface GenerateBillMeter {
  meterId: number;

  meterNumber: string;

  meterType: string;

  previousReading: number;

  currentReading?: number;
}

export interface GenerateBillPreviousDue {
  tenantId: number;

  tenantName: string;

  billingMonth: string;

  billingYear: number;

  dueAmount: number;
}
