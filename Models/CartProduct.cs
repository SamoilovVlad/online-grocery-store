using System.ComponentModel.DataAnnotations;

namespace Shop_Mvc.Models
{
    public class CartProduct
    {
        [Key]
        public int PK_id { get; set; }

        public int product_Id {get; set; }        
        public int count {get; set;}
        public decimal price { get; set;}
        public string title { get; set; }
        public string user_Id { get; set; }
        public string? promo { get; set;}
        public string? brand { get; set; }
        public string? country { get; set; }
    }
}
