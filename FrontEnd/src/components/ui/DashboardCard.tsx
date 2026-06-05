type DashboardCardProps = {
  title: string;
  value: string;
};

function DashboardCard(props: DashboardCardProps) {
  return (
    <div className="bg-white p-5 rounded-lg shadow">
      <h3 className="text-gray-500">{props.title}</h3>

      <p className="text-2xl font-bold mt-2">{props.value}</p>
    </div>
  );
}

export default DashboardCard;
