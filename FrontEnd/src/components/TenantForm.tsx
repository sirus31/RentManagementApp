import { useState } from "react";

import type { CreateTenant } from "../models/CreateTenant";

import { createTenant } from "../services/tenantService";



type TenantFormProps = {


    onTenantCreated: () => void;


};



function TenantForm(props: TenantFormProps) {


    const [tenant, setTenant] = useState<CreateTenant>({

        fullName: "",

        phoneNumber: "",

        monthlyRent: 0

    });



    const handleSave = async () => {


        if (tenant.phoneNumber.length !== 10) {


            alert("Phone number must be 10 digits");


            return;


        }



        await createTenant(tenant);



        props.onTenantCreated();



        setTenant({


            fullName: "",


            phoneNumber: "",


            monthlyRent: 0


        });


    };




    return (

        <div className="bg-white p-5 rounded-lg shadow mb-6">


            <input

                className="border p-2 mr-3"

                placeholder="Full Name"

                value={tenant.fullName}

                onChange={(e) =>

                    setTenant({

                        ...tenant,

                        fullName: e.target.value

                    })

                }

            />





            <input

                className="border p-2 mr-3"

                placeholder="Phone Number"

                value={tenant.phoneNumber}

                maxLength={10}

                onChange={(e) => {


                    const value = e.target.value;



                    if (/^\d*$/.test(value)) {


                        setTenant({


                            ...tenant,


                            phoneNumber: value


                        });


                    }


                }}

            />






            <input

                className="border p-2 mr-3"

                placeholder="Rent"

                type="number"

                value={tenant.monthlyRent}

                onChange={(e) =>

                    setTenant({


                        ...tenant,


                        monthlyRent: Number(e.target.value)


                    })

                }

            />






            <button

                onClick={handleSave}

                className="bg-blue-600 text-white px-4 py-2 rounded"

            >


                Save


            </button>




        </div>

    );

}



export default TenantForm;