import "../index.css";

interface WorkOrder {
  workOrderName: string;
  technician: string;
  workType: string;
  status: string;
  endTime: string;
  startTime: string;
}

interface ListWorkOrdersProps {
  workOrders: WorkOrder[];
}

const ListWorkOrders: React.FC<ListWorkOrdersProps> = ({ workOrders }) => {
  return (
    <div style={{ width: "70vw" }}>
      <div style={{ overflowY: "scroll", width: "100%", height: "70vh" }}>
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
            {workOrders.map((workOrder: WorkOrder) => {
              return (
                <tr className="table-rows" key={workOrder.workOrderName}>
                  <td style={{ width: "20%" }}>{workOrder.workOrderName}</td>
                  <td style={{ width: "20%" }}>{workOrder.technician}</td>
                  <td style={{ width: "15%" }}>{workOrder.workType}</td>
                  <td style={{ width: "15%" }}>{workOrder.status}</td>
                  <td style={{ width: "15%" }}>{workOrder.startTime}</td>
                  <td style={{ width: "15%" }}>{workOrder.endTime}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ListWorkOrders;
