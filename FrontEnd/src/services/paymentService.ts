import api from "../api/axiosConfig";

import type { CreatePaymentRequest } from "../models/CreatePaymentRequest";
import type { PaymentDashboard } from "../models/PaymentDashboard";
import type { PaymentDashboardFilter } from "../models/PaymentDashboardFilter";
import type { PaymentFilter } from "../models/PaymentFilter";

export const createPayment = async (request: CreatePaymentRequest) => {
  const response = await api.post("/payment", request);

  return response.data;
};

export const getPaymentDashboard = async (
  filters?: PaymentDashboardFilter
): Promise<PaymentDashboard> => {
  const response = await api.get<PaymentDashboard>("/payment/dashboard", {
    params: filters,
  });

  return response.data;
};

export const getPaymentFilters = async (): Promise<PaymentFilter> => {
  const response = await api.get<PaymentFilter>("/payment/filters");

  return response.data;
};
