using System.ComponentModel.DataAnnotations;

namespace Shop_Mvc.Models
{
    public class Subcategory : Info
    {
        [Key]
        public int SubcategoryId { get; set; }
        public int CategoryId { get; set; }
        public string SubcategoryName { get; set; }
        public int CountOfProducts { get; set; }

        public override string GetInfo() => $"Subcategory id: {SubcategoryId}\nCategory id: {CategoryId}\nSubcategory name: {SubcategoryName}\nCount of products: {CountOfProducts}";

    }
}
