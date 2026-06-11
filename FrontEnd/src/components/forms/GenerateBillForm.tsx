import { useEffect, useState } from "react";

import { useNavigate } from "react-router-dom";

import Select from "../ui/Select";

import Input from "../ui/Input";

import Button from "../ui/Button";

import { getHouses } from "../../services/houseService";

import { validateBillCycle } from "../../services/billService";

import type { House } from "../../models/House";

function GenerateBillForm() {
  const navigate = useNavigate();

  const [houses, setHouses] = useState<House[]>([]);

  const [houseId, setHouseId] = useState("");

  const [billingMonth, setBillingMonth] = useState("");

  const [billingYear, setBillingYear] = useState("");

  const [error, setError] = useState("");

  const [isCycleValid, setIsCycleValid] = useState(true);

  useEffect(() => {
    loadHouses();
  }, []);

  useEffect(() => {
    validateCycle();
  }, [houseId, billingMonth, billingYear]);

  const loadHouses = async () => {
    const data = await getHouses();

    setHouses(data);

    if (data.length > 0) {
      setHouseId(String(data[0].id));
    }
  };

  const validateCycle = async () => {
    if (!houseId || !billingMonth || !billingYear) {
      setError("");

      setIsCycleValid(true);

      return;
    }

    const result = await validateBillCycle(
      Number(houseId),

      billingMonth,

      billingYear
    );

    setIsCycleValid(result.isValid);

    if (!result.isValid) {
      setError(result.message);
    } else {
      setError("");
    }
  };

  const generateBill = () => {
    if (!houseId || !billingMonth || !billingYear) {
      setError("Please select all fields");

      return;
    }

    if (!isCycleValid) {
      return;
    }

    navigate(
      `/bills/generate?houseId=${houseId}&month=${billingMonth}&year=${billingYear}`
    );
  };

  return (
    <div
      className="
        bg-white
        shadow
        rounded-lg
        p-5
        mb-5
      "
    >
      <h2 className="text-xl font-semibold mb-4">Generate Monthly Bill</h2>

      {error && <p className="text-red-500 mb-3">{error}</p>}

      <div className="flex gap-3">
        <Select
          value={houseId}
          options={houses.map((house) => ({
            label: house.name,

            value: house.id,
          }))}
          onChange={(value) => setHouseId(value)}
        />

        <Select
          value={billingMonth}
          options={[
            { label: "Select Month", value: "" },

            { label: "Baisakh", value: "1" },

            { label: "Jestha", value: "2" },

            { label: "Ashadh", value: "3" },

            { label: "Shrawan", value: "4" },

            { label: "Bhadra", value: "5" },

            { label: "Ashwin", value: "6" },

            { label: "Kartik", value: "7" },

            { label: "Mangsir", value: "8" },

            { label: "Poush", value: "9" },

            { label: "Magh", value: "10" },

            { label: "Falgun", value: "11" },

            { label: "Chaitra", value: "12" },
          ]}
          onChange={(value) => setBillingMonth(value)}
        />

        <Input
          placeholder="Billing Year"
          type="number"
          value={billingYear}
          onKeyDown={(event) => {
            if (
              event.key === "e" ||
              event.key === "E" ||
              event.key === "+" ||
              event.key === "-"
            ) {
              event.preventDefault();
            }
          }}
          onChange={(value) => setBillingYear(value)}
        />

        <Button onClick={generateBill}>Generate</Button>
      </div>
    </div>
  );
}

export default GenerateBillForm;
