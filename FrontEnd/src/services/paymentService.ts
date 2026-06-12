import api from "../api/axiosConfig";

import type { CreatePaymentRequest } from "../models/CreatePaymentRequest";

export const createPayment = async (request: CreatePaymentRequest) => {
  const response = await api.post("/payment", request);

  return response.data;
};
