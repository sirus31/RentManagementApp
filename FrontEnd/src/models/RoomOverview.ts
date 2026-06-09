export interface RoomOverview {
  floorId: number;

  floorName: string;

  totalRooms: number;

  occupiedRooms: number;

  vacantRooms: number;

  rooms: RoomDetail[];
}

export interface RoomDetail {
  roomId: number;

  roomNumber: string;

  isOccupied: boolean;

  tenantName: string | null;
}
