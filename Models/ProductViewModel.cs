namespace Shop_Mvc.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public List<Product> Product_list { get; set; }
        public ProductViewModel(Product product, List<Product> product_list) { 
            this.Product = product;
            this.Product_list = product_list;
        }
    }
}
