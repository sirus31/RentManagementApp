import Modal from "../ui/Modal";

import DataTable from "../ui/DataTable";

import Select from "../ui/Select";

import type { BillFullDetail } from "../../models/BillFullDetail";

import { useState } from "react";

import Input from "../ui/Input";

import Button from "../ui/Button";

import { createPayment } from "../../services/paymentService";

type Props = {
  bill: BillFullDetail;

  onClose: () => void;

  onRefresh: () => Promise<void>;
};

function BillDetails({ bill, onClose, onRefresh }: Props) {
  const [showPaymentForm, setShowPaymentForm] = useState(false);

  const [amount, setAmount] = useState("");

  const [paymentMode, setPaymentMode] = useState("Cash");

  const [notes, setNotes] = useState("");

  const [error, setError] = useState("");

  const electricityDetails = bill.details.filter(
    (detail) => detail.detailType === "Electricity"
  );

  const extraCharges = bill.details.filter(
    (detail) => detail.detailType === "Maintenance"
  );

  const recordPayment = async () => {
    if (!amount || Number(amount) <= 0) {
      setError("Enter a valid payment amount");

      return;
    }

    if (Number(amount) > bill.pendingAmount) {
      setError("Payment cannot exceed remaining amount");

      return;
    }
    try {
      setError("");

      await createPayment({
        billId: bill.billId,

        amount: Number(amount),

        paymentMode,

        notes,
      });

      await onRefresh();

      setShowPaymentForm(false);

      setAmount("");

      setNotes("");
    } catch (error: any) {
      setError(error.response?.data ?? "Failed to record payment");
    }
  };

  return (
    <Modal title={`${bill.tenantName} Bill Details`} onClose={onClose}>
      {/* TENANT DETAILS */}

      <div
        className="
          bg-gray-50
          rounded
          p-4
          mb-5
        "
      >
        <h3 className="font-semibold mb-3">Tenant Details</h3>

        <div className="grid grid-cols-2 gap-3">
          <p>Tenant: {bill.tenantName}</p>

          <p>Rooms: {bill.rooms.join(", ")}</p>

          <p>
            Billing: {bill.billingMonth} {bill.billingYear}
          </p>

          <p>Rent: Rs {bill.rentAmount}</p>

          <p>Garbage: Rs {bill.garbageAmount}</p>
        </div>
      </div>

      {/* ELECTRICITY */}

      <div className="mb-5">
        <h3 className="font-semibold mb-3">Electricity Usage</h3>

        {electricityDetails.length > 0 ? (
          <DataTable
            columns={[
              "Meter",
              "Previous",
              "Current",
              "Total Units",
              "Shared By",
              "Your Units",
              "Rate",
              "Amount",
            ]}
          >
            {electricityDetails.map((detail, index) => (
              <tr key={index}>
                <td className="p-4">{detail.description}</td>

                <td className="p-4">{detail.previousReading}</td>

                <td className="p-4">{detail.currentReading}</td>

                <td className="p-4">{detail.unitsConsumed}</td>

                <td className="p-4">
                  {detail.sharedTenantCount > 1
                    ? `${detail.sharedTenantCount} tenants`
                    : "-"}
                </td>

                <td className="p-4">{detail.tenantUnits}</td>

                <td className="p-4">Rs {detail.rate}</td>

                <td className="p-4">Rs {detail.amount}</td>
              </tr>
            ))}
          </DataTable>
        ) : (
          <p className="text-gray-500">No electricity charges</p>
        )}
      </div>

      <div className="mb-5">
        <h3 className="font-semibold mb-3">Extra Charges</h3>

        {extraCharges.length > 0 ? (
          <DataTable columns={["Description", "Amount"]}>
            {extraCharges.map((charge, index) => (
              <tr key={index}>
                <td className="p-4">{charge.description}</td>

                <td className="p-4">Rs {charge.amount}</td>
              </tr>
            ))}
          </DataTable>
        ) : (
          <p className="text-gray-500">No extra charges</p>
        )}
      </div>

      <div
        className="
          border-t
          pt-4
          space-y-2
        "
      >
        <h3 className="font-semibold">Bill Summary</h3>

        <p>Rent: Rs {bill.rentAmount}</p>

        <p>Electricity: Rs {bill.electricityAmount}</p>

        <p>Garbage: Rs {bill.garbageAmount}</p>

        <p>Extra Charges: Rs {bill.extraChargeAmount}</p>

        <p>Previous Due: Rs {bill.previousDueAmount}</p>

        <hr />

        <p className="font-bold">Total: Rs {bill.totalAmount}</p>

        <p>Paid: Rs {bill.amountPaid}</p>

        <p className="font-bold">Remaining: Rs {bill.pendingAmount}</p>

        <div
          className="
    border-t
    mt-5
    pt-4
  "
        >
          <div
            className="
      flex
      justify-between
      items-center
      mb-4
    "
          >
            <h3 className="font-semibold">Payments</h3>

            {bill.pendingAmount > 0 ? (
              <Button onClick={() => setShowPaymentForm(true)}>
                Record Payment
              </Button>
            ) : (
              <p className="font-semibold text-green-600">✓ Fully Paid</p>
            )}
          </div>

          {error && <p className="text-red-500 mb-3">{error}</p>}

          {showPaymentForm && (
            <div
              className="
        bg-gray-50
        p-4
        rounded
        mb-5
        space-y-3
      "
            >
              <Input
                placeholder="Payment Amount"
                type="number"
                value={amount}
                onChange={(value) => setAmount(value)}
              />

              <Select
                value={paymentMode}
                options={[
                  {
                    label: "Cash",
                    value: "Cash",
                  },

                  {
                    label: "Bank Transfer",
                    value: "Bank Transfer",
                  },

                  {
                    label: "Online",
                    value: "Online",
                  },

                  {
                    label: "Cheque",
                    value: "Cheque",
                  },
                ]}
                onChange={(value) => setPaymentMode(value)}
              />

              <Input
                placeholder="Notes"
                value={notes}
                onChange={(value) => setNotes(value)}
              />

              <div className="flex gap-3">
                <Button onClick={recordPayment}>Save Payment</Button>

                <Button onClick={() => setShowPaymentForm(false)}>
                  Cancel
                </Button>
              </div>
            </div>
          )}

          {bill.payments.length > 0 ? (
            <DataTable columns={["Date", "Amount", "Mode", "Notes"]}>
              {bill.payments.map((payment) => (
                <tr key={payment.id}>
                  <td className="p-4">
                    {new Date(payment.paymentDate).toLocaleDateString()}
                  </td>

                  <td className="p-4">Rs {payment.amount}</td>

                  <td className="p-4">{payment.paymentMode}</td>

                  <td className="p-4">{payment.notes || "-"}</td>
                </tr>
              ))}
            </DataTable>
          ) : (
            <p className="text-gray-500">No payments recorded</p>
          )}
        </div>
      </div>
    </Modal>
  );
}

export default BillDetails;
