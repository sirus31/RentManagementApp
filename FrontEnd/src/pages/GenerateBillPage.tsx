import { useEffect, useState } from "react";

import { useNavigate, useSearchParams } from "react-router-dom";

import {
  generateMonthlyBill,
  getGenerateBillInfo,
} from "../services/billService";

import type { GenerateBillInfo } from "../models/GenerateBillInfo";

import type { GenerateMonthlyBillRequest } from "../models/GenerateMonthlyBillRequest";

import DataTable from "../components/ui/DataTable";

import Input from "../components/ui/Input";

import Button from "../components/ui/Button";

import { BillingMonth } from "../enums/BillingMonth";

type ExtraCharge = {
  id: number;

  chargeName: string;

  amount: string;

  tenantIds: number[];
};

function GenerateBillPage() {
  const navigate = useNavigate();

  const [searchParams] = useSearchParams();

  const houseId = Number(searchParams.get("houseId"));

  const billingMonth = searchParams.get("month") ?? "";

  const billingYear = searchParams.get("year") ?? "";

  const [billInfo, setBillInfo] = useState<GenerateBillInfo | null>(null);

  const [meterReadings, setMeterReadings] = useState<Record<number, string>>(
    {}
  );

  const [extraCharges, setExtraCharges] = useState<ExtraCharge[]>([]);

  const [extraChargeForm, setExtraChargeForm] = useState<ExtraCharge>({
    id: 0,

    chargeName: "",

    amount: "",

    tenantIds: [],
  });

  const [showExtraChargeForm, setShowExtraChargeForm] = useState(false);

  const [isPreviewMode, setIsPreviewMode] = useState(false);

  const [error, setError] = useState("");

  useEffect(() => {
    loadGenerateInfo();
  }, []);

  const loadGenerateInfo = async () => {
    const data = await getGenerateBillInfo(houseId);

    setBillInfo(data);
  };

  const updateMeterReading = (
    meterId: number,

    value: string
  ) => {
    setMeterReadings({
      ...meterReadings,

      [meterId]: value,
    });
  };

  const validateMeterReadings = () => {
    if (billInfo === null) {
      return false;
    }

    const missingReading = billInfo.meters.some(
      (meter) => !meterReadings[meter.meterId]
    );

    if (missingReading) {
      setError("Please enter all meter readings");

      return false;
    }

    const invalidReading = billInfo.meters.find(
      (meter) => Number(meterReadings[meter.meterId]) < meter.previousReading
    );

    if (invalidReading) {
      setError(
        `Current reading for meter ${invalidReading.meterNumber} cannot be less than previous reading`
      );

      return false;
    }

    setError("");

    return true;
  };

  const toggleTenantSelection = (tenantId: number) => {
    const alreadySelected = extraChargeForm.tenantIds.includes(tenantId);

    setExtraChargeForm({
      ...extraChargeForm,

      tenantIds: alreadySelected
        ? extraChargeForm.tenantIds.filter((id) => id !== tenantId)
        : [...extraChargeForm.tenantIds, tenantId],
    });
  };

  const addExtraCharge = () => {
    setExtraCharges([
      ...extraCharges,

      {
        ...extraChargeForm,

        id: Date.now(),
      },
    ]);

    setExtraChargeForm({
      id: 0,

      chargeName: "",

      amount: "",

      tenantIds: [],
    });

    setShowExtraChargeForm(false);
  };

  const handleGenerateBills = async () => {
    if (billInfo === null) {
      return;
    }

    const missingReading = billInfo.meters.some(
      (meter) => !meterReadings[meter.meterId]
    );

    if (missingReading) {
      setError("Please enter all meter readings");

      return;
    }

    const request: GenerateMonthlyBillRequest = {
      houseId: houseId,

      billingYear: Number(billingYear),

      billingMonth: Number(billingMonth),

      meterReadings: billInfo.meters.map((meter) => ({
        meterId: meter.meterId,

        readingValue: Number(meterReadings[meter.meterId]),
      })),

      extraCharges: extraCharges.map((charge) => ({
        chargeName: charge.chargeName,

        amount: Number(charge.amount),

        tenantIds: charge.tenantIds,
      })),
    };

    try {
      setError("");

      const result = await generateMonthlyBill(request);

      const billCycleId = result[0].billCycleId;

      navigate(`/bills?houseId=${houseId}&cycleId=${billCycleId}`);
    } catch (error: any) {
      setError(error.response?.data ?? "Unable to generate bills");
    }
  };

  if (billInfo === null) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      {error && <p className="text-red-500 mb-4">{error}</p>}

      <h1 className="text-3xl font-bold mb-6">
        {isPreviewMode ? "Preview Monthly Bill" : "Generate Monthly Bill"}
      </h1>

      {/* HOUSE DETAILS */}

      <div className="bg-white shadow rounded-lg p-5 mb-6">
        <h2 className="text-xl font-semibold mb-4">House Details</h2>

        <div className="space-y-2">
          <p>House: {billInfo.house.name}</p>

          <p>Address: {billInfo.house.address}</p>

          <p>Electricity Rate: Rs {billInfo.house.electricityRate}</p>

          <p>Garbage Fee: Rs {billInfo.house.garbageFee}</p>

          <p className="font-semibold pt-3">
            Billing:{" "}
            {BillingMonth[Number(billingMonth) as keyof typeof BillingMonth]}{" "}
            {billingYear}
          </p>
        </div>
      </div>

      {/* TENANTS */}

      <div className="bg-white shadow rounded-lg p-5 mb-6">
        <h2 className="text-xl font-semibold mb-4">Tenants Included</h2>

        <DataTable columns={["Tenant", "Rooms", "Rent", "Assigned Meters"]}>
          {billInfo.tenants.map((tenant) => (
            <tr key={tenant.tenantId}>
              <td className="p-4">{tenant.tenantName}</td>

              <td className="p-4">{tenant.rooms.join(", ")}</td>

              <td className="p-4">Rs {tenant.monthlyRent}</td>

              <td className="p-4">
                {tenant.meters.length > 0 ? tenant.meters.join(", ") : "-"}
              </td>
            </tr>
          ))}
        </DataTable>
      </div>

      {/* METERS */}

      <div className="bg-white shadow rounded-lg p-5 mb-6">
        <h2 className="text-xl font-semibold mb-4">Meter Readings</h2>

        <DataTable
          columns={["Meter", "Type", "Previous Reading", "Current Reading"]}
        >
          {billInfo.meters.map((meter) => (
            <tr key={meter.meterId}>
              <td className="p-4">{meter.meterNumber}</td>

              <td className="p-4">{meter.meterType}</td>

              <td className="p-4">{meter.previousReading}</td>

              <td className="p-4">
                {isPreviewMode ? (
                  meterReadings[meter.meterId]
                ) : (
                  <Input
                    placeholder="Current Reading"
                    type="number"
                    value={meterReadings[meter.meterId] ?? ""}
                    onKeyDown={(e) => {
                      if (
                        e.key === "e" ||
                        e.key === "E" ||
                        e.key === "+" ||
                        e.key === "-"
                      ) {
                        e.preventDefault();
                      }
                    }}
                    onChange={(value) =>
                      updateMeterReading(
                        meter.meterId,

                        value
                      )
                    }
                  />
                )}
              </td>
            </tr>
          ))}
        </DataTable>
      </div>

      {/* EXTRA CHARGES */}

      <div className="bg-white shadow rounded-lg p-5 mb-6">
        <h2 className="text-xl font-semibold mb-4">Extra Charges</h2>

        {!isPreviewMode && (
          <>
            {!showExtraChargeForm && (
              <Button onClick={() => setShowExtraChargeForm(true)}>
                Add Extra Charge
              </Button>
            )}

            {showExtraChargeForm && (
              <div className="space-y-5">
                <div className="flex gap-4">
                  <Input
                    placeholder="Charge Name"
                    value={extraChargeForm.chargeName}
                    onChange={(value) =>
                      setExtraChargeForm({
                        ...extraChargeForm,

                        chargeName: value,
                      })
                    }
                  />

                  <Input
                    placeholder="Amount"
                    type="number"
                    value={extraChargeForm.amount}
                    onKeyDown={(e) => {
                      if (
                        e.key === "e" ||
                        e.key === "E" ||
                        e.key === "+" ||
                        e.key === "-"
                      ) {
                        e.preventDefault();
                      }
                    }}
                    onChange={(value) =>
                      setExtraChargeForm({
                        ...extraChargeForm,

                        amount: value,
                      })
                    }
                  />
                </div>

                <div className="flex gap-6">
                  {billInfo.tenants.map((tenant) => (
                    <label
                      key={tenant.tenantId}
                      className="
                          flex
                          items-center
                          gap-2
                        "
                    >
                      <input
                        type="checkbox"
                        checked={extraChargeForm.tenantIds.includes(
                          tenant.tenantId
                        )}
                        onChange={() => toggleTenantSelection(tenant.tenantId)}
                      />

                      {tenant.tenantName}
                    </label>
                  ))}
                </div>

                <Button onClick={addExtraCharge}>Add Charge</Button>
              </div>
            )}
          </>
        )}

        {extraCharges.length > 0 && (
          <DataTable columns={["Charge", "Amount", "Tenants"]}>
            {extraCharges.map((charge) => (
              <tr key={charge.id}>
                <td className="p-4">{charge.chargeName}</td>

                <td className="p-4">Rs {charge.amount}</td>

                <td className="p-4">
                  {billInfo.tenants

                    .filter((tenant) =>
                      charge.tenantIds.includes(tenant.tenantId)
                    )

                    .map((tenant) => tenant.tenantName)

                    .join(", ")}
                </td>
              </tr>
            ))}
          </DataTable>
        )}
      </div>

      {/* PREVIOUS DUES */}

      {isPreviewMode && (
        <div className="bg-white shadow rounded-lg p-5 mb-6">
          <h2 className="text-xl font-semibold mb-4">Previous Dues</h2>

          {billInfo.previousDues.length > 0 ? (
            <DataTable columns={["Tenant", "Bill Cycle", "Due Amount"]}>
              {billInfo.previousDues.map((due) => (
                <tr
                  key={`${due.tenantId}-${due.billingMonth}-${due.billingYear}`}
                >
                  <td className="p-4">{due.tenantName}</td>

                  <td className="p-4">
                    {due.billingMonth} {due.billingYear}
                  </td>

                  <td className="p-4">Rs {due.dueAmount}</td>
                </tr>
              ))}
            </DataTable>
          ) : (
            <p className="text-gray-500">No previous dues pending</p>
          )}
        </div>
      )}

      {/* BUTTONS */}

      <div className="flex justify-end gap-3">
        {isPreviewMode && (
          <Button onClick={() => setIsPreviewMode(false)}>Edit Details</Button>
        )}

        <Button
          onClick={() => {
            if (isPreviewMode) {
              handleGenerateBills();
            } else {
              const valid = validateMeterReadings();

              if (valid) {
                setIsPreviewMode(true);
              }
            }
          }}
        >
          {isPreviewMode ? "Generate Bills" : "Preview"}
        </Button>
      </div>
    </div>
  );
}

export default GenerateBillPage;
