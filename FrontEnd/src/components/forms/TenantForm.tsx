import { useEffect, useState } from "react";

import Input from "../ui/Input";

import Button from "../ui/Button";

import Select from "../ui/Select";

import { getHouses } from "../../services/houseService";

import { getAvailableRoomsByHouse } from "../../services/roomService";

import { moveInTenant } from "../../services/occupancyService";

import type { House } from "../../models/House";

import type { Room } from "../../models/Room";

import type { MoveInTenant } from "../../models/MoveInTenant";

type TenantFormProps = {
  onTenantCreated: () => void;
};

const initialTenantState: MoveInTenant = {
  fullName: "",

  phoneNumber: "",

  monthlyRent: 0,

  roomIds: [],

  moveInDate: "",
};

function TenantForm({ onTenantCreated }: TenantFormProps) {
  const [tenant, setTenant] = useState<MoveInTenant>(initialTenantState);

  const [houses, setHouses] = useState<House[]>([]);

  const [rooms, setRooms] = useState<Room[]>([]);

  const [houseId, setHouseId] = useState(0);

  const [error, setError] = useState("");

  useEffect(() => {
    loadHouses();
  }, []);

  const loadHouses = async () => {
    const data = await getHouses();

    setHouses(data);
  };

  const handleHouseChange = async (value: string) => {
    const selectedHouseId = Number(value);

    setHouseId(selectedHouseId);

    setTenant({
      ...tenant,

      roomIds: [],
    });

    if (selectedHouseId === 0) {
      setRooms([]);

      return;
    }

    const data = await getAvailableRoomsByHouse(selectedHouseId);

    setRooms(data);
  };

  const toggleRoom = (roomId: number) => {
    if (tenant.roomIds.includes(roomId)) {
      setTenant({
        ...tenant,

        roomIds: tenant.roomIds.filter((id) => id !== roomId),
      });
    } else {
      setTenant({
        ...tenant,

        roomIds: [...tenant.roomIds, roomId],
      });
    }
  };

  const validateTenant = () => {
    if (!houseId) {
      setError("Please select house");

      return false;
    }

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

    if (tenant.roomIds.length === 0) {
      setError("Please select room");

      return false;
    }

    if (!tenant.moveInDate) {
      setError("Move in date required");

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
      await moveInTenant(tenant);

      onTenantCreated();

      setTenant(initialTenantState);

      setHouseId(0);

      setRooms([]);
    } catch (error) {
      console.log(error);

      setError("Unable to move in tenant");
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

      <h2
        className="
      text-xl
      font-semibold
      mb-5
      "
      >
        Move In New Tenant
      </h2>

      <div className="flex gap-3 mb-4">
        <Select
          value={houseId}
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
          onChange={handleHouseChange}
        />

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
      </div>

      <div className="mb-4">
        <p className="font-semibold mb-2">Select Rooms</p>

        <div className="flex gap-4">
          {houseId === 0 ? (
            <p className="text-gray-500">Select a house to view rooms</p>
          ) : rooms.length === 0 ? (
            <p className="text-gray-500">No available rooms</p>
          ) : (
            rooms.map((room) => (
              <label key={room.id}>
                <input
                  type="checkbox"
                  checked={tenant.roomIds.includes(room.id)}
                  onChange={() => toggleRoom(room.id)}
                />{" "}
                {room.roomNumber}
              </label>
            ))
          )}
        </div>
      </div>

      <Input
        type="date"
        placeholder="Move In Date"
        value={tenant.moveInDate}
        onChange={(value) =>
          setTenant({
            ...tenant,

            moveInDate: value,
          })
        }
      />

      <div className="mt-4">
        <Button type="button" onClick={handleSave}>
          Move In Tenant
        </Button>
      </div>
    </div>
  );
}

export default TenantForm;
