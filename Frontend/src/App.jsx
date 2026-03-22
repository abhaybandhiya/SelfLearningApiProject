import { Routes, Route, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import Products from "./pages/Products/Products";
import ProtectedRoute from "./routes/ProtectedRoute";
import Navbar from "./components/Navbar";
import { ToastContainer } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';

function App() {
  return (
    <>
    <Navbar />
    <Routes>
      {/* Default route */}
      <Route path="/" element={<Navigate to="/login" />} />
      
      {/* Public route */}
      <Route path="/login" element={<Login />} />
      
      {/* Protected route */}
      <Route
        path="/products/*"
        element={
          <ProtectedRoute>
            <Products />
          </ProtectedRoute>
        }
      />
    </Routes>
          <ToastContainer position="top-right" autoClose={3000} />

  </>);
}

export default App;
