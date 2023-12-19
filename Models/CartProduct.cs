namespace Shop_Mvc.Models
{
    public class CartProduct
    {
        public int id { get; set; }
        public int count { get; set; }
        public decimal price { get; set; }
        public string title { get; set; }
    }
}
