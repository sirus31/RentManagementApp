import { useState } from "react";

import type { FormEvent } from "react";

import type { CreateRoom } from "../../models/CreateRoom";

import { createRoom } from "../../services/roomService";

import Input from "../ui/Input";

import Button from "../ui/Button";

type RoomFormProps = {
  floorId: number;

  onRoomCreated: () => void;
};

function RoomForm({
  floorId,

  onRoomCreated,
}: RoomFormProps) {
  const [room, setRoom] = useState<CreateRoom>({
    floorId: floorId,

    roomNumber: "",
  });

  const [error, setError] = useState("");

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!room.roomNumber.trim()) {
      setError("Room number is required");

      return;
    }

    try {
      await createRoom(room);

      onRoomCreated();

      setRoom({
        floorId: floorId,

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
      className="bg-white shadow rounded-lg p-5 mb-5"
    >
      {error && <p className="text-red-500 mb-3">{error}</p>}

      <div className="flex gap-3">
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
