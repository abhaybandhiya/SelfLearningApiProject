
namespace SelfLearningApiProject.Models
{
    // ProductDto class - yeh class Data Transfer Object (DTO) hai jo client ko product data bhejne ke liye use hoti hai
    public class ProductDto // DTO ka matlab hai Data Transfer Object, jo sirf wahi fields rakhta hai jo client ko chahiye
    {
        // Yeh properties sirf wahi fields rakhti hain jo client ko chahiye, jaise ki Id aur Name
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
