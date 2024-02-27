import "../index.css";
import { DataGrid, GridColDef } from "@mui/x-data-grid";

interface Technician {
  technicianId: number;
  name: string;
  address: string;
}

// position:"relative", top:"5%",
const ListTechnicians = ({
  technicians,
  isLoading,
}: {
  technicians: Array<Technician>;
  isLoading: boolean;
}) => {
  const columns: GridColDef[] = [
    {
      field: "technicianId",
      headerName: "TechnicianId",
      width: 200,
    },
    {
      field: "name",
      headerName: "Name",
      width: 300,
    },
    {
      field: "address",
      headerName: "Address",
      width: 400,
    },
  ];

  return (
    <div style={{ width: "70vw", height: "70vh" }}>
      <DataGrid
        sx={{
          "& .MuiDataGrid-columnHeaders": {
            fontSize: "1rem",
          },
        }}
        rows={technicians}
        columns={columns}
        getRowId={(row) => row.technicianId}
        pageSizeOptions={[20, 50, 100]}
        initialState={{
          pagination: { paginationModel: { pageSize: 20 } },
        }}
        loading={isLoading}
        localeText={{
          noRowsLabel: "No work orders found",
        }}
      />
      {/* <div style={{ overflowY: "scroll", width: "100%", height: "70vh" }}>
        <table className="content-table">
          <thead>
            <tr className="table-headers">
              <th style={{ width: "20%" }}>TechnicianId</th>
              <th style={{ width: "40%" }}>Name</th>
              <th style={{ width: "40%" }}>Address</th>
            </tr>
          </thead>
          <tbody>
            {!isLoading ? (
              technicians?.map((technician: Technician) => {
                return (
                  <tr className="table-rows" key={technician.technicianId}>
                    <td style={{ width: "20%" }}>{technician.technicianId}</td>
                    <td style={{ width: "40%" }}>{technician.name}</td>
                    <td style={{ width: "40%" }}>{technician.address}</td>
                  </tr>
                );
              })
            ) : (
              <tr>
                <td>Loading...</td>
              </tr>
            )}
          </tbody>
        </table>
      </div> */}
    </div>
  );
};

export default ListTechnicians;
