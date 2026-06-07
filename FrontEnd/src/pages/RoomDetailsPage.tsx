import { useEffect, useState } from "react";

import { useNavigate, useParams } from "react-router-dom";

import type { Occupancy } from "../models/Occupancy";

import { getActiveOccupancies, vacateRoom } from "../services/occupancyService";

import AssignTenantForm from "../components/forms/AssignTenantForm";

import Button from "../components/ui/Button";

function RoomDetailsPage() {
  const { houseId, floorId, roomId } = useParams();

  const navigate = useNavigate();

  const [occupancy, setOccupancy] = useState<Occupancy | null>(null);

  const loadOccupancy = async () => {
    const data = await getActiveOccupancies();

    const currentRoomOccupancy = data.find(
      (item) => item.roomId === Number(roomId)
    );

    setOccupancy(currentRoomOccupancy ?? null);
  };

  const handleVacate = async () => {
    if (!occupancy) {
      return;
    }

    try {
      await vacateRoom({
        tenantRoomId: occupancy.tenantRoomId,

        endDate: new Date().toISOString().split("T")[0],
      });

      loadOccupancy();
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    loadOccupancy();
  }, []);

  return (
    <div>
      <Button onClick={() => navigate(`/houses/${houseId}/floors/${floorId}`)}>
        ← Back To Floor
      </Button>

      <h1 className="text-2xl font-bold my-5">Room Details</h1>

      {occupancy ? (
        <div>
          <h2 className="font-bold">Current Tenant</h2>

          <p>{occupancy.tenantName}</p>

          <p>Moved In: {occupancy.startDate}</p>
          <Button onClick={handleVacate}>Vacate Tenant</Button>
        </div>
      ) : (
        <AssignTenantForm
          roomId={Number(roomId)}
          onTenantAssigned={loadOccupancy}
        />
      )}
    </div>
  );
}

export default RoomDetailsPage;
