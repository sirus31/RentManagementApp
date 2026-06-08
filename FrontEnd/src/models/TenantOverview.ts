export type TenantOverview = {
  tenantId: number;

  tenantName: string;

  phoneNumber: string;

  monthlyRent: number;

  isActive: boolean;

  rooms: string[];

  meters: string[];
};
