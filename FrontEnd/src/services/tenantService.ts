import api from "../api/axiosConfig";

import type { CreateTenant } from "../models/CreateTenant";
import type { Tenant } from "../models/Tenant";

export const getTenants = async (): Promise<Tenant[]> => {
  const response = await api.get<Tenant[]>("/Tenant");

  return response.data;
};

export const getTenantById = async (id: number): Promise<Tenant> => {
  const response = await api.get<Tenant>(`/Tenant/${id}`);

  return response.data;
};

export const createTenant = async (tenant: CreateTenant): Promise<Tenant> => {
  const response = await api.post<Tenant>("/Tenant", tenant);

  return response.data;
};
