using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop_Mvc.Models
{
    public class UserFavoriteProduct
    {
        [Key]
        public int id { get; set; }
        public int product_Id { get; set; }
        public string user_Id { get; set; }
        public string title { get; set;}
        public decimal price { get; set;}
        public string country { get; set;}
        public string brand { get; set;}
        public string? promo { get; set;}
        [NotMapped]
        public CartProduct inCart { get; set; }
    }
}
