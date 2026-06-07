import { useEffect, useState } from "react";

import { useNavigate, useParams } from "react-router-dom";

import Button from "../components/ui/Button";

import Card from "../components/ui/Card";

import type { Room } from "../models/Room";

import { getRoomsByFloorId } from "../services/roomService";

function FloorDetailsPage() {
  const { houseId, floorId } = useParams();

  const navigate = useNavigate();

  const [rooms, setRooms] = useState<Room[]>([]);

  const [error, setError] = useState("");

  const loadRooms = async () => {
    if (!floorId && !houseId) {
      return;
    }

    try {
      const data = await getRoomsByFloorId(Number(floorId));

      setRooms(data);
    } catch (error) {
      console.log(error);

      setError("Unable to load rooms");
    }
  };

  useEffect(() => {
    loadRooms();
  }, [floorId]);

  return (
    <div>
      <Button onClick={() => navigate(`/houses/${houseId}`)}>
        ← Back to House
      </Button>

      <h1 className="text-2xl font-bold mt-5 mb-5 ">Floor Details</h1>

      <h2 className="text-xl font-semibold mb-3 ">Rooms</h2>

      {error && <p className="text-red-500 mb-3">{error}</p>}

      <div className="grid grid-cols-3 gap-4">
        {rooms.map((room) => (
          <Card key={room.id}>
            <h3 className="font-bold text-lg ">Room {room.roomNumber}</h3>
          </Card>
        ))}
      </div>
    </div>
  );
}

export default FloorDetailsPage;
