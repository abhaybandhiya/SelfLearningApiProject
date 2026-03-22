import { useState } from "react";
import { postProduct } from "../../api/productApi";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function AddProductForm({ onProductAdded }) {
  const [name, setName] = useState("");
  const [price, setPrice] = useState("");
  const [category, setCategory] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault(); // page reload rokta hai (VERY IMPORTANT)

    try {
      setLoading(true);
      setError(null);

      await postProduct({name,price: Number(price),category});

      setName("");
      setPrice("");
      setCategory("");
      onProductAdded(); // parent ko bolo refresh karne ke liye
        setTimeout(() => {
                toast.success("Product added successfully 🎉");
        }, 1000);
    } catch (err) {
      setError("Failed to add product");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
    <form onSubmit={handleSubmit}>
      <h3>Add Product</h3>

      {error && <p style={{ color: "red" }}>{error}</p>}

      <div>
        <input
          placeholder="Product name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </div>

      <div>
        <input
          placeholder="Price"
          type="number"
          value={price}
          onChange={(e) => setPrice(e.target.value)}
        />
      </div>

      <button disabled={loading} onClick={() => toast.success("Manual toast")}>
        {loading ? "Saving..." : "Add"}
      </button>
            
    </form>
      <ToastContainer position="top-right" autoClose={3000} />

    </>
  );
}

export default AddProductForm;
