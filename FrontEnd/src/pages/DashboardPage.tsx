import DashboardCard from "../components/ui/DashboardCard";

function DashboardPage() {
  const dashboardCards = [
    {
      title: "Total Tenants",
      value: "10",
    },

    {
      title: "Pending Bills",
      value: "4",
    },

    {
      title: "Monthly Revenue",
      value: "Rs 50,000",
    },
  ];

  return (
    <div>
      <h1 className="text-3xl font-bold">Dashboard</h1>

      <div className="mt-8 flex gap-5">
        {dashboardCards.map((card) => (
          <DashboardCard
            key={card.title}
            title={card.title}
            value={card.value}
          />
        ))}
      </div>
    </div>
  );
}

export default DashboardPage;
