import api from "../api/axiosConfig";
import type { Meter } from "../models/Meter";
import type { CreateMeter } from "../models/CreateMeter";
import type { MeterOverview } from "../models/MeterOverview";

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

export const getMeterOverviewByHouse = async (
  houseId: number
): Promise<MeterOverview[]> => {
  const response = await api.get(`/Meter/overview/${houseId}`);

  return response.data;
};
