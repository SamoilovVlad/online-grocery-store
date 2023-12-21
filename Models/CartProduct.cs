namespace Shop_Mvc.Models
{
    public class CartProduct
    {
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
        private string title;
        public string Title
        {
            set { if (string.IsNullOrEmpty(value)) title = value; }
            get => title;
        }
    }
}
