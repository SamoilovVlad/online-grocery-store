using Shop_Mvc.Controllers;

namespace Shop_Mvc.Models
{
    public class IndexViewModel
    {
        public IEnumerable<SliderImage> sliderImages { get; set; }
        public IEnumerable<Product> promoProducts { get; set; }
        public IEnumerable<Product> domesticManufacturerProducts { get; set; }
        public IEnumerable<Product> ownBrandProducts { get; set; }
        public IEnumerable<Product> breadProducts { get; set; }
        public IEnumerable<Product> meatProducts { get; set; }
        public IEnumerable<Product> cheeseProducts { get; set; }
        public IEnumerable<Product> milkProducts { get; set; }
        public IEnumerable<Product> gobletProducts { get; set; }
        public IEnumerable<Product> coffeeProducts { get; set; }
        public IEnumerable<Product> teaProducts { get; set; }
        public IEnumerable<Product> vegetableProducts { get; set; }
        public IEnumerable<Product> fruitProducts { get; set; }
        public IEnumerable<CartProduct> cartProducts { get; set; }
    public IEnumerable<Product> sweetProducts { get; set; }

        public MainPartialViewModel mainPartialViewModel { get; set; }
    }
}
