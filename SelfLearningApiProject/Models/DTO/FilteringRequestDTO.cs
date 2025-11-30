namespace SelfLearningApiProject.Models.DTO
{
    public class FilteringRequestDTO
    {
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string? NameStartsWith { get; set; }
        public string? Category { get; set; } // abhi optional, future ready
    }
}
