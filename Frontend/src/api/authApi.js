import api from "./axios";

export const login = async (username, password) => {
  const response = await api.post("/Auth/login", {
    username,
    password,
  });
  return response.data;
};