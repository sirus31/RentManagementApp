import Modal from "../ui/Modal";

import type { BillFullDetail } from "../../models/BillFullDetail";

type Props = {
  bill: BillFullDetail;

  onClose: () => void;
};

function BillDetails({ bill, onClose }: Props) {
  return (
    <Modal title={`${bill.tenantName} Bill Details`} onClose={onClose}>
      <div className="space-y-2">
        <p>Rent: Rs {bill.rentAmount}</p>

        <p>Rooms: {bill.rooms.join(", ")}</p>

        <p>Electricity: Rs {bill.electricityAmount}</p>

        <p>Garbage: Rs {bill.garbageAmount}</p>

        <p className="font-bold">Total: Rs {bill.totalAmount}</p>
      </div>

      <hr className="my-4" />

      <h3 className="font-semibold mb-3">Breakdown</h3>

      {bill.details.map((detail, index) => (
        <div
          key={index}
          className="
            border
            rounded
            p-3
            mb-3
          "
        >
          <p className="font-semibold">{detail.description}</p>

          <p>Previous Reading: {detail.previousReading}</p>

          <p>Current Reading: {detail.currentReading}</p>

          <p>Units: {detail.unitsConsumed}</p>

          <p>Rate: Rs {detail.rate}</p>

          <p>Amount: Rs {detail.amount}</p>
        </div>
      ))}
    </Modal>
  );
}

export default BillDetails;
