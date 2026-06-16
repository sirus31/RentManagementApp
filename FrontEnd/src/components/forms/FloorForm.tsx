import { useState } from "react";

// import type { CreateFloor } from "../../models/CreateFloor";

import { createFloor } from "../../services/floorService";

import Input from "../ui/Input";

import Button from "../ui/Button";

type FloorFormState = {
  houseId: number;

  floorNumber: string;

  name: string;
};

type FloorFormProps = {
  houseId: number;

  onFloorCreated: () => void;
};

function FloorForm({
  houseId,

  onFloorCreated,
}: FloorFormProps) {
  const [floor, setFloor] = useState<FloorFormState>({
    houseId: houseId,

    floorNumber: "",

    name: "",
  });

  const [error, setError] = useState("");

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!floor.name.trim()) {
      setError("Floor name is required");

      return;
    }

    try {
      await createFloor({
        ...floor,

        floorNumber: Number(floor.floorNumber),
      });

      onFloorCreated();

      setFloor({
        houseId: houseId,

        floorNumber: "",

        name: "",
      });

      setError("");
    } catch (error) {
      console.log(error);

      setError("Unable to create floor");
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="bg-white p-5 rounded-lg shadow mb-5 "
    >
      {error && <p className="text-red-500 mb-3">{error}</p>}

      <div className="flex gap-3">
        <Input
          placeholder="Floor Name"
          value={floor.name}
          onChange={(value) =>
            setFloor({
              ...floor,

              name: value,
            })
          }
        />

        <Input
          placeholder="Floor Number"
          type="number"
          value={floor.floorNumber}
          onKeyDown={(e) => {
            if (e.key === "e" || e.key === "+" || e.key === "-") {
              e.preventDefault();
            }
          }}
          onChange={(value) =>
            setFloor({
              ...floor,

              floorNumber: value,
            })
          }
        />

        <Button type="submit">Add Floor</Button>
      </div>
    </form>
  );
}

export default FloorForm;
