import "../index.css";

interface Technician {
  technicianId: number;
  name: string;
  address: string;
}

interface ListTechniciansProps {
  technicians: Technician[];
}
// position:"relative", top:"5%",
const ListTechnicians: React.FC<ListTechniciansProps> = ({ technicians }) => {
  return (
    <div style={{ width: "70vw" }}>
      <div style={{ overflowY: "scroll", width: "100%", height: "70vh" }}>
        <table className="content-table">
          <thead>
            <tr className="table-headers">
              <th style={{ width: "20%" }}>TechnicianId</th>
              <th style={{ width: "40%" }}>Name</th>
              <th style={{ width: "40%" }}>Address</th>
            </tr>
          </thead>
          <tbody>
            {technicians.map((technician: Technician) => {
              return (
                <tr className="table-rows" key={technician.technicianId}>
                  <td style={{ width: "20%" }}>{technician.technicianId}</td>
                  <td style={{ width: "40%" }}>{technician.name}</td>
                  <td style={{ width: "40%" }}>{technician.address}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default ListTechnicians;
