namespace Shop_Mvc.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Product> Product_list { get; set; }
        public ProductViewModel(Product product, IEnumerable<Product> product_list) { 
            this.Product = product;
            this.Product_list = product_list;
        }
    }
}
