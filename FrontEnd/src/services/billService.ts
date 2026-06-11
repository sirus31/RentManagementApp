import api from "../api/axiosConfig";

import type { BillCycleOverview } from "../models/BillCycleOverview";

import type { BillFullDetail } from "../models/BillFullDetail";

import type { GenerateBillInfo } from "../models/GenerateBillInfo";

import type { GenerateMonthlyBillRequest } from "../models/GenerateMonthlyBillRequest";

export const getBillCyclesByHouse = async (
  houseId: number
): Promise<BillCycleOverview[]> => {
  const response = await api.get(`/Bill/house/${houseId}/cycles`);

  return response.data;
};

export const getBillDetails = async (billId: number) => {
  const response = await api.get<BillFullDetail>(`/bill/${billId}/details`);

  return response.data;
};

export const getGenerateBillInfo = async (
  houseId: number
): Promise<GenerateBillInfo> => {
  const response = await api.get<GenerateBillInfo>(
    `/bill/generate-info/${houseId}`
  );

  return response.data;
};

export const generateMonthlyBill = async (
  request: GenerateMonthlyBillRequest
) => {
  const response = await api.post(
    "/bill/generate-monthly",

    request
  );

  return response.data;
};

type BillCycleValidationResponse = {
  isValid: boolean;

  message: string;
};

export const validateBillCycle = async (
  houseId: number,

  month: string,

  year: string
): Promise<BillCycleValidationResponse> => {
  const response = await api.get<BillCycleValidationResponse>(
    "/bill/validate-cycle",
    {
      params: {
        houseId,

        month,

        year,
      },
    }
  );

  return response.data;
};
