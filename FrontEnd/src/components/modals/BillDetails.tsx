import Modal from "../ui/Modal";

import DataTable from "../ui/DataTable";

import type { BillFullDetail } from "../../models/BillFullDetail";

type Props = {
  bill: BillFullDetail;

  onClose: () => void;
};

function BillDetails({ bill, onClose }: Props) {
  const electricityDetails = bill.details.filter(
    (detail) => detail.detailType === "Electricity"
  );

  const extraCharges = bill.details.filter(
    (detail) => detail.detailType === "Maintenance"
  );

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

      {/* EXTRA CHARGES */}

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

      {/* SUMMARY */}

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
      </div>
    </Modal>
  );
}

export default BillDetails;
