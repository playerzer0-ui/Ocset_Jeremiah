namespace Jeremiah_SupermarketOnline.Models
{
    public class Filter
    {
        public List<Product> allProducts { get; set; }
        public List<Product> filteredProducts { get; set; }

        public Filter(List<Product> allProducts, List<Product> filteredProducts)
        {
            this.allProducts = allProducts;
            this.filteredProducts = filteredProducts;
        }
        public Filter()
        {

        }
    }
}
