using Microsoft.EntityFrameworkCore;
using Shop_Mvc.Models;

namespace Shop_Mvc.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<SliderImage> SliderImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

        public List<Product> GetProductBySubcategory(string subcategory)
        {
            return Products
                .Where(p => p.Subcategory == subcategory)
                .ToList();
        }

    }
}
