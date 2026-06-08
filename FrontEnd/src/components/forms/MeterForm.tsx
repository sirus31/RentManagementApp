import { useEffect, useState } from "react";

import Input from "../ui/Input";

import Button from "../ui/Button";

import Select from "../ui/Select";

import type { CreateMeter } from "../../models/CreateMeter";

import type { House } from "../../models/House";

import { createMeter } from "../../services/meterService";

import { getHouses } from "../../services/houseService";

type Props = {
  houseId?: number;

  onMeterCreated: () => void;
};

function MeterForm({ houseId, onMeterCreated }: Props) {
  const [meter, setMeter] = useState<CreateMeter>({
    houseId: houseId ?? 0,

    meterNumber: "",

    meterType: "Private",

    initialReading: "",
  });

  const [houses, setHouses] = useState<House[]>([]);

  const [error, setError] = useState("");

  useEffect(() => {
    if (!houseId) {
      loadHouses();
    }
  }, [houseId]);

  const loadHouses = async () => {
    const data = await getHouses();

    setHouses(data);
  };

  const resetMeter = () => {
    setMeter({
      houseId: houseId ?? 0,

      meterNumber: "",

      meterType: "Private",

      initialReading: "",
    });
  };

  const validateMeter = () => {
    if (meter.houseId === 0) {
      setError("Please select house");

      return false;
    }

    if (!meter.meterNumber.trim()) {
      setError("Meter number is required");

      return false;
    }

    if (meter.initialReading === "") {
      setError("Initial reading is required");

      return false;
    }

    if (meter.initialReading < 0) {
      setError("Initial reading cannot be negative");

      return false;
    }

    setError("");

    return true;
  };

  const handleSubmit = async () => {
    if (!validateMeter()) {
      return;
    }

    try {
      await createMeter(meter);

      resetMeter();

      onMeterCreated();
    } catch (error) {
      console.log(error);

      setError("Unable to create meter");
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
      <h2
        className="
        text-xl
        font-semibold
        mb-5
        "
      >
        Create Meter
      </h2>

      {error && <p className="text-red-500 mb-3">{error}</p>}

      <div className="flex gap-4 mb-6">
        {!houseId && (
          <Select
            value={meter.houseId}
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
            onChange={(value) =>
              setMeter({
                ...meter,

                houseId: Number(value),
              })
            }
          />
        )}

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

        <Select
          value={meter.meterType}
          options={[
            {
              label: "Private",

              value: "Private",
            },

            {
              label: "Shared",

              value: "Shared",
            },
          ]}
          onChange={(value) =>
            setMeter({
              ...meter,

              meterType: value,
            })
          }
        />

        <Input
          type="number"
          placeholder="Initial Reading"
          value={String(meter.initialReading)}
          onKeyDown={(e) => {
            if (e.key === "-" || e.key === "+" || e.key === "e") {
              e.preventDefault();
            }
          }}
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
