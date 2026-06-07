import api from "../api/axiosConfig";
import type { Meter } from "../models/Meter";
import type { CreateMeter } from "../models/CreateMeter";

export const createMeter = async (meter: CreateMeter): Promise<Meter> => {
  const response = await api.post("/Meter", meter);

  return response.data;
};

export const getMeters = async (): Promise<Meter[]> => {
  const response = await api.get("/Meter");

  return response.data;
};

export const getMetersByHouse = async (houseId: number): Promise<Meter[]> => {
  const response = await api.get(`/Meter/house/${houseId}`);

  return response.data;
};
