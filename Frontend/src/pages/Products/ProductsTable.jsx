function ProductsTable({ productsPROP }) {
  if (productsPROP.length === 0) {
    return <p>No products found</p>;
  }

  return (
    <table border="1" cellPadding="8">
      <thead>
        <tr>
          <th>Name</th>
          <th>Price</th>
        </tr>
      </thead>
      <tbody>
        {productsPROP.map(p => (
          <tr key={p.id}>
            <td>{p.name}</td>
            <td>₹{p.price}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

export default ProductsTable;
