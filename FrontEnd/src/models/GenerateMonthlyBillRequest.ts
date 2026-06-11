export interface GenerateMonthlyBillRequest {
  houseId: number;

  billingYear: number;

  billingMonth: number;

  meterReadings: GenerateMeterReading[];

  extraCharges: GenerateExtraCharge[];
}

export interface GenerateMeterReading {
  meterId: number;

  readingValue: number;
}

export interface GenerateExtraCharge {
  chargeName: string;

  amount: number;

  tenantIds: number[];
}
