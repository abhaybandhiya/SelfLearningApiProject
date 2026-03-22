import api from "./axios";

// ye getAllProducts function sabhi products ko fetch karega
export const getAllProducts = async () => {
  const response = await api.get("/product");
  
  return response.data.data;  
};

// ye postProduct function naya product create karega
export const postProduct = async (product) => {
  const response = await api.post("/product", product);
  return response.data.data;
}

// ye updateProduct function existing product ko update karega
export const updateProduct = async (id, product) => {
  const response = await api.put(`/product/${id}`, product);
  console.log("Update response:", response);
  return response.data.data;
}