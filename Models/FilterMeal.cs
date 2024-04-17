namespace Jeremiah_SupermarketOnline.Models
{
    public class RootobjectFilter
    {
        public FilterMeal[] meals { get; set; }
    }

    public class FilterMeal
    {
        public string strMeal { get; set; }
        public string strMealThumb { get; set; }
        public string idMeal { get; set; }
    }

}
