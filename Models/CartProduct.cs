using System.ComponentModel.DataAnnotations;

namespace Shop_Mvc.Models
{
    public class CartProduct
    {
        [Key]
        public int PK_id { get; set; }

        private int id;
        public int Id
        {
            get => id;
            set { if (value > 0) id = value; }
        }
        private int count;
        public int Count
        {
            set { if (value > 0) count = value; }
            get => count;
        }
        private decimal price;
        public decimal Price
        {
            set { if (value > 0) price = value; }
            get => price;
        }
        public string title { get; set; }
       

        public string user_id { get; set; }
    }
}
