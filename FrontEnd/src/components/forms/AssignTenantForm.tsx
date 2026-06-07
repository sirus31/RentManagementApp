import { useEffect, useState } from "react";

import type { Tenant } from "../../models/Tenant";
import type { AssignRoom } from "../../models/AssignRoom";

import { getTenants } from "../../services/tenantService";
import { assignRoom } from "../../services/occupancyService";

import Button from "../ui/Button";

type AssignTenantFormProps = {
  roomId: number;

  onTenantAssigned: () => void;
};

function AssignTenantForm({ roomId, onTenantAssigned }: AssignTenantFormProps) {
  const [tenants, setTenants] = useState<Tenant[]>([]);

  const [assignment, setAssignment] = useState<AssignRoom>({
    tenantId: 0,

    roomId: roomId,

    startDate: "",
  });

  useEffect(() => {
    loadTenants();
  }, []);

  const loadTenants = async () => {
    const data = await getTenants();

    const activeTenants = data.filter((tenant) => tenant.isActive);

    setTenants(activeTenants);
  };

  const handleAssign = async () => {
    await assignRoom(assignment);

    onTenantAssigned();

    setAssignment({
      tenantId: 0,

      roomId: roomId,

      startDate: "",
    });
  };

  return (
    <div className="bg-white p-5 rounded shadow">
      <h2 className="font-bold mb-3">Assign Tenant</h2>

      <select
        className="border p-2 mr-3"
        value={assignment.tenantId}
        onChange={(e) =>
          setAssignment({
            ...assignment,

            tenantId: Number(e.target.value),
          })
        }
      >
        <option value={0}>Select Tenant</option>

        {tenants.map((tenant) => (
          <option key={tenant.id} value={tenant.id}>
            {tenant.fullName}
          </option>
        ))}
      </select>

      <input
        type="date"
        className="border p-2 mr-3"
        value={assignment.startDate}
        onChange={(e) =>
          setAssignment({
            ...assignment,

            startDate: e.target.value,
          })
        }
      />

      <Button onClick={handleAssign}>Assign</Button>
    </div>
  );
}

export default AssignTenantForm;
