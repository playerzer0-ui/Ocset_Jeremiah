using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jeremiah_SupermarketOnline.Models
{
    public class Product
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Name { get; set; }

        [StringLength(80, MinimumLength = 3)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        //navigation property
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
