import { useState } from "react";
import type { FormEvent } from "react";

import type { CreateHouse } from "../../models/CreateHouse";

import { createHouse } from "../../services/houseService";
import Button from "../ui/Button";
import Input from "../ui/Input";

type HouseFormProps = {
  onHouseCreated: () => void;
};

type HouseFormState = {
  userId: number;

  name: string;

  address: string;

  electricityRate: string;

  garbageFee: string;
};

const initialHouseState: HouseFormState = {
  userId: 2,

  name: "",

  address: "",

  electricityRate: "",

  garbageFee: "",
};

function HouseForm({ onHouseCreated }: HouseFormProps) {
  const [house, setHouse] = useState<HouseFormState>(initialHouseState);

  const [error, setError] = useState("");

  const validateHouse = () => {
    if (!house.name.trim()) {
      setError("House name is required");

      return false;
    }

    if (!house.address.trim()) {
      setError("Address is required");

      return false;
    }

    if (/\d/.test(house.address)) {
      setError("Address cannot contain numbers");

      return false;
    }

    if (Number(house.electricityRate) <= 0) {
      setError("Electricity rate must be greater than 0");

      return false;
    }

    if (Number(house.garbageFee) < 0) {
      setError("Garbage fee cannot be negative");

      return false;
    }

    setError("");

    return true;
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!validateHouse()) {
      return;
    }

    const newHouse: CreateHouse = {
      userId: house.userId,

      name: house.name,

      address: house.address,

      electricityRate: Number(house.electricityRate),

      garbageFee: Number(house.garbageFee),
    };

    try {
      await createHouse(newHouse);

      setHouse(initialHouseState);

      onHouseCreated();
    } catch (error) {
      console.log(error);

      setError("Unable to create house");
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="bg-white shadow rounded-lg p-5 mb-5"
    >
      {error && <p className="text-red-500 mb-3">{error}</p>}

      <div className="flex gap-3">
        <Input
          placeholder="House Name"
          value={house.name}
          onChange={(value) =>
            setHouse({
              ...house,
              name: value,
            })
          }
        />

        <Input
          placeholder="Address"
          value={house.address}
          onChange={(value) =>
            setHouse({
              ...house,
              address: value,
            })
          }
        />

        <Input
          placeholder="Electricity Rate"
          type="number"
          value={house.electricityRate}
          onKeyDown={(e) => {
            if (e.key === "e" || e.key === "+" || e.key === "-") {
              e.preventDefault();
            }
          }}
          onChange={(value) =>
            setHouse({
              ...house,

              electricityRate: value,
            })
          }
        />

        <Input
          placeholder="Garbage Fee"
          type="number"
          value={house.garbageFee}
          onKeyDown={(e) => {
            if (e.key === "e" || e.key === "+" || e.key === "-") {
              e.preventDefault();
            }
          }}
          onChange={(value) =>
            setHouse({
              ...house,

              garbageFee: value,
            })
          }
        />

        <Button type="submit">Add House</Button>
      </div>
    </form>
  );
}

export default HouseForm;
