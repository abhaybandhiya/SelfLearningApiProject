import { useNavigate } from "react-router-dom";
import { clearToken, getToken } from "../utils/tokenService";

function Navbar() {
  const navigate = useNavigate();
  const token = getToken();

  // agar token nahi hai → navbar hi mat dikhao
  if (!token) return null;

  const handleLogout = () => {
    if (!window.confirm("Are you sure you want to logout?")) {
      return;
    }
    clearToken();          // token delete
    navigate("/login");    // login page
  };

  return (
    <div style={{
      display: "flex",
      justifyContent: "space-between",
      padding: "10px 20px",
      background: "#222",
      color: "#fff"
    }}>
      <h3>Product Management App</h3>
      <button onClick={handleLogout}>
        Logout
      </button>
    </div>
  );
}

export default Navbar;
