import { useEffect, useState } from "react";

import { useNavigate, useSearchParams } from "react-router-dom";

import Select from "../components/ui/Select";

import DataTable from "../components/ui/DataTable";

import Button from "../components/ui/Button";

import GenerateBillForm from "../components/forms/GenerateBillForm";

import { getHouses } from "../services/houseService";

import { getBillCyclesByHouse, getBillDetails } from "../services/billService";

import BillDetails from "../components/modals/BillDetails";

import type { BillFullDetail } from "../models/BillFullDetail";

import type { House } from "../models/House";

import type { BillCycleOverview } from "../models/BillCycleOverview";

function BillPage() {
  const [houses, setHouses] = useState<House[]>([]);

  const [selectedHouseId, setSelectedHouseId] = useState(0);

  const [cycles, setCycles] = useState<BillCycleOverview[]>([]);

  const [expandedCycles, setExpandedCycles] = useState<number[]>([]);

  const [monthFilter, setMonthFilter] = useState("all");

  const [yearFilter, setYearFilter] = useState("all");

  const [selectedBill, setSelectedBill] = useState<BillFullDetail | null>(null);

  const navigate = useNavigate();

  const [searchParams] = useSearchParams();

  const openCycleId = Number(searchParams.get("cycleId"));

  const openHouseId = Number(searchParams.get("houseId"));

  useEffect(() => {
    loadHouses();
  }, []);

  const loadHouses = async () => {
    const housesData = await getHouses();

    setHouses(housesData);

    if (housesData.length > 0) {
      const defaultHouseId = openHouseId ? openHouseId : housesData[0].id;

      setSelectedHouseId(defaultHouseId);

      loadBills(defaultHouseId);
    }
  };

  const loadBills = async (houseId: number) => {
    setSelectedHouseId(houseId);

    const billCycles = await getBillCyclesByHouse(houseId);

    setCycles(billCycles);

    if (openCycleId) {
      setExpandedCycles([openCycleId]);
    } else {
      setExpandedCycles([]);
    }
  };

  const openBillDetails = async (billId: number) => {
    const data = await getBillDetails(billId);

    setSelectedBill(data);
  };

  const toggleCycle = (billCycleId: number) => {
    if (expandedCycles.includes(billCycleId)) {
      setExpandedCycles(
        expandedCycles.filter(
          (expandedCycleId) => expandedCycleId !== billCycleId
        )
      );
    } else {
      setExpandedCycles([...expandedCycles, billCycleId]);
    }
  };

  const filteredCycles = cycles.filter((cycle) => {
    const monthMatches =
      monthFilter === "all" || cycle.billingMonth.toString() === monthFilter;

    const yearMatches =
      yearFilter === "all" || cycle.billingYear.toString() === yearFilter;

    return monthMatches && yearMatches;
  });

  const years = [...new Set(cycles.map((cycle) => cycle.billingYear))];

  const expandAll = () => {
    setExpandedCycles(filteredCycles.map((cycle) => cycle.id));
  };

  const collapseAll = () => {
    setExpandedCycles([]);
  };

  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Bills</h1>

      <div className="flex justify-between mb-6">
        <div className="flex gap-4">
          <Select
            value={selectedHouseId}
            options={houses.map((house) => ({
              label: house.name,
              value: house.id,
            }))}
            onChange={(value) => loadBills(Number(value))}
          />

          <select
            className="border p-2 rounded"
            value={monthFilter}
            onChange={(event) => setMonthFilter(event.target.value)}
          >
            <option value="all">All Months</option>
            <option value="Baisakh">Baisakh</option>
            <option value="Jestha">Jestha</option>
            <option value="Ashadh">Ashadh</option>
            <option value="Shrawan">Shrawan</option>
            <option value="Bhadra">Bhadra</option>
            <option value="Ashwin">Ashwin</option>
            <option value="Kartik">Kartik</option>
            <option value="Mangsir">Mangsir</option>
            <option value="Poush">Poush</option>
            <option value="Magh">Magh</option>
            <option value="Falgun">Falgun</option>
            <option value="Chaitra">Chaitra</option>
          </select>

          <select
            className="border p-2 rounded"
            value={yearFilter}
            onChange={(event) => setYearFilter(event.target.value)}
          >
            <option value="all">All Years</option>

            {years.map((year) => (
              <option key={year} value={year}>
                {year}
              </option>
            ))}
          </select>
        </div>

        <div className="flex gap-3">
          <button
            onClick={expandAll}
            className="
              border px-4 py-2 rounded
              cursor-pointer
              hover:bg-gray-200
              active:scale-95
              transition duration-150
            "
          >
            Expand All
          </button>

          <button
            onClick={collapseAll}
            className="
              border px-4 py-2 rounded
              cursor-pointer
              hover:bg-gray-200
              active:scale-95
              transition duration-150
            "
          >
            Collapse All
          </button>
        </div>
      </div>

      <GenerateBillForm />

      {filteredCycles.map((cycle) => (
        <div
          key={cycle.id}
          className="
                bg-white
                shadow
                rounded-lg
                mb-5
              "
        >
          <div
            onClick={() => toggleCycle(cycle.id)}
            className="
                  p-4
                  cursor-pointer
                  flex
                  justify-between
                "
          >
            <div>
              {expandedCycles.includes(cycle.id) ? "▼" : "▶"}{" "}
              {cycle.billingMonth} {cycle.billingYear}
            </div>

            <div>
              {cycle.totalBills} Bills | Rs {cycle.paidAmount}/
              {cycle.totalAmount}| Pending Rs {cycle.pendingAmount}
            </div>
          </div>

          <div
            className={`
                  overflow-hidden
                  transition-all
                  duration-300
                  ease-in-out

                  ${
                    expandedCycles.includes(cycle.id)
                      ? "max-h-200 opacity-100"
                      : "max-h-0 opacity-0"
                  }
                `}
          >
            <DataTable
              columns={[
                "Tenant",
                "Rooms",
                "Rent",
                "Electricity",
                "Garbage",
                "Extra",
                "Previous Due",
                "Total",
                "Paid",
                "Due",
                "Status",
                "Action",
              ]}
            >
              {cycle.bills.map((bill) => (
                <tr key={bill.billId}>
                  <td className="p-4">{bill.tenantName}</td>

                  <td className="p-4">{bill.rooms.join(", ")}</td>

                  <td className="p-4">Rs {bill.rentAmount}</td>

                  <td className="p-4">Rs {bill.electricityAmount}</td>

                  <td className="p-4">Rs {bill.garbageAmount}</td>

                  <td className="p-4">Rs {bill.extraChargeAmount}</td>

                  <td className="p-4">Rs {bill.previousDueAmount}</td>

                  <td className="p-4 font-semibold">Rs {bill.totalAmount}</td>

                  <td className="p-4">Rs {bill.amountPaid}</td>

                  <td className="p-4">Rs {bill.pendingAmount}</td>

                  <td className="p-4">{bill.paymentStatus}</td>

                  <td className="p-4">
                    <button
                      onClick={() => openBillDetails(bill.billId)}
                      className="
            text-blue-600
            hover:underline
            cursor-pointer
          "
                    >
                      View Details
                    </button>
                  </td>
                </tr>
              ))}
            </DataTable>
          </div>
        </div>
      ))}

      {selectedBill && (
        <BillDetails
          bill={selectedBill}
          onClose={() => setSelectedBill(null)}
          onRefresh={async () => {
            await openBillDetails(selectedBill.billId);

            await loadBills(selectedHouseId);
          }}
        />
      )}
    </div>
  );
}

export default BillPage;
