type DataTableProps = {


    columns: string[];


    children: React.ReactNode;


};



function DataTable(props: DataTableProps) {


    return (

        <div className="bg-white rounded-lg shadow">


            <table className="w-full">


                <thead>


                    <tr className="border-b">


                        {

                            props.columns.map((column) => (


                                <th

                                    key={column}

                                    className="text-left p-4"

                                >


                                    {column}


                                </th>


                            ))

                        }


                    </tr>


                </thead>




                <tbody>


                    {props.children}


                </tbody>


            </table>


        </div>

    );

}



export default DataTable;