import { NavLink } from "react-router-dom";


function Sidebar() {


    return (

        <div className="w-64 min-h-screen bg-gray-900 text-white p-5">


            <h2 className="text-2xl font-bold mb-8">

                GharSewa

            </h2>



            <div className="flex flex-col space-y-4">


                <NavLink

                    to="/"

                    end

                    className={({ isActive }) =>

                        isActive

                            ? "bg-gray-700 p-2 rounded"

                            : "p-2"

                    }

                >

                    Dashboard

                </NavLink>




                <NavLink

                    to="/tenants"

                    className={({ isActive }) =>

                        isActive

                            ? "bg-gray-700 p-2 rounded"

                            : "p-2"

                    }

                >

                    Tenants

                </NavLink>




                <NavLink

                    to="/bills"

                    className={({ isActive }) =>

                        isActive

                            ? "bg-gray-700 p-2 rounded"

                            : "p-2"

                    }

                >

                    Bills

                </NavLink>




                <NavLink

                    to="/payments"

                    className={({ isActive }) =>

                        isActive

                            ? "bg-gray-700 p-2 rounded"

                            : "p-2"

                    }

                >

                    Payments

                </NavLink>


            </div>


        </div>

    );

}


export default Sidebar;