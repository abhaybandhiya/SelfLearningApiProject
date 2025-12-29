import ProductsTable from "./ProductsTable";
import { useProducts } from "./useProducts";

function Products() {
  const { products, loading, error } = useProducts();

  if (loading) return <h3>Loading products...</h3>;
  if (error) return <h3 style={{ color: "red" }}>{error}</h3>;

  return (
    <div>
      <h2>Products</h2>
      <ProductsTable productsPROP={products} />
    </div>
  );
}

export default Products;
