using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ShoppingCartAPI.Models.DTO
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        [Range(0, 1000)]
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
