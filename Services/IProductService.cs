using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Shop_Mvc.Data;
using Shop_Mvc.Models;

namespace Shop_Mvc.Services
{
    public interface IProductService
    {
        public List<Product> GetAllProducts();
        public Product GetProductById(int id);
        public List<Product> GetProductsByCategory(string category, int n);
        public List<Product> GetProductsBySubcategory(string subcategory, int n);

        public void CreateProduct([FromBody] Product product);

        public void UpdateProduct(int id, [FromBody] Product product);

        public void Delete(int id);
    }
    public class ProductService : IProductService
    {
        private readonly MyDbContext _context;

        public ProductService(MyDbContext context)
        {
            _context = context;
        }
        public List<Product> GetAllProducts() { 
            List<Product> products = _context.Products.ToList();
            return products;
        }
        public Product GetProductById(int id) {
            Product product = _context.Products.FirstOrDefault(p => p.id == id)!;
            return product;
        }

        public List<Product> GetProductsByCategory(string category, int n) {
            return _context.Products
                .Where(p => p.Category == category)
                .Take(n)
                .ToList();
        }
        public List<Product> GetProductsBySubcategory(string subcategory, int n)
        {
            return _context.Products
                .Where(p => p.Subcategory == subcategory)
                .Take(n)
                .ToList();
        }
        public void CreateProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void UpdateProduct(int id,[FromBody] Product product) { 

            Product _product = _context.Products.FirstOrDefault(p => p.id == id)!;
            if (_product != null) {
                _product.Title = product.Title;
                _product.OnlyInNovus = product.OnlyInNovus;
                _product.Brand = product.Brand;
                _product.Category = product.Category;
                _product.Subcategory = product.Subcategory;
                _product.Promo = product.Promo;
                _product.Country = product.Country;
                _product.Method = product.Method;
                _product.Basis = product.Basis;
                _product.Temperature = product.Temperature;
                _product.Composition = product.Composition;
                _product.Description = product.Description;
                _product.Expiration_date = product.Expiration_date;
                _product.Storage_conditions = product.Storage_conditions;
                _product.Caloric = product.Caloric;
                _product.Carbohydrate = product.Carbohydrate;
                _product.Fat = product.Fat;
                _product.Protein = product.Protein;
                _product.Allergen = product.Allergen;
                _product.Refuel = product.Refuel;
                _product.Quantity_in_package = product.Quantity_in_package;
                _product.Energy_value = product.Energy_value;
                _product.Sort = product.Sort;
                _product.Features = product.Features;
                _product.For_children_with = product.For_children_with;
                _product.Microelements = product.Microelements;
                _product.Vitamins = product.Vitamins;
                _product.Type_of_cheese = product.Type_of_cheese;
                _product.Type_of_sausage = product.Type_of_sausage;
                _product.Method_of_processing = product.Method_of_processing;
                _product.By_composition = product.By_composition;
                _product.Quantity_box_package = product.Quantity_box_package;
                _product.Diaper_size = product.Diaper_size;
                _product.Alcohol = product.Alcohol;
                _product.Temperature_wine_serving = product.Temperature_wine_serving;
                _product.Region = product.Region;
                _product.Wine_classification = product.Wine_classification;
                _product.Aging_in_barrel = product.Aging_in_barrel;
                _product.Package_volume = product.Package_volume;
                _product.Price = product.Price;
                _product.Image = product.Image;

            }
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.id == id)!;
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            _context.SaveChanges();
        }
    }
}
