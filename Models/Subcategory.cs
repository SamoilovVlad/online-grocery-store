using System.ComponentModel.DataAnnotations;

namespace Shop_Mvc.Models
{
    public class Subcategory
    {
        [Key]
        public int SubcategoryId { get; set; }
        public int CategoryId { get; set; }
        public string SubcategoryName { get; set; }
        //public byte[]? Image { get; set;}
        public int CountOfProducts { get; set; }

    }
}
