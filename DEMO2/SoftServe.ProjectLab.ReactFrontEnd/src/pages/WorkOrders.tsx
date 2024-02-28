import { useState, useEffect } from "react";
import ListWorkOrders from "../components/ListWorkOrders";
import { ToastContainer, toast } from "react-toastify";
import { Select, MenuItem, SelectChangeEvent } from "@mui/material";
import "react-toastify/dist/ReactToastify.css";

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

interface Status {
    id: number;
    name: string;
}

interface WorkType {
    id: number;
    name: string;
}

const WorkOrders = () => {
    const [workOrders, setWorkOrders] = useState([]);
    const [isLoading, setIsLoading] = useState(true);

    const [search, setSearch] = useState("");
    const [foundWorkOrder, setFoundWorkOrder] = useState({} as WorkOrder);

    const [timezone, setTimezone] = useState("America/Los_Angeles");

    const [statuses, setStatuses] = useState([]);
    const [workTypes, setWorkTypes] = useState([]);

    //filters
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [workType, setWorkType] = useState("all");
    const [status, setStatus] = useState("all");

    useEffect(() => {
        const getWorkOrders = async () => {
            setIsLoading(true);
            try {
                const workOrders = fetch("https://localhost:7178/api/AllWorkOrders");
                const statuses = fetch(
                    "https://localhost:7178/api/WorkOrderDetails/statuses"
                );
                const workTypes = fetch(
                    "https://localhost:7178/api/WorkOrderDetails/workTypes"
                );

                const responses = await Promise.all([workOrders, statuses, workTypes]);

                responses.forEach((response) => {
                    if (!response.ok) {
                        throw new Error("Network response was not ok");
                    }
                });

                const data = await responses[0].json();
                setWorkOrders(data);
                setStatuses(await responses[1].json());
                setWorkTypes(await responses[2].json());
            } catch (error) {
                console.error(error);
                toast.error("Error processing data", {
                    position: "top-right",
                    theme: "dark",
                });
            }
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

            dialog?.showModal();

            toast.success(`Found work order: ${search}`, { theme: "dark" });
            setFoundWorkOrder(await response.json());
        } else {
            toast.error(`Work order not found: ${search}`, { theme: "dark" });
            return;
        }
    };

    const handleFilter = async () => {
        if (startDate === "" || endDate === "") {
            return;
        }

        const initialDate = dayjs
            .tz(startDate, timezone)
            .format("YYYY-MM-DDTHH:mm:ssZ");
        const finalDate = dayjs
            .tz(endDate, timezone)
            .format("YYYY-MM-DDTHH:mm:ssZ");

        const fetchPromise = fetch(
            `https://localhost:7178/api/WorkOrderDetails?startTime=${initialDate}&endTime=${finalDate}&workType=${workType}&status=${status}`,
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
    };

    return (
        <section>
            <ToastContainer />
            <dialog
                id="workOrderDialog"
                style={{ width: 400, borderRadius: 20, borderColor: "#999" }}
            >
                <div className="flex-between">
                    <strong>Work Order Name:</strong>
                    <span>{foundWorkOrder.workOrderName}</span>
                </div>
                <div className="flex-between">
                    <strong>Technician:</strong>
                    <span>{foundWorkOrder.technician}</span>
                </div>
                <div className="flex-between">
                    <strong>Work Type:</strong>
                    <span> {foundWorkOrder.workType}</span>
                </div>
                <div className="flex-between">
                    <strong>Status:</strong>
                    <span> {foundWorkOrder.status}</span>
                </div>
                <div className="flex-between">
                    <strong>Start Time: </strong>
                    <span> {foundWorkOrder.startTime || "N/A"}</span>
                </div>
                <div className="flex-between">
                    <strong>End Time: </strong>
                    <span>{foundWorkOrder.endTime || "N/A"}</span>
                </div>
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
                        style={{ marginLeft: "0.5rem" }}
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
                    <label id="timezone">Timezone</label>
                    <Select
                        sx={{ width: 150 }}
                        title="Timezone
Note: Changing the timezone will affect both the filters and the table."
                        size="small"
                        value={timezone}
                        onChange={(e: SelectChangeEvent) => setTimezone(e.target.value)}
                    >
                        <MenuItem value="America/Los_Angeles">PST/PDT</MenuItem>
                        <MenuItem value="America/Edmonton">MST/MDT</MenuItem>
                        <MenuItem value="America/Chicago">CST/CDT</MenuItem>
                        <MenuItem value="America/New_York">EST/EDT</MenuItem>
                        <MenuItem value="America/Santiago">CL-Santiago</MenuItem>
                    </Select>
                </div>
                <div className="form-group">
                    <label htmlFor="startDate">Start Date</label>
                    <input
                        className="date-picker"
                        id="startDate"
                        type="datetime-local"
                        value={startDate}
                        max={endDate}
                        onChange={(e) => {
                            setStartDate(e.target.value);
                        }}
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="endDate">End Date</label>
                    <input
                        className="date-picker"
                        id="endDate"
                        type="datetime-local"
                        value={endDate}
                        min={startDate}
                        onChange={(e) => {
                            setEndDate(e.target.value);
                        }}
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="workType">Work Type</label>
                    <Select
                        sx={{ width: 200 }}
                        id="workType"
                        size="small"
                        defaultValue="all"
                        onChange={(e: SelectChangeEvent) => setWorkType(e.target.value)}
                    >
                        <MenuItem value="all">All</MenuItem>
                        {workTypes.map((workType: WorkType) => (
                            <MenuItem key={workType.id} value={workType.name}>
                                {workType.name}
                            </MenuItem>
                        ))}
                    </Select>
                </div>
                <div className="form-group">
                    <label htmlFor="status">Status</label>
                    <Select
                        sx={{ width: 200 }}
                        id="status"
                        size="small"
                        defaultValue="all"
                        onChange={(e: SelectChangeEvent) => setStatus(e.target.value)}
                    >
                        <MenuItem value="all">All</MenuItem>
                        {statuses.map((status: Status) => (
                            <MenuItem key={status.id} value={status.name}>
                                {status.name}
                            </MenuItem>
                        ))}
                    </Select>
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

            <ListWorkOrders
                workOrders={workOrders}
                isLoading={isLoading}
                timezone={timezone}
            />
        </section>
    );
};

export default WorkOrders;
