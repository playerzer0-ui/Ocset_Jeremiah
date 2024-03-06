using System.ComponentModel.DataAnnotations;

namespace Jeremiah_SupermarketOnline.Models
{
    public class OrderEditViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please select a customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please select a product")]
        public int ProductId { get; set; }
    }
}
