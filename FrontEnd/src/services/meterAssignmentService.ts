import api from "../api/axiosConfig";

import type { AssignMeter } from "../models/AssignMeter";

import type { RemoveMeterAssignment } from "../models/RemoveMeterAssignment";

import type { MeterAssignment } from "../models/MeterAssignment";

export const assignMeter = async (
  request: AssignMeter
): Promise<MeterAssignment> => {
  const response = await api.post<MeterAssignment>(
    "/MeterAssignment/assign",
    request
  );

  return response.data;
};

export const removeMeterAssignment = async (
  request: RemoveMeterAssignment
): Promise<string> => {
  const response = await api.post<string>("/MeterAssignment/remove", request);

  return response.data;
};

export const getActiveMeterAssignments = async (): Promise<
  MeterAssignment[]
> => {
  const response = await api.get<MeterAssignment[]>("/MeterAssignment/active");

  return response.data;
};
