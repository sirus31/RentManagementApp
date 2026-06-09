import { useEffect, useState } from "react";

import type { FormEvent } from "react";

import type { CreateRoom } from "../../models/CreateRoom";

import type { House } from "../../models/House";

import type { Floor } from "../../models/Floor";

import { createRoom } from "../../services/roomService";

import { getHouses } from "../../services/houseService";

import { getFloorsByHouseId } from "../../services/floorService";

import Input from "../ui/Input";

import Button from "../ui/Button";

import Select from "../ui/Select";

type RoomFormProps = {
  floorId?: number;

  onRoomCreated: (houseId: number, floorId: number) => void;
};

function RoomForm({
  floorId,

  onRoomCreated,
}: RoomFormProps) {
  const [room, setRoom] = useState<CreateRoom>({
    floorId: floorId ?? 0,

    roomNumber: "",
  });

  const [houses, setHouses] = useState<House[]>([]);

  const [floors, setFloors] = useState<Floor[]>([]);

  const [selectedHouseId, setSelectedHouseId] = useState(0);

  const [error, setError] = useState("");

  useEffect(() => {
    if (!floorId) {
      loadHouses();
    }
  }, []);

  const loadHouses = async () => {
    const data = await getHouses();

    setHouses(data);
  };

  const handleHouseChange = async (value: number) => {
    setSelectedHouseId(value);

    setRoom({
      ...room,

      floorId: 0,
    });

    const data = await getFloorsByHouseId(value);

    setFloors(data);
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (room.floorId === 0) {
      setError("Please select floor");

      return;
    }

    if (!room.roomNumber.trim()) {
      setError("Room number is required");

      return;
    }

    try {
      await createRoom(room);

      onRoomCreated(selectedHouseId, room.floorId);

      setRoom({
        floorId: floorId ?? 0,

        roomNumber: "",
      });

      setError("");
    } catch (error) {
      console.log(error);

      setError("Unable to create room");
    }
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="
      bg-white
      shadow
      rounded-lg
      p-5
      mb-5
      "
    >
      <h2
        className="
        text-xl
        font-semibold
        mb-5
        "
      >
        Create Room
      </h2>

      {error && <p className="text-red-500 mb-3">{error}</p>}

      <div className="flex gap-3">
        {!floorId && (
          <>
            <Select
              value={selectedHouseId}
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
              onChange={(value) => handleHouseChange(Number(value))}
            />

            <Select
              value={room.floorId}
              options={[
                {
                  label: "Select Floor",

                  value: 0,
                },

                ...floors.map((floor) => ({
                  label: floor.name,

                  value: floor.id,
                })),
              ]}
              onChange={(value) =>
                setRoom({
                  ...room,

                  floorId: Number(value),
                })
              }
            />
          </>
        )}

        <Input
          placeholder="Room Number"
          value={room.roomNumber}
          onChange={(value) =>
            setRoom({
              ...room,

              roomNumber: value,
            })
          }
        />

        <Button type="submit">Add Room</Button>
      </div>
    </form>
  );
}

export default RoomForm;
