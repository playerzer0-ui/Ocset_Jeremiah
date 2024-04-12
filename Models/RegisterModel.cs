using System.ComponentModel.DataAnnotations;

namespace Jeremiah_SupermarketOnline.Models
{
    public class RegisterModel
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string Address { get; set; }
    }
}
