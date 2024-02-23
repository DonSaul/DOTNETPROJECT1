import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import Technicians from "./pages/Technicians.tsx";
import "./index.css";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import Navbar from "./components/Navbar.tsx";
import WorkOrders from "./pages/WorkOrders.tsx";
import { Login } from "./pages/Login.tsx";

const router = createBrowserRouter([
  {
    path: "/login",
    element: <Login/>,
  },
  {
    path: "/",
    element: <App />,
  },
  {
    path: "/technicians",
    element: <Technicians />,
  },
  {
    path: "/workorders",
    element: <WorkOrders />,
  },
]);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <main className="app-container">
      <Navbar />
      <RouterProvider router={router} />
    </main>
  </React.StrictMode>
);
