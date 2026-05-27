using System.ComponentModel.DataAnnotations;

namespace BlazorEFDemo.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        // Navigation property — one Category has many Products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

}
