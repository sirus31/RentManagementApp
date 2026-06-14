import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import HouseForm from "../components/forms/HouseForm";

import type { House } from "../models/House";

import { getHouses } from "../services/houseService";
import Card from "../components/ui/Card";

function HousePage() {
  const navigate = useNavigate();

  const [houses, setHouses] = useState<House[]>([]);

  const loadHouses = async () => {
    const data = await getHouses();

    setHouses(data);
  };

  useEffect(() => {
    loadHouses();
  }, []);

  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Houses</h1>

      <h2 className="text-xl font-semibold mb-3">Create House</h2>

      <HouseForm onHouseCreated={loadHouses} />

      <h2 className="text-xl font-semibold mb-3">Your Houses</h2>

      <div className="grid grid-cols-3 gap-5">
        {houses.map((house) => (
          // <div
          //   key={house.id}
          //   onClick={() => navigate(`/houses/${house.id}`)}
          //   className="bg-white shadow rounded-lg p-5 cursor-pointer"
          // >
          //   <h2 className="text-xl font-bold">{house.name}</h2>

          //   <p>{house.address}</p>

          //   <p>Electricity Rate: Rs {house.electricityRate}/unit</p>

          //   <p>Garbage Fee: Rs {house.garbageFee}</p>
          // </div>
          <Card key={house.id} onClick={() => navigate(`/houses/${house.id}`)}>
            <h2 className="font-bold text-xl">{house.name}</h2>

            <p>{house.address}</p>

            <p>Electricity Rate: Rs {house.electricityRate}/unit</p>

            <p>Garbage Fee: Rs {house.garbageFee}</p>
          </Card>
        ))}
      </div>
    </div>
  );
}

export default HousePage;
