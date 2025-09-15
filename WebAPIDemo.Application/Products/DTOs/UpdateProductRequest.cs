using System.ComponentModel.DataAnnotations;

namespace WebAPIDemo.Application.Products.DTOs
{
    public class UpdateProductRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than zero.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }
    }
}
