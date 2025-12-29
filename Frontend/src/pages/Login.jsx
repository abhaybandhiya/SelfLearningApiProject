import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../api/authApi";

const Login = () => {
  //  Form fields ke liye state
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  //  UI states
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  //  Redirect ke liye
  const navigate = useNavigate();

  //  Login button click handler
  const handleLogin = async (e) => {
    e.preventDefault(); // page reload roko

    setError("");
    setLoading(true);

    try {
      // 🔐 API call
      const data = await login(username, password);
      // 🔐 Token save (important)
      localStorage.setItem("token", data.accessToken);
      // ✅ Login success → products page
      navigate("/products");
    } catch (err) {
      console.error(err);
      setError("Invalid username or password");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ maxWidth: "400px", margin: "100px auto" }}>
      <h2>Login</h2>

      {error && <p style={{ color: "red" }}>{error}</p>}

      <form onSubmit={handleLogin}>
        
        <div style={{ marginBottom: "10px" }}>
          <label>Username</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
            style={{ width: "100%", padding: "8px" }}
          />
        </div>

        <div style={{ marginBottom: "10px" }}>
          <label>Password</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            style={{ width: "100%", padding: "8px" }}
          />
        </div>

        <button
          type="submit"
          disabled={loading}
          style={{ width: "105%", padding: "10px" }}
        >
          {loading ? "Logging in..." : "Login"}
        </button>
      </form>
    </div>
  );
};

export default Login;
