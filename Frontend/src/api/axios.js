import axios from "axios";
import { getToken, clearToken } from "../utils/tokenService";

// ye file axios instance create karegi jisme baseURL aur interceptors set honge
const api = axios.create({
  baseURL: "https://localhost:7179/api",
});

// ✅ REQUEST INTERCEPTOR (token attach)
// har request se pehle token ko header me attach karne ke liye interceptor use karte hain
api.interceptors.request.use(
  (config) => {
    const token = getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    console.log("Request error:", error);
    return Promise.reject(error);
  }
);

// ✅ RESPONSE INTERCEPTOR (auto logout on 401)
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      clearToken();               // localStorage se token hata
      window.location.href = "/login"; // force redirect
    }
    return Promise.reject(error);
  }
);

export default api;
