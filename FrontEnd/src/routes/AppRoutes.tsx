import { Route, Routes } from "react-router-dom";

import MainLayout from "../layouts/MainLayout";

import DashboardPage from "../pages/DashboardPage";
import TenantPage from "../pages/TenantPage";
import BillPage from "../pages/BillPage";
import PaymentPage from "../pages/PaymentPage";
import TenantDetailsPage from "../pages/TenantDetailsPage";
import HousePage from "../pages/HousePage";
import HouseDetailsPage from "../pages/HouseDetailsPage";
import FloorDetailsPage from "../pages/FloorDetailsPage";

function AppRoutes() {
  return (
    <Routes>
      <Route element={<MainLayout />}>
        <Route path="/" element={<DashboardPage />} />

        <Route path="/tenants" element={<TenantPage />} />

        <Route path="/tenants/:id" element={<TenantDetailsPage />} />

        <Route path="/bills" element={<BillPage />} />

        <Route path="/payments" element={<PaymentPage />} />

        <Route path="/houses" element={<HousePage />} />

        <Route path="/houses/:houseId" element={<HouseDetailsPage />} />

        <Route
          path="/houses/:houseId/floors/:floorId"
          element={<FloorDetailsPage />}
        />
      </Route>
    </Routes>
  );
}

export default AppRoutes;
