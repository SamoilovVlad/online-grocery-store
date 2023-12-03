using System.ComponentModel.DataAnnotations;

namespace Shop_Mvc.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
