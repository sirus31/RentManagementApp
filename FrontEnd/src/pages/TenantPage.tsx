import { useEffect, useState } from "react";

import type { Tenant } from "../models/Tenant";

import { getTenants } from "../services/tenantService";

import DataTable from "../components/DataTable";


function TenantPage() {


    const [tenants, setTenants] = useState<Tenant[]>([]);



    useEffect(() => {


        loadTenants();


    }, []);




    const loadTenants = async () => {


        const data = await getTenants();


        setTenants(data);


    };




    return (

        <div>


            <h1 className="text-3xl font-bold mb-6">

                Tenants

            </h1>




            <DataTable

                columns={[

                    "Name",

                    "Phone",

                    "Rent"

                ]}

            >


                {


                    tenants.map((tenant) => (


                        <tr

                            key={tenant.id}

                            className="border-b"

                        >


                            <td className="p-4">

                                {tenant.fullName}

                            </td>



                            <td className="p-4">

                                {tenant.phoneNumber}

                            </td>



                            <td className="p-4">

                                Rs {tenant.monthlyRent}

                            </td>


                        </tr>


                    ))


                }


            </DataTable>


        </div>

    );

}


export default TenantPage;