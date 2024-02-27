import "../index.css";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";

dayjs.extend(utc);
dayjs.extend(timezone);

interface WorkOrder {
  workOrderName: string;
  technician: string;
  workType: string;
  status: string;
  endTime: string;
  startTime: string;
}

const ListWorkOrders = ({
  workOrders,
  isLoading,
  timezone,
}: {
  workOrders: Array<WorkOrder>;
  isLoading: boolean;
  timezone: string;
}) => {
  const columns: GridColDef[] = [
    {
      field: "workOrderName",
      headerName: "WorkOrderName",
      width: 200,
    },
    {
      field: "technician",
      headerName: "Technician",
      width: 200,
    },
    {
      field: "workType",
      headerName: "WorkType",
      width: 200,
    },
    {
      field: "status",
      headerName: "Status",
      width: 150,
    },
    {
      field: "startTime",
      headerName: "StartTime",
      width: 250,
      valueFormatter: (params: any) => {
        if (!params.value) return "";
        return dayjs(params.value).tz(timezone).format("YYYY-MM-DD HH:mm:ss");
      },
    },
    {
      field: "endTime",
      headerName: "EndTime",
      width: 300,
      valueFormatter: (params: any) => {
        if (!params.value) return "";
        return dayjs(params.value).tz(timezone).format("YYYY-MM-DD HH:mm:ss");
      },
    },
  ];

  return (
    <div style={{ width: "70vw", height: "70vh" }}>
      <DataGrid
        sx={{
          "& .MuiDataGrid-columnHeaders": {
            fontSize: "1rem"
          },
        }}
        rows={workOrders}
        columns={columns}
        getRowId={(row) => row.workOrderName}
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
              <th style={{ width: "20%" }}>WorkOrderName</th>
              <th style={{ width: "20%" }}>Technician</th>
              <th style={{ width: "15%" }}>WorkType</th>
              <th style={{ width: "15%" }}>Status</th>
              <th style={{ width: "15%" }}>EndTime</th>
              <th style={{ width: "15%" }}>StartTime</th>
            </tr>
          </thead>
          <tbody>
            {!isLoading ? (
              workOrders?.map((workOrder: WorkOrder) => {
                return (
                  <tr className="table-rows" key={workOrder.workOrderName}>
                    <td style={{ width: "20%" }}>{workOrder.workOrderName}</td>
                    <td style={{ width: "20%" }}>{workOrder.technician}</td>
                    <td style={{ width: "15%" }}>{workOrder.workType}</td>
                    <td style={{ width: "15%" }}>{workOrder.status}</td>
                    <td style={{ width: "15%" }}>
                      {new Date(workOrder.startTime).toLocaleDateString(
                        locale
                      ) +
                        " " +
                        new Date(workOrder.startTime).toLocaleTimeString(
                          locale,
                          {
                            hour: "numeric",
                            minute: "numeric",
                            second: "numeric",
                            hour12: false,
                          }
                        )}
                    </td>
                    <td style={{ width: "15%" }}>
                      {new Date(workOrder.endTime).toLocaleDateString(locale) +
                        " " +
                        new Date(workOrder.endTime).toLocaleTimeString(locale, {
                          hour: "numeric",
                          minute: "numeric",
                          second: "numeric",
                          hour12: false,
                        })}
                    </td>
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

export default ListWorkOrders;
