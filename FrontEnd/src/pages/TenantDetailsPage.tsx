import { useEffect, useState } from "react";

import { Link, useParams } from "react-router-dom";

import type { Tenant } from "../models/Tenant";

import { getTenantById } from "../services/tenantService";

function TenantDetailsPage() {
  const { id } = useParams();

  const [tenant, setTenant] = useState<Tenant | null>(null);

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState("");

  useEffect(() => {
    loadTenant();
  }, []);

  const loadTenant = async () => {
    try {
      const data = await getTenantById(Number(id));

      setTenant(data);
    } catch {
      setError("Tenant not found");
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <p>Loading tenant details...</p>;
  }

  if (error) {
    return <p className="text-red-500">{error}</p>;
  }

  if (!tenant) {
    return null;
  }

  return (
    <div>
      <Link to="/tenants" className="text-blue-600">
        ← Back to Tenants
      </Link>

      <h1 className="text-3xl font-bold my-5">Tenant Details</h1>

      <div className="bg-white shadow rounded-lg p-6 w-96">
        <p>
          <b>Name:</b> {tenant.fullName}
        </p>

        <p>
          <b>Phone:</b> {tenant.phoneNumber}
        </p>

        <p>
          <b>Monthly Rent:</b> Rs {tenant.monthlyRent}
        </p>

        <p>
          <b>Status:</b> {tenant.isActive ? "Active" : "Inactive"}
        </p>
      </div>
    </div>
  );
}

export default TenantDetailsPage;
