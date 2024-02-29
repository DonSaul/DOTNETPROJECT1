import React, { ReactNode, useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import AsyncStorage from "@react-native-async-storage/async-storage";

interface ProtectedRouteProps {
  children: ReactNode;
}

export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean | null>(null);

  useEffect(() => {
    const checkAuth = async () => {
      const token = await AsyncStorage.getItem("token");
      setIsAuthenticated(token !== null);
    };

    checkAuth();
  }, []);

  console.log(isAuthenticated)

  if (isAuthenticated === null) {
    // Esperando la verificación
    return <div>Loading...</div>;
  }

  return isAuthenticated ? <>{children}</> : <Navigate to="/login" replace />;
};


