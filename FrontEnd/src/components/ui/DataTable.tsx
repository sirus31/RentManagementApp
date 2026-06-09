type DataTableProps = {
  columns: string[];

  children: React.ReactNode;
};

function DataTable(props: DataTableProps) {
  return (
    <div
      className="
      bg-white
      rounded-lg
      shadow
      overflow-hidden
      "
    >
      <table
        className="
        w-full
        table-fixed
        "
      >
        <thead>
          <tr
            className="
            border-b
            "
          >
            {props.columns.map((column) => (
              <th
                key={column}
                className="
                  text-left
                  p-4
                  font-semibold
                  "
              >
                {column}
              </th>
            ))}
          </tr>
        </thead>

        <tbody>{props.children}</tbody>
      </table>
    </div>
  );
}

export default DataTable;
