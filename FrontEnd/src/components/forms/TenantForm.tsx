import { useState } from "react";

import type { CreateTenant } from "../../models/CreateTenant";

import { createTenant } from "../../services/tenantService";

import Input from "../ui/Input";

import Button from "../ui/Button";

type TenantFormProps = {
  onTenantCreated: () => void;
};

const initialTenantState: CreateTenant = {
  fullName: "",

  phoneNumber: "",

  monthlyRent: 0,
};

function TenantForm({ onTenantCreated }: TenantFormProps) {
  const [tenant, setTenant] = useState<CreateTenant>(initialTenantState);

  const [error, setError] = useState("");

  const validateTenant = () => {
    if (!tenant.fullName.trim()) {
      setError("Tenant name is required");

      return false;
    }

    if (tenant.phoneNumber.length !== 10) {
      setError("Phone number must be 10 digits");

      return false;
    }

    if (tenant.monthlyRent <= 0) {
      setError("Monthly rent must be greater than 0");

      return false;
    }

    setError("");

    return true;
  };

  const handleSave = async () => {
    if (!validateTenant()) {
      return;
    }

    try {
      await createTenant(tenant);

      onTenantCreated();

      setTenant(initialTenantState);
    } catch (error) {
      console.log(error);

      setError("Unable to create tenant");
    }
  };

  return (
    <div
      className="
        bg-white
        p-5
        rounded-lg
        shadow
        mb-6
      "
    >
      {error && <p className="text-red-500 mb-3">{error}</p>}

      <div className="flex gap-3">
        <Input
          placeholder="Full Name"
          value={tenant.fullName}
          onChange={(value) =>
            setTenant({
              ...tenant,

              fullName: value,
            })
          }
        />

        <Input
          placeholder="Phone Number"
          value={tenant.phoneNumber}
          maxLength={10}
          onChange={(value) => {
            const numbersOnly = value.replace(/\D/g, "");

            setTenant({
              ...tenant,

              phoneNumber: numbersOnly,
            });
          }}
        />

        <Input
          placeholder="Monthly Rent"
          type="number"
          value={String(tenant.monthlyRent || "")}
          onKeyDown={(e) => {
            if (e.key === "e" || e.key === "+" || e.key === "-") {
              e.preventDefault();
            }
          }}
          onChange={(value) =>
            setTenant({
              ...tenant,

              monthlyRent: Number(value),
            })
          }
        />

        <Button type="button" onClick={handleSave}>
          Add Tenant
        </Button>
      </div>
    </div>
  );
}

export default TenantForm;
