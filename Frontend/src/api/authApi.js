import api from "./axios";

export const login = async (credentials) => {
  console.log("Logging in with credentials:", credentials);
  const response = await api.post("/Auth/login", credentials);
  console.log("Login response:", response);
  return response.data;
};