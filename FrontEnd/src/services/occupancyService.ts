import api from "../api/axiosConfig";

import type { Occupancy } from "../models/Occupancy";

import type { AssignRoom } from "../models/AssignRoom";

import type { VacateRoom } from "../models/VacateRoom";

export const getActiveOccupancies = async (): Promise<Occupancy[]> => {
  const response = await api.get("/Occupancy/active");

  return response.data;
};

export const assignRoom = async (
  assignment: AssignRoom
): Promise<Occupancy> => {
  const response = await api.post("/Occupancy/assign", assignment);

  return response.data;
};

export const vacateRoom = async (request: VacateRoom): Promise<void> => {
  await api.post("/Occupancy/vacate", request);
};
