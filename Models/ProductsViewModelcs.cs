namespace Shop_Mvc.Models
{
    public class ProductsViewModel
    {
        public IEnumerable<Product> products { get; set; }
        public int productsCount { get; set; }
        public bool onlySales {get; set; }  
        public int currentPage { get; set;}
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sortedBy { get; set; }
    }
}
