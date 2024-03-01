import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import Technicians from "./pages/Technicians.tsx";
import "./index.css";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import Navbar from "./components/Navbar.tsx";
import WorkOrders from "./pages/WorkOrders.tsx";
import { Login } from "./pages/Login.tsx";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import { ProtectedRoute } from "./components/ProtectedRoute.tsx";

const darkTheme = createTheme({
    palette: {
        mode: "dark",
    },
});

const router = createBrowserRouter([
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/",
    element: <App />
  },
  {
    path: "/technicians",
    element: (
      <ProtectedRoute>
        <Technicians />
      </ProtectedRoute>
    ),
  },
  {
    path: "/workorders",
    element: (
      <ProtectedRoute>
        <WorkOrders />
      </ProtectedRoute>
    ),
  },
]);

ReactDOM.createRoot(document.getElementById("root")!).render(
    <React.StrictMode>
        <ThemeProvider theme={darkTheme}>
            <main className="app-container">
                <Navbar />
                <RouterProvider router={router} />
            </main>
        </ThemeProvider>
    </React.StrictMode>
);
