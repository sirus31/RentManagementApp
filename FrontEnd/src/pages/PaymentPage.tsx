import { useEffect, useState } from "react";

import DataTable from "../components/ui/DataTable";

import Select from "../components/ui/Select";

import Button from "../components/ui/Button";

import ReceivePaymentModal from "../components/modals/ReceivePaymentModal";

import {
  getPaymentDashboard,
  getPaymentFilters,
} from "../services/paymentService";

import type {
  PaymentFilterHouse,
  PaymentFilterTenant,
  PaymentFilterMonth,
} from "../models/PaymentFilter";

import type {
  PaymentDashboard,
  PendingPayment,
} from "../models/PaymentDashboard";

function PaymentPage() {
  const [dashboard, setDashboard] = useState<PaymentDashboard | null>(null);

  const [selectedBill, setSelectedBill] = useState<PendingPayment | null>(null);

  const [houses, setHouses] = useState<PaymentFilterHouse[]>([]);

  const [tenants, setTenants] = useState<PaymentFilterTenant[]>([]);

  const [months, setMonths] = useState<PaymentFilterMonth[]>([]);

  const [years, setYears] = useState<number[]>([]);

  const [selectedHouseId, setSelectedHouseId] = useState("all");

  const [selectedTenantId, setSelectedTenantId] = useState("all");

  const [selectedMonth, setSelectedMonth] = useState("all");

  const [selectedYear, setSelectedYear] = useState("all");

  useEffect(() => {
    loadFilters();
  }, []);

  useEffect(() => {
    loadDashboard();
  }, [selectedHouseId, selectedTenantId, selectedMonth, selectedYear]);

  const loadFilters = async () => {
    const filters = await getPaymentFilters();

    setHouses(filters.houses);

    setTenants(filters.tenants);

    setMonths(filters.months);

    setYears(filters.years);
  };

  const loadDashboard = async () => {
    const data = await getPaymentDashboard({
      houseId: selectedHouseId === "all" ? undefined : Number(selectedHouseId),

      tenantId:
        selectedTenantId === "all" ? undefined : Number(selectedTenantId),

      month: selectedMonth === "all" ? undefined : Number(selectedMonth),

      year: selectedYear === "all" ? undefined : Number(selectedYear),
    });

    setDashboard(data);
  };

  if (!dashboard) {
    return <p>Loading...</p>;
  }

  const filteredTenants =
    selectedHouseId === "all"
      ? tenants
      : tenants.filter((tenant) =>
          tenant.houseIds.includes(Number(selectedHouseId))
        );

  const selectedTenant = tenants.find(
    (tenant) => tenant.id === Number(selectedTenantId)
  );

  const filteredHouses =
    selectedTenantId === "all"
      ? houses
      : houses.filter((house) => selectedTenant?.houseIds.includes(house.id));

  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Payments</h1>

      {/* FILTERS */}

      <div className="flex gap-4 mb-6">
        <Select
          value={selectedHouseId}
          onChange={setSelectedHouseId}
          options={[
            {
              label: "All Houses",
              value: "all",
            },

            ...filteredHouses.map((house) => ({
              label: house.name,

              value: house.id.toString(),
            })),
          ]}
        />

        <Select
          value={selectedTenantId}
          onChange={setSelectedTenantId}
          options={[
            {
              label: "All Tenants",
              value: "all",
            },

            ...filteredTenants.map((tenant) => ({
              label: tenant.name,

              value: tenant.id.toString(),
            })),
          ]}
        />

        <Select
          value={selectedMonth}
          onChange={setSelectedMonth}
          options={[
            {
              label: "All Months",

              value: "all",
            },

            ...months.map((month) => ({
              label: month.name,

              value: month.value.toString(),
            })),
          ]}
        />

        <Select
          value={selectedYear}
          onChange={setSelectedYear}
          options={[
            {
              label: "All Years",

              value: "all",
            },

            ...years.map((year) => ({
              label: year.toString(),

              value: year.toString(),
            })),
          ]}
        />
      </div>

      {/* SUMMARY */}

      <div className="grid grid-cols-3 gap-5 mb-6">
        <div className="bg-white shadow rounded p-5">
          <p>Total Collected</p>

          <h2 className="text-xl font-bold">Rs {dashboard.totalCollected}</h2>
        </div>

        <div className="bg-white shadow rounded p-5">
          <p>Pending Amount</p>

          <h2 className="text-xl font-bold">Rs {dashboard.totalPending}</h2>
        </div>

        <div className="bg-white shadow rounded p-5">
          <p>Transactions</p>

          <h2 className="text-xl font-bold">{dashboard.totalTransactions}</h2>
        </div>
      </div>

      {/* PENDING PAYMENTS */}

      <h2 className="font-semibold mb-3">Pending Payments</h2>

      {dashboard.pendingPayments.length === 0 ? (
        <div className="rounded p-8 text-center shadow">
          <h3 className="font-semibold text-lg">All payments cleared</h3>

          <p className="text-gray-500 mt-2">No pending bills found.</p>
        </div>
      ) : (
        <div className="max-h-80 overflow-y-auto">
          <DataTable
            columns={[
              "Tenant",
              "Month",
              "Bill Amount",
              "Paid",
              "Due",
              "Status",
              "Action",
            ]}
          >
            {dashboard.pendingPayments.map((bill) => (
              <tr key={bill.billId}>
                <td className="p-4">{bill.tenantName}</td>

                <td className="p-4">
                  {bill.billingMonth} {bill.billingYear}
                </td>

                <td className="p-4">Rs {bill.totalAmount}</td>

                <td className="p-4">Rs {bill.amountPaid}</td>

                <td className="p-4">Rs {bill.pendingAmount}</td>

                <td className="p-4">
                  {bill.canReceivePayment ? "Pending" : "Carried Forward"}
                </td>

                <td className="p-4">
                  {bill.canReceivePayment ? (
                    <Button onClick={() => setSelectedBill(bill)}>
                      Receive Payment
                    </Button>
                  ) : (
                    <span className="text-gray-500">View Only</span>
                  )}
                </td>
              </tr>
            ))}
          </DataTable>
        </div>
      )}

      {/* RECENT PAYMENTS */}

      <h2 className="font-semibold mt-8 mb-3">Recent Payments</h2>

      {dashboard.recentPayments.length === 0 ? (
        <div className="bg-white rounded shadow p-8 text-center">
          <h3 className="font-semibold">No transactions yet</h3>

          <p className="text-gray-500 mt-2">
            Payments you record will appear here.
          </p>
        </div>
      ) : (
        <div className="max-h-70 overflow-y-auto">
          <DataTable
            columns={["Date", "Tenant", "Bill Month", "Amount", "Mode"]}
          >
            {dashboard.recentPayments.map((payment) => (
              <tr key={payment.paymentId}>
                <td className="p-4">
                  {new Date(payment.paymentDate).toLocaleDateString()}
                </td>

                <td className="p-4">{payment.tenantName}</td>

                <td className="p-4">
                  {payment.billingMonth} {payment.billingYear}
                </td>

                <td className="p-4">Rs {payment.amount}</td>

                <td className="p-4">{payment.paymentMode}</td>
              </tr>
            ))}
          </DataTable>
        </div>
      )}

      {selectedBill && (
        <ReceivePaymentModal
          bill={selectedBill}
          onClose={() => setSelectedBill(null)}
          onSuccess={loadDashboard}
        />
      )}
    </div>
  );
}

export default PaymentPage;
