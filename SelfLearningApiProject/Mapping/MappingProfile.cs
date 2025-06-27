

using AutoMapper;
using SelfLearningApiProject.Entities;
using SelfLearningApiProject.Models.DTO;

namespace SelfLearningApiProject.Mapping
{
    // AutoMapper ka use karke hum entities aur DTOs ke beech mapping define karte hain //Yeh class AutoMapper ke liye configuration profile hai // Isme hum mapping rules define karte hain, jisse ki entities aur DTOs ke beech conversion asan ho jata hai
    public class MappingProfile : Profile // AutoMapper ka Profile class ko inherit karta hai, jisse mapping configurations define kar sakte hain
    {
        // Yeh constructor mapping rules define karta hai // Isme hum batate hain ki kaunse entity ko kaunse DTO me map karna hai // ReverseMap() se yeh dono tarah se mapping allow hoti hai
        public MappingProfile() // Constructor for MappingProfile class
        {
            // Product entity ko ProductDto me map karte hain aur vice versa // ReverseMap() se yeh dono tarah se mapping allow hoti hai // Iska matlab hai ki agar hum ProductDto se Product me map karna chahte hain to bhi yeh kaam karega
            CreateMap<Product, ProductDto>().ReverseMap();
        }

    }
}
