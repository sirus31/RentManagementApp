import { useState } from "react";

import Modal from "../ui/Modal";

import Button from "../ui/Button";

import { createPayment } from "../../services/paymentService";

import type { PendingPayment } from "../../models/PaymentDashboard";

type Props = {
  bill: PendingPayment;

  onClose: () => void;

  onSuccess: () => void;
};

function ReceivePaymentModal({
  bill,

  onClose,

  onSuccess,
}: Props) {
  const [amount, setAmount] = useState(bill.pendingAmount.toString());

  const [paymentMode, setPaymentMode] = useState("Cash");

  const [notes, setNotes] = useState("");

  const [error, setError] = useState("");

  const savePayment = async () => {
    setError("");

    const paymentAmount = Number(amount);

    if (!paymentAmount || paymentAmount <= 0) {
      setError("Please enter a valid payment amount");

      return;
    }

    if (paymentAmount > bill.pendingAmount) {
      setError("Payment amount cannot exceed remaining balance");

      return;
    }

    try {
      await createPayment({
        billId: bill.billId,

        amount: paymentAmount,

        paymentMode,

        notes,
      });

      await onSuccess();

      onClose();
    } catch {
      setError("Unable to record payment. Please try again.");
    }
  };

  return (
    <Modal title="Receive Payment" onClose={onClose}>
      <div className="space-y-4">
        <div
          className="
            bg-gray-50
            p-4
            rounded
          "
        >
          <p>Tenant: {bill.tenantName}</p>

          <p>
            Bill: {bill.billingMonth} {bill.billingYear}
          </p>

          <p>Total Bill: Rs {bill.totalAmount}</p>

          <p>Already Paid: Rs {bill.amountPaid}</p>

          <p className="font-bold">Remaining: Rs {bill.pendingAmount}</p>
        </div>

        {error && (
          <p
            className="
              bg-red-100
              text-red-700
              p-3
              rounded
            "
          >
            {error}
          </p>
        )}

        <input
          className="
            border
            p-2
            rounded
            w-full
          "
          placeholder="Payment Amount"
          value={amount}
          onChange={(event) => setAmount(event.target.value)}
        />

        <select
          className="
            border
            p-2
            rounded
            w-full
          "
          value={paymentMode}
          onChange={(event) => setPaymentMode(event.target.value)}
        >
          <option>Cash</option>

          <option>Bank Transfer</option>

          <option>Esewa</option>
        </select>

        <input
          className="
            border
            p-2
            rounded
            w-full
          "
          placeholder="Notes"
          value={notes}
          onChange={(event) => setNotes(event.target.value)}
        />

        <Button onClick={savePayment}>Save Payment</Button>
      </div>
    </Modal>
  );
}

export default ReceivePaymentModal;
