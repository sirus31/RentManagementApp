import { useEffect, useState } from "react";

import Button from "../ui/Button";

import { assignMeter } from "../../services/meterAssignmentService";

import { getMetersByHouse } from "../../services/meterService";

import type { Meter } from "../../models/Meter";

import type { TenantOverview } from "../../models/TenantOverview";

type Props = {
  tenant: TenantOverview;

  houseId: number;

  onSuccess: () => void;
};

function AssignMeterForm({ tenant, houseId, onSuccess }: Props) {
  const [meters, setMeters] = useState<Meter[]>([]);

  const [meterId, setMeterId] = useState<number>(0);

  const [startDate, setStartDate] = useState("");

  const [error, setError] = useState("");

  useEffect(() => {
    loadMeters();
  }, []);

  const loadMeters = async () => {
    const data = await getMetersByHouse(houseId);

    const activeMeters = data.filter((meter) => meter.isActive);

    setMeters(activeMeters);
  };

  const handleSubmit = async () => {
    setError("");

    if (meterId === 0) {
      setError("Please select a meter");

      return;
    }

    if (!startDate) {
      setError("Please select start date");

      return;
    }

    try {
      await assignMeter({
        tenantId: tenant.tenantId,

        meterId,

        startDate,
      });

      onSuccess();
    } catch (error: any) {
      setError(error.response?.data || "Something went wrong");
    }
  };

  return (
    <div className="space-y-4">
      {/* Tenant Info */}
      <div className="border rounded p-3 bg-gray-50">
        <h3 className="font-bold mb-2">Tenant Details</h3>

        <p>Name: {tenant.tenantName}</p>

        <p>
          Current Meters:{" "}
          {tenant.meters.length > 0
            ? tenant.meters.join(", ")
            : "No meters assigned"}
        </p>
      </div>

      {error && (
        <div className="bg-red-100 text-red-700 border border-red-300 p-3 rounded ">
          {error}
        </div>
      )}

      <select
        className="
        border
        p-2
        w-full
        "
        value={meterId}
        onChange={(e) => {
          setMeterId(Number(e.target.value));
          setError("");
        }}
      >
        <option value={0}>Select Meter</option>

        {meters.map((meter) => (
          <option key={meter.id} value={meter.id}>
            {meter.meterNumber}
          </option>
        ))}
      </select>

      <input
        className="
        border
        p-2
        w-full
        "
        type="date"
        value={startDate}
        onChange={(e) => setStartDate(e.target.value)}
      />

      <Button onClick={handleSubmit}>Assign Meter</Button>
    </div>
  );
}

export default AssignMeterForm;
