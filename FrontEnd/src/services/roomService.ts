import api from "../api/axiosConfig";

import type { Room } from "../models/Room";

import type { CreateRoom } from "../models/CreateRoom";

export const getRoomsByFloorId = async (floorId: number): Promise<Room[]> => {
  const response = await api.get(`/Room/floor/${floorId}`);

  return response.data;
};

export const createRoom = async (room: CreateRoom): Promise<Room> => {
  const response = await api.post("/Room", room);

  return response.data;
};
