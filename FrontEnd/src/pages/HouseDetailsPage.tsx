import { useEffect, useState } from "react";

import { useNavigate, useParams } from "react-router-dom";

import Button from "../components/ui/Button";

import Card from "../components/ui/Card";

import type { Floor } from "../models/Floor";

import { getFloorsByHouseId } from "../services/floorService";

import FloorForm from "../components/forms/FloorForm";

function HouseDetailsPage() {
  const { houseId } = useParams();

  const navigate = useNavigate();

  const [floors, setFloors] = useState<Floor[]>([]);

  const [error, setError] = useState("");

  const loadFloors = async () => {
    if (!houseId) {
      return;
    }

    try {
      const data = await getFloorsByHouseId(Number(houseId));

      setFloors(data);
    } catch (error) {
      console.log(error);

      setError("Unable to load floors");
    }
  };

  useEffect(() => {
    loadFloors();
  }, [houseId]);

  return (
    <div>
      <Button onClick={() => navigate("/houses")}>← Back to Houses</Button>

      <h1 className="text-2xl font-bold mt-5 mb-5 ">House Details</h1>

      {houseId && (
        <FloorForm houseId={Number(houseId)} onFloorCreated={loadFloors} />
      )}

      <h2 className=" text-xl font-semibold mb-3 ">Floors</h2>

      {error && <p className="text-red-500 mb-3">{error}</p>}

      <div className="grid grid-cols-3 gap-4 ">
        {floors.map((floor) => (
          <Card
            key={floor.id}
            onClick={() => navigate(`/houses/${houseId}/floors/${floor.id}`)}
          >
            <h3 className=" font-bold text-lg ">{floor.name}</h3>

            <p>Floor No: {floor.floorNumber}</p>
          </Card>
        ))}
      </div>
    </div>
  );
}

export default HouseDetailsPage;
