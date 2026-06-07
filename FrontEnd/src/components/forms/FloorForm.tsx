import { useState } from "react";

import type { CreateFloor } from "../../models/CreateFloor";

import { createFloor } from "../../services/floorService";

import Input from "../ui/Input";

import Button from "../ui/Button";

type FloorFormProps = {
  houseId: number;

  onFloorCreated: () => void;
};

function FloorForm({
  houseId,

  onFloorCreated,
}: FloorFormProps) {
  const [floor, setFloor] = useState<CreateFloor>({
    houseId: houseId,

    floorNumber: 0,

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
      await createFloor(floor);

      onFloorCreated();

      setFloor({
        houseId: houseId,

        floorNumber: 0,

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
          value={String(floor.floorNumber || "")}
          onKeyDown={(e) => {
            if (e.key === "e" || e.key === "+" || e.key === "-") {
              e.preventDefault();
            }
          }}
          onChange={(value) =>
            setFloor({
              ...floor,

              floorNumber: Number(value),
            })
          }
        />

        <Button type="submit">Add Floor</Button>
      </div>
    </form>
  );
}

export default FloorForm;
