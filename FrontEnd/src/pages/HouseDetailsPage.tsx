import { useEffect, useState } from "react";

import { useNavigate, useParams } from "react-router-dom";

import Button from "../components/ui/Button";

import Card from "../components/ui/Card";

import type { Floor } from "../models/Floor";

import { getFloorsByHouseId } from "../services/floorService";

import type { Meter } from "../models/Meter";

import MeterForm from "../components/forms/MeterForm";

import { getMetersByHouse } from "../services/meterService";

import FloorForm from "../components/forms/FloorForm";

function HouseDetailsPage() {
  const { houseId } = useParams();

  const navigate = useNavigate();

  const [floors, setFloors] = useState<Floor[]>([]);

  const [meters, setMeters] = useState<Meter[]>([]);

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

  const loadMeters = async () => {
    const data = await getMetersByHouse(Number(houseId));

    setMeters(data);
  };

  useEffect(() => {
    loadFloors();
    loadMeters();
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

      <h2 className="text-xl font-bold mt-8 mb-4">Meters</h2>

      <MeterForm
        houseId={Number(houseId)}
        onMeterCreated={() => {
          loadMeters();
        }}
      />

      <div className="grid grid-cols-3 gap-4">
        {meters.map((meter) => (
          <Card key={meter.id}>
            <h3 className="font-bold text-lg">{meter.meterNumber}</h3>

            <p>Type: {meter.meterType}</p>

            <p>Initial Reading: {meter.initialReading}</p>

            <p>Status: {meter.isActive ? "Active" : "Inactive"}</p>
          </Card>
        ))}
      </div>
    </div>
  );
}

export default HouseDetailsPage;
