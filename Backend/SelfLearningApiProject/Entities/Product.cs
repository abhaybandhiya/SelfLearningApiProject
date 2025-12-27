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

        public bool IsDeleted { get; set; } = false; //soft delete ke liye hai, jisse hum record ko permanently delete na karke usse inactive kar sakte hain
        public DateTime CreatedAt { get; set; } // record creation timestamp ke liye yeh nullable nahi hai kyunki har record ka creation time hona chahiye
        public string? CreatedBy { get; set; } // record creator ke liye yeh nullable hai kyunki har record ke creator ka pata nahi ho sakta

        public DateTime? UpdatedAt { get; set; } // record update timestamp ke liye yeh nullable hai kyunki har record update nahi hota
        public string? UpdatedBy { get; set; } // record updater ke liye yeh nullable hai kyunki har record ke updater ka pata nahi ho sakta
    }
   
}
