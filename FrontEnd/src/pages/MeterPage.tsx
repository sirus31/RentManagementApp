import { useEffect, useState } from "react";

import DataTable from "../components/ui/DataTable";

import Select from "../components/ui/Select";

import MeterForm from "../components/forms/MeterForm";

import { getHouses } from "../services/houseService";

import { getMeterOverviewByHouse } from "../services/meterService";

import type { House } from "../models/House";

import type { MeterOverview } from "../models/MeterOverview";

function MeterPage() {
  const [houses, setHouses] = useState<House[]>([]);

  const [meters, setMeters] = useState<MeterOverview[]>([]);

  const [selectedHouseId, setSelectedHouseId] = useState(0);

  const [statusFilter, setStatusFilter] = useState("all");

  useEffect(() => {
    loadHouses();
  }, []);

  const loadHouses = async () => {
    const data = await getHouses();

    setHouses(data);
  };

  const loadMeters = async (houseId: number) => {
    setSelectedHouseId(houseId);

    if (houseId === 0) {
      setMeters([]);

      return;
    }

    const data = await getMeterOverviewByHouse(houseId);

    setMeters(data);
  };

  const filteredMeters = meters.filter((meter) => {
    if (statusFilter === "active") {
      return meter.isActive;
    }

    if (statusFilter === "inactive") {
      return !meter.isActive;
    }

    return true;
  });

  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Meters</h1>

      <div className="flex gap-4 mb-6">
        <Select
          value={selectedHouseId}
          options={[
            {
              label: "Select House",

              value: 0,
            },

            ...houses.map((house) => ({
              label: house.name,

              value: house.id,
            })),
          ]}
          onChange={(value) => loadMeters(Number(value))}
        />

        <select
          className="border p-2 rounded"
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
        >
          <option value="all">All</option>

          <option value="active">Active</option>

          <option value="inactive">Inactive</option>
        </select>
      </div>

      <MeterForm
        onMeterCreated={() => {
          if (selectedHouseId !== 0) {
            loadMeters(selectedHouseId);
          }
        }}
      />

      <DataTable columns={["Meter", "Type", "Status", "Assigned Tenants"]}>
        {filteredMeters.map((meter) => (
          <tr key={meter.id} className="border-b">
            <td className="p-4">{meter.meterNumber}</td>

            <td className="p-4">{meter.meterType}</td>

            <td className="p-4">{meter.isActive ? "Active" : "Inactive"}</td>

            <td className="p-4">
              {meter.assignedTenants.length > 0
                ? meter.assignedTenants.join(", ")
                : "Not assigned"}
            </td>
          </tr>
        ))}
      </DataTable>
    </div>
  );
}

export default MeterPage;
