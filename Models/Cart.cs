using System.ComponentModel.DataAnnotations;

namespace Shop_Mvc.Models
{
    public class Cart
    {
        [Key]
        public int id { get; set; }
        public string user_Id { get; set; }
        public string cartName { get; set; }
    }
}
