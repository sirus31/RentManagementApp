export interface PaymentFilter {
  houses: PaymentFilterHouse[];

  tenants: PaymentFilterTenant[];

  months: PaymentFilterMonth[];

  years: number[];
}

export interface PaymentFilterHouse {
  id: number;

  name: string;
}

export interface PaymentFilterTenant {
  id: number;

  name: string;

  houseIds: number[];
}

export interface PaymentFilterMonth {
  value: number;

  name: string;
}
