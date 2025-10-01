using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfLearningApiProject.Models.DTO
{
    // ProductDto class - yeh class Data Transfer Object (DTO) hai jo client ko product data bhejne ke liye use hoti hai
    public class ProductDto // DTO ka matlab hai Data Transfer Object, jo sirf wahi fields rakhta hai jo client ko chahiye
    {
        // Yeh properties sirf wahi fields rakhti hain jo client ko chahiye, jaise ki Id aur Name
        public int Id { get; set; } // Product ka unique identifier, yeh database me bhi primary key hota hai // yeh client ko product ki ID dikhane ke liye hai

        [Required(ErrorMessage = "Product name is required.")] // Yeh property product ka naam hai, aur yeh required hai // agar client ne naam nahi diya to error message dikhayega
        [MaxLength(50, ErrorMessage = "Name can't exceed 50 characters.")] // Yeh property product ka naam hai, aur iska maximum length 50 characters hai // agar client ne naam 50 characters se zyada diya to error message dikhayega     
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")] // ⬅️ Price > 0 hona chahiye
        [Required(ErrorMessage = "Product Prince is require")] // Yeh property product ka price hai, aur yeh required hai // agar client ne price nahi diya to error message dikhayega
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}