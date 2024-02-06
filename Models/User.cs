using Microsoft.AspNetCore.Identity;

namespace Shop_Mvc.Models
{
    public class User : IdentityUser
    {
       public string Name { get; set; } 
       public string? Surname { get; set; }
       public string? SecondName { get; set; }
    }
}
