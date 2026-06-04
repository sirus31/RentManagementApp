import api from "../api/axiosConfig";

import type { Tenant } from "../models/Tenant";


export const getTenants = async () => {


    const response = await api.get<Tenant[]>(

        "/tenant"

    );


    return response.data;


};