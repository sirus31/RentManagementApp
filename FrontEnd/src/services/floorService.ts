import api from "../api/axiosConfig";

import type { Floor } from "../models/Floor";

import type { CreateFloor } from "../models/CreateFloor";

export const getFloorsByHouseId = async (houseId: number): Promise<Floor[]> => {
  const response = await api.get(`/Floor/house/${houseId}`);

  return response.data;
};

export const createFloor = async (floor: CreateFloor): Promise<Floor> => {
  const response = await api.post("/Floor", floor);

  return response.data;
};
