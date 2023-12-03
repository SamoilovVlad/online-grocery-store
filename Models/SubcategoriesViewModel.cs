namespace Shop_Mvc.Models
{
    public class SubcategoriesViewModel
    {
        public Category Category { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set;}
        public IEnumerable<Product> promoProducts { get; set;}
    }
}
