import { useEffect, useState } from "react";

import type { Tenant } from "../models/Tenant";

import { getTenants } from "../services/tenantService";

import DataTable from "../components/ui/DataTable";

import TenantForm from "../components/forms/TenantForm";

import { Link } from "react-router-dom";

function TenantPage() {
  const [tenants, setTenants] = useState<Tenant[]>([]);
  const [statusFilter, setStatusFilter] = useState("all");

  useEffect(() => {
    loadTenants();
  }, []);

  const loadTenants = async () => {
    const data = await getTenants();

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

      <select
        className="border p-2 mb-5"
        value={statusFilter}
        onChange={(e) => setStatusFilter(e.target.value)}
      >
        <option value="all">All</option>

        <option value="active">Active</option>

        <option value="inactive">Inactive</option>
      </select>

      <TenantForm onTenantCreated={loadTenants} />

      <DataTable columns={["Name", "Phone", "Rent", "Status", "Actions"]}>
        {filteredTenants.map((tenant) => (
          <tr key={tenant.id} className="border-b">
            <td className="p-4">{tenant.fullName}</td>

            <td className="p-4">{tenant.phoneNumber}</td>

            <td className="p-4">Rs {tenant.monthlyRent}</td>

            <td className="p-4">{tenant.isActive ? "Active" : "Inactive"}</td>

            <td className="p-4">
              <Link className="text-blue-600" to={`/tenants/${tenant.id}`}>
                View
              </Link>
            </td>
          </tr>
        ))}
      </DataTable>
    </div>
  );
}

export default TenantPage;
