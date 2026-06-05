import api from "../api/axiosConfig";

import type { CreateHouse } from "../models/CreateHouse";
import type { House } from "../models/House";

export const getHouses = async (): Promise<House[]> => {
  const response = await api.get<House[]>("/House");

  return response.data;
};

export const createHouse = async (house: CreateHouse): Promise<House> => {
  const response = await api.post<House>("/House", house);

  return response.data;
};
