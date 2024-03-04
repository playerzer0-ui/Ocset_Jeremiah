using System.ComponentModel.DataAnnotations;

namespace Jeremiah_SupermarketOnline.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Name { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Address { get; set; }

    }
}
