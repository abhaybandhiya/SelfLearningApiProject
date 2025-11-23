namespace SelfLearningApiProject.Entities
{
    // Yeh class Product entity ko represent karti hai, jo database me store hoti hai
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }         
        public string Description { get; set; } = string.Empty;
        public int ProductQty { get; set; }
        public string Category { get; set; } = string.Empty;
      
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
   
}
