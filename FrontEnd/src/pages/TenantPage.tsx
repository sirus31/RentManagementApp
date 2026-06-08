import { useEffect, useState } from "react";

import { getTenantOverview } from "../services/tenantService";

import { getHouses } from "../services/houseService";

import type { House } from "../models/House";

import Modal from "../components/ui/Modal";

import AssignMeterForm from "../components/forms/AssignMeterForm";

import DataTable from "../components/ui/DataTable";

import TenantForm from "../components/forms/TenantForm";

import { Link } from "react-router-dom";

import type { TenantOverview } from "../models/TenantOverview";
import Select from "../components/ui/Select";

function TenantPage() {
  const [houses, setHouses] = useState<House[]>([]);

  const [selectedHouseId, setSelectedHouseId] = useState<number>(0);

  const [tenants, setTenants] = useState<TenantOverview[]>([]);

  const [selectedTenant, setSelectedTenant] = useState<TenantOverview | null>(
    null
  );

  const [statusFilter, setStatusFilter] = useState("all");

  useEffect(() => {
    loadHouses();
  }, []);

  const loadHouses = async () => {
    const data = await getHouses();

    setHouses(data);

    if (data.length > 0) {
      setSelectedHouseId(data[0].id);
    }
  };

  useEffect(() => {
    if (selectedHouseId !== 0) {
      loadTenants();
    }
  }, [selectedHouseId]);

  const loadTenants = async () => {
    const data = await getTenantOverview(selectedHouseId);

    setTenants(data);
  };

  const filteredTenants = tenants.filter((tenant) => {
    if (statusFilter === "active") {
      return tenant.isActive;
    }

    if (statusFilter === "inactive") {
      return !tenant.isActive;
    }

    return true;
  });

  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Tenants</h1>
      <div className="flex gap-4 mb-6">
        <Select
          value={selectedHouseId}
          onChange={(value) => setSelectedHouseId(Number(value))}
          options={houses.map((house) => ({
            label: house.name,

            value: house.id,
          }))}
        />

        <Select
          value={statusFilter}
          onChange={(value) => setStatusFilter(value)}
          options={[
            {
              label: "All",
              value: "all",
            },

            {
              label: "Active",
              value: "active",
            },

            {
              label: "Inactive",
              value: "inactive",
            },
          ]}
        />
      </div>

      <TenantForm onTenantCreated={loadTenants} />

      <DataTable
        columns={[
          "Name",
          "Phone",
          "Rent",
          "Status",
          "Rooms",
          "Meters",
          "Actions",
        ]}
      >
        {filteredTenants.map((tenant) => (
          <tr key={tenant.tenantId} className="border-b">
            <td className="p-4">{tenant.tenantName}</td>

            <td className="p-4">{tenant.phoneNumber}</td>

            <td className="p-4">Rs {tenant.monthlyRent}</td>

            <td className="p-4">{tenant.isActive ? "Active" : "Inactive"}</td>

            <td className="p-4">
              {tenant.rooms.length > 0 ? tenant.rooms.join(", ") : "No rooms"}
            </td>

            <td className="p-4">
              {tenant.meters.length > 0
                ? tenant.meters.join(", ")
                : "No meters"}
            </td>

            <td className="p-4 flex gap-4">
              <Link
                className="text-blue-600"
                to={`/tenants/${tenant.tenantId}`}
              >
                View
              </Link>

              <button
                className="
                bg-blue-600
                text-white
                px-4
                py-2
                rounded
                "
                onClick={() => setSelectedTenant(tenant)}
              >
                Assign Meter
              </button>
            </td>
          </tr>
        ))}
      </DataTable>

      {selectedTenant && (
        <Modal title="Assign Meter" onClose={() => setSelectedTenant(null)}>
          <AssignMeterForm
            tenant={selectedTenant}
            houseId={selectedHouseId}
            onSuccess={() => {
              setSelectedTenant(null);

              loadTenants();
            }}
          />
        </Modal>
      )}
    </div>
  );
}

export default TenantPage;
