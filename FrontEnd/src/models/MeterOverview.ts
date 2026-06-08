export interface MeterOverview {
  id: number;

  houseId: number;

  meterNumber: string;

  meterType: string;

  initialReading: number;

  isActive: boolean;

  assignedTenants: string[];
}
