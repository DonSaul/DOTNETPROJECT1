import { useState, useEffect } from "react";
import ListWorkOrders from "../components/ListWorkOrders";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

interface WorkOrder {
  workOrderName: string;
  technician: string;
  workType: string;
  status: string;
  endTime: string;
  startTime: string;
}

const WorkOrders = () => {
  const [workOrders, setWorkOrders] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  const [search, setSearch] = useState("");
  const [foundWorkOrder, setFoundWorkOrder] = useState({} as WorkOrder);

  //filters
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [workType, setWorkType] = useState("");
  const [status, setStatus] = useState("");

  useEffect(() => {
    const getWorkOrders = async () => {
      setIsLoading(true);
      const response = await fetch(
        "https://localhost:7178/api/WorkOrderDetails/all"
      );
      // console.log(response)
      const data = await response.json();
      setWorkOrders(data);
      setIsLoading(false);
    };

    getWorkOrders();
  }, []);

  const handleSearch = async () => {
    if (search === "") {
      return;
    }
    const response = await fetch(
      `https://localhost:7178/api/WorkOrderDetails/${search}`
    );
    if (response.ok) {
      const dialog = document.querySelector("dialog");

      const closeButton = document.querySelector("dialog button");
      closeButton?.addEventListener("click", () => {
        dialog?.close();
      });

      setFoundWorkOrder(await response.json());

      dialog?.showModal();

      toast.success(`Found work order: ${search}`, { theme: "dark" });
      const data = await response.json();
      setFoundWorkOrder(data);
    } else {
      toast.error(`Work order not found: ${search}`, { theme: "dark" });
      return;
    }
  };

  const handleFilter = async () => {
    if (startDate === "" || endDate === "") {
      return;
    }

    const fetchPromise = fetch(
      `https://localhost:7178/api/WorkOrderDetails?startTime=${startDate}&endTime=${endDate}&workType=${workType}&status=${status}`,
      {
        method: "GET",
      }
    ).then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    });

    toast.promise(
      fetchPromise,
      {
        pending: "Loading...",
        success: "Applied filters successfully",
      },
      { position: "top-right", theme: "dark", pauseOnHover: false }
    );

    fetchPromise
      .then((data) => {
        setWorkOrders(data);
      })
      .catch((error) => {
        toast.error(`Fetch failed: ${error.message}`, {
          position: "top-right",
          theme: "dark",
          pauseOnHover: false,
        });
      });

    // const response = await toast.promise(
    //   fetch(`https://localhost:7178/api/WorkOrderDetails`, {
    //     method: "POST",
    //     body: JSON.stringify(body),
    //   }),
    //   { pending: "Loading...", success: "Success!", error: "Error!" },
    //   { position: "top-right", theme: "dark" }
    // );

    // if (response.ok) {
    //   const data = await response.json();
    //   setWorkOrders(data);
    // }
  };

  return (
    <section>
      <ToastContainer />
      <dialog id="workOrderDialog">
        <p>Work Order Name: {foundWorkOrder.workOrderName}</p>
        <p>Technician: {foundWorkOrder.technician}</p>
        <p>Work Type: {foundWorkOrder.workType}</p>
        <p>Status: {foundWorkOrder.status}</p>
        <p>Start Time: {foundWorkOrder.startTime}</p>
        <p>End Time: {foundWorkOrder.endTime}</p>
        <button>Close</button>
      </dialog>
      <div className="toolbar-container">
        <h1 className="my-heading">Work Orders</h1>
        <div
          style={{
            display: "flex",
            width: "100%",
            justifyContent: "flex-end",
            paddingTop: "1rem",
          }}
        >
          <input
            type="text"
            pattern="^[a-zA-Z\s]*$"
            title="Only enter letters and spaces."
            onChange={(e) => {
              setSearch(e.target.value);
            }}
          />
          <button
            style={{ marginLeft: "1rem", marginRight: "1rem" }}
            type="submit"
            onClick={(e) => {
              e.preventDefault();
              handleSearch();
            }}
          >
            Search by Name
          </button>
          <button
            style={{ marginLeft: "0.5rem", marginRight: "0.5rem" }}
            onClick={(e) => {
              e.preventDefault();
              // this is standard download procedure:
              // fetch the file,
              // create a blob URL representing the file and append it to doc,
              // click on the URL to download the file,
              // remove the link from the doc
              fetch("https://localhost:7178/api/WorkOrder/export-csv")
                .then((response) => response.blob())
                .then((blob) => {
                  const url = URL.createObjectURL(blob);
                  const a = document.createElement("a");
                  a.href = url;
                  a.download = "report.csv";
                  document.body.appendChild(a);
                  a.click();
                  setTimeout(() => document.body.removeChild(a), 0);
                  toast.success("Exported to CSV successfully", {
                    theme: "dark",
                  });
                })
                .catch((error) =>
                  toast.error(`Fetch failed: ${error.message}`, {
                    theme: "dark",
                  })
                );
            }}
          >
            Export to CSV
          </button>
        </div>
      </div>

      <div className="toolbar-container">
        <div className="form-group">
          <label htmlFor="startDate">Start Date</label>
          <input
            id="startDate"
            type="datetime-local"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
          />
        </div>
        <div className="form-group">
          <label htmlFor="endDate">End Date</label>
          <input
            id="endDate"
            type="datetime-local"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
          />
        </div>
        <div className="form-group">
          <label htmlFor="workType">Work Type</label>
          <input
            id="workType"
            type="text"
            value={workType}
            onChange={(e) => setWorkType(e.target.value)}
          />
        </div>
        <div className="form-group">
          <label htmlFor="status">Status</label>
          <input
            id="status"
            type="text"
            value={status}
            onChange={(e) => setStatus(e.target.value)}
          />
        </div>
        <div className="form-group">
          <button
            type="submit"
            style={{ marginTop: "1.5rem" }}
            onClick={(e) => {
              e.preventDefault();
              handleFilter();
            }}
          >
            Filter
          </button>
        </div>
      </div>

      {!isLoading ? (
        <ListWorkOrders workOrders={workOrders} />
      ) : (
        <div>Loading...</div>
      )}
    </section>
  );
};

export default WorkOrders;
