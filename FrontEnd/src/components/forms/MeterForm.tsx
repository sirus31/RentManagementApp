import { useState } from "react";

import Input from "../ui/Input";
import Button from "../ui/Button";

import type { CreateMeter } from "../../models/CreateMeter";

import { createMeter } from "../../services/meterService";

type Props = {
  houseId: number;

  onMeterCreated: () => void;
};

function MeterForm({ houseId, onMeterCreated }: Props) {
  const [meter, setMeter] = useState<CreateMeter>({
    houseId: houseId,

    meterNumber: "",

    meterType: "Private",

    initialReading: "",
  });

  const [error, setError] = useState("");

  const handleSubmit = async () => {
    if (meter.meterNumber.trim() === "") {
      setError("Meter number is required");

      return;
    }

    if (meter.initialReading === "") {
      setError("Initial reading is required");

      return;
    }

    if (meter.initialReading < 0) {
      setError("Initial reading cannot be negative");

      return;
    }

    await createMeter(meter);

    setMeter({
      houseId: houseId,

      meterNumber: "",

      meterType: "Private",

      initialReading: "",
    });

    setError("");

    onMeterCreated();
  };

  return (
    <div>
      {error && <p className="text-red-500 mb-2">{error}</p>}

      <div className="flex gap-4 mb-6">
        <Input
          placeholder="Meter Number"
          value={meter.meterNumber}
          onChange={(value) =>
            setMeter({
              ...meter,

              meterNumber: value,
            })
          }
        />

        <select
          className="
          border
          border-gray-300
          rounded
          px-3
          py-2
          "
          value={meter.meterType}
          onChange={(e) =>
            setMeter({
              ...meter,

              meterType: e.target.value,
            })
          }
        >
          <option value="Private">Private</option>

          <option value="Shared">Shared</option>
        </select>

        <Input
          type="number"
          placeholder="Initial Reading"
          value={String(meter.initialReading)}
          onChange={(value) =>
            setMeter({
              ...meter,

              initialReading: value === "" ? "" : Number(value),
            })
          }
        />

        <Button onClick={handleSubmit}>Add Meter</Button>
      </div>
    </div>
  );
}

export default MeterForm;
