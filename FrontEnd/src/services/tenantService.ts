import api from "../api/axiosConfig";

import type { Tenant } from "../models/Tenant";

import type { CreateTenant } from "../models/CreateTenant";



export const getTenants = async () => {


    const response = await api.get<Tenant[]>(

        "/tenant"

    );


    return response.data;


};




export const createTenant = async (tenant: CreateTenant) => {


    const response = await api.post(

        "/tenant",

        tenant

    );


    return response.data;


};