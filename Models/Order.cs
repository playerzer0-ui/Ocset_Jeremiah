using System.ComponentModel.DataAnnotations;

namespace Jeremiah_SupermarketOnline.Models
{
    public class Order
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        //navigation property
        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
