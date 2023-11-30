using Microsoft.EntityFrameworkCore;
using Shop_Mvc.Models;

namespace Shop_Mvc.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }

    }
}
