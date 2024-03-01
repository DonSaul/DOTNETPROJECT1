import { useState, useEffect } from "react";
import ListTechnicians from "../components/ListTechnicians";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { apiGet } from "../services/api";

const Technicians = () => {
    const [technicians, setTechnicians] = useState([]);
    const [search, setSearch] = useState("");
    const [isLoading, setIsLoading] = useState(true);

    // const success = () => toast("Wow so easy!");

    useEffect(() => {
        const getTechnicians = async () => {
            setIsLoading(true);
            try {
                const response = await apiGet("https://localhost:7178/api/Technician");
                if (response.ok) {
                    // success();
                    const data = await response.json();
                    setTechnicians(data);
                } else {
                    toast.error("Failed to fetch data", {
                        position: "top-right",
                        autoClose: 5000,
                        pauseOnHover: true,
                        theme: "dark",
                    });
                }
            } catch (error) {
                console.error(error);
                toast.error("Error processing data", {
                    position: "top-right",
                    autoClose: 5000,
                    pauseOnHover: true,
                    theme: "dark",
                });
            }
            // console.log(response)

            setIsLoading(false);
        };

        getTechnicians();
    }, []);

    const handleSearch = async () => {
        if (search === "") {
            return;
        }
        const response = await apiGet(
            `https://localhost:7178/api/Technician/TechnicianByName/${search}`
        );
        if (response.ok) {
            toast.success("Filter applied successfully", {
                position: "top-right",
                autoClose: 2500,
                hideProgressBar: false,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: true,
                progress: undefined,
                theme: "dark",
            });
        }
        const data = await response.json();

        setTechnicians(data);
    };

    return (
        <section>
            <ToastContainer />
            <h1 className="my-heading">Technicians</h1>
            <div
                style={{
                    display: "flex",
                    width: "100%",
                    alignItems: "center",
                    margin: "0.5rem 0",
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
                    type="submit"
                    onClick={(e) => {
                        e.preventDefault();
                        handleSearch();
                    }}
                >
                    Search by Name
                </button>
            </div>
            <ListTechnicians technicians={technicians} isLoading={isLoading} />
        </section>
    );
};

export default Technicians;
