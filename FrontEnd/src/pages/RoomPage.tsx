import { useEffect, useState } from "react";

import Select from "../components/ui/Select";

import DataTable from "../components/ui/DataTable";

import type { House } from "../models/House";

import type { RoomOverview } from "../models/RoomOverview";

import { getHouses } from "../services/houseService";

import { getRoomOverviewByHouse } from "../services/roomService";

import RoomForm from "../components/forms/RoomForm";

function RoomPage() {
  const [houses, setHouses] = useState<House[]>([]);

  const [selectedHouseId, setSelectedHouseId] = useState(0);

  const [floors, setFloors] = useState<RoomOverview[]>([]);

  const [expandedFloors, setExpandedFloors] = useState<number[]>([]);

  const [statusFilter, setStatusFilter] = useState("all");

  useEffect(() => {
    loadHouses();
  }, []);

  const loadHouses = async () => {
    const data = await getHouses();

    setHouses(data);

    if (data.length > 0) {
      const firstHouseId = data[0].id;

      setSelectedHouseId(firstHouseId);

      loadRooms(firstHouseId);
    }
  };

  const loadRooms = async (houseId: number) => {
    const data = await getRoomOverviewByHouse(houseId);

    setFloors(data);
  };

  const toggleFloor = (floorId: number) => {
    if (expandedFloors.includes(floorId)) {
      setExpandedFloors(expandedFloors.filter((id) => id !== floorId));
    } else {
      setExpandedFloors([...expandedFloors, floorId]);
    }
  };

  const expandAll = () => {
    setExpandedFloors(floors.map((floor) => floor.floorId));
  };

  const collapseAll = () => {
    setExpandedFloors([]);
  };

  return (
    <div>
      <h1 className="text-3xl font-bold mb-6">Rooms</h1>

      <div className="flex gap-4 mb-6">
        <Select
          value={selectedHouseId}
          options={houses.map((house) => ({
            label: house.name,

            value: house.id,
          }))}
          onChange={(value) => {
            const id = Number(value);

            setSelectedHouseId(id);

            loadRooms(id);
          }}
        />

        <select
          className="border rounded px-3 py-2"
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
        >
          <option value="all">All</option>

          <option value="occupied">Occupied</option>

          <option value="vacant">Vacant</option>
        </select>

        <button
          onClick={expandAll}
          className="
            border
            px-4
            py-2
            rounded

            cursor-pointer

            hover:bg-gray-200

            active:scale-95

            transition
            duration-150
        "
        >
          Expand All
        </button>

        <button
          onClick={collapseAll}
          className="
            border
            px-4
            py-2
            rounded

            cursor-pointer

            hover:bg-gray-200

            active:scale-95

            transition
            duration-150
        "
        >
          Collapse All
        </button>
      </div>

      <RoomForm
        onRoomCreated={(houseId, floorId) => {
          setSelectedHouseId(houseId);

          loadRooms(houseId);

          setExpandedFloors([floorId]);
        }}
      />

      {floors.map((floor) => (
        <div
          key={floor.floorId}
          className="
          bg-white
          rounded
          shadow
          mb-4
          "
        >
          <div
            onClick={() => toggleFloor(floor.floorId)}
            className="
                p-4
                cursor-pointer
                font-semibold
                flex
                justify-between
                items-center
                border-b
            "
          >
            <span>
              {expandedFloors.includes(floor.floorId) ? "▼" : "▶"}{" "}
              {floor.floorName}
            </span>

            <span>
              {floor.totalRooms} Rooms | {floor.occupiedRooms} Occupied |{" "}
              {floor.vacantRooms} Vacant
            </span>
          </div>

          <div
            className={`
    overflow-hidden
    transition-all
    duration-300
    ease-in-out

    ${
      expandedFloors.includes(floor.floorId)
        ? "max-h-200 opacity-100"
        : "max-h-0 opacity-0"
    }
  `}
          >
            <DataTable columns={["Room", "Status", "Tenant"]}>
              {floor.rooms

                .filter((room) => {
                  if (statusFilter === "occupied") {
                    return room.isOccupied;
                  }

                  if (statusFilter === "vacant") {
                    return !room.isOccupied;
                  }

                  return true;
                })

                .map((room) => (
                  <tr key={room.roomId}>
                    <td className="p-4">{room.roomNumber}</td>

                    <td className="p-4">
                      {room.isOccupied ? "Occupied" : "Vacant"}
                    </td>

                    <td className="p-4">{room.tenantName ?? "-"}</td>
                  </tr>
                ))}
            </DataTable>
          </div>
        </div>
      ))}
    </div>
  );
}

export default RoomPage;
