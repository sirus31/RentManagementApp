import { BillingMonth } from "../enums/BillingMonth";

export const getBillingMonthName = (month: number) => {
  const entry = Object.entries(BillingMonth).find(
    ([monthName, monthValue]) => Number(monthValue) === month
  );

  return entry ? entry[0] : "";
};
