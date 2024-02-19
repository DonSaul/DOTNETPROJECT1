import { useState, useEffect } from "react";
import ListTechnicians from "../components/ListTechnicians";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const Technicians = () => {
  const [technicians, setTechnicians] = useState([]);
  const [search, setSearch] = useState("");
  const [isLoading, setIsLoading] = useState(true);

  // const success = () => toast("Wow so easy!");

  useEffect(() => {
    const getTechnicians = async () => {
      setIsLoading(true);
      const response = await fetch("https://localhost:7178/api/Technician");
      // console.log(response)
      if (response.ok) {
        // success();
      } else {
        return;
      }

      const data = await response.json();
      setTechnicians(data);
      setIsLoading(false);
    };

    getTechnicians();
  }, []);

  const handleSearch = async () => {
    if (search === "") {
      return;
    }
    const response = await fetch(
      `https://localhost:7178/api/Technician/TechnicianByName/${search}`
    );
    if (response.ok) {
      toast.success('Filter applied successfully', {
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
          margin: 10,
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
      {!isLoading ? (
        <ListTechnicians technicians={technicians} />
      ) : (
        <div>Loading...</div>
      )}
    </section>
  );
};

export default Technicians;
