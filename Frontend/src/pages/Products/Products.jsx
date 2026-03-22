import React, { useState } from "react";
import ProductsTable from "./ProductsTable";
import { useProducts } from "./useProducts";
import AddProductForm from "./AddProductForm";
// const [editingProduct, setEditingProduct] = useState();
// console.log("Editing product:", editingProduct);
function Products() {
  const { products, loading, error, reload } = useProducts();

  if (loading) return <h3>Loading products...</h3>;
  if (error) return <h3 style={{ color: "red" }}>{error}</h3>;
  // const handleEdit = (products) => {
  // setEditingProduct(products);
  // };
  return (
    <div>
      <h2>Products</h2>
       <AddProductForm onProductAdded={reload} />
       <ProductsTable productsPROP={products} />
       <ProductsTable productsPROP={products} /> 
    </div>
  );
}

export default Products;
