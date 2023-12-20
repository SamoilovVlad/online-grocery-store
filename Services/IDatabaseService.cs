using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Shop_Mvc.Data;
using Shop_Mvc.Models;
using System.Diagnostics.Metrics;
using System.Drawing;

namespace Shop_Mvc.Services
{
    public interface IDatabaseServise
    {
        public IEnumerable<Product> GetAllProducts();
        public Product GetProductById(int id);
        public IEnumerable<SliderImage> GetAllSliderImages();
        public IEnumerable<Product> GetProductsByCountry(string country, int count = 0);
        public IEnumerable<Product> GetProductsByCategory(string category, int count = 0);
        public IEnumerable<Product> GetProductsBySubcategory(string subcategory, int count = 0, int skip = 0, bool isPromo = false);
        public IEnumerable<Product> GetProductsBySubcategoryWithPromo(string subcategory, int count = 0, int skip = 0);
        public IEnumerable<Product> GetProductsByBrand(string brand, int count = 0);
        public IEnumerable<Product> GetPromoProducts(int count = 0);
        public IEnumerable<Product> GetProductsByTitle(string title, int count = 0);
        public IEnumerable<Product> GetProductsBySubcategoryOrderByPriceDescending(string subcategory);
        public IEnumerable<Product> GetProductsBySubcategoryOrderByPriceDescending(string subcategory, int count = 0, int skip = 0, bool isPromo = false);
        public IEnumerable<Product> GetProductsBySubcategoryOrderByPrice(string subcategory);
        public IEnumerable<Product> GetProductsBySubcategoryOrderByPrice(string subcategory, int count = 0, int skip = 0, bool isPromo = false);
        public IEnumerable<Product> GetProductsBySubcategoryPromoFirstly(string subcategory, int count = 0, int skip = 0, bool isPromo = false);
        public Task<IEnumerable<Product>> GetProductsByCountryAsync(string country, int count = 0);
        public Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, int count = 0);
        public Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(string subcategory, int count = 0);
        public Task<IEnumerable<Product>> GetProductsByBrandAsync(string brand, int count = 0);
        public Task<IEnumerable<Product>> GetProductsByTitleAsync(string title, int count = 0);
        public Task<IEnumerable<Product>> GetPromoProductsAsync(int count = 0);
        public Task<IEnumerable<SliderImage>> GetAllSliderImagesAsync();
        public void CreateProduct([FromBody] Product product);
        public void UpdateProduct(int id, [FromBody] Product product);
        public void Delete(int id);
        public Category GetProductCategory(string categoryName);
        public IEnumerable<Subcategory> GetAllSubcategoriesByProductCategoryName(string categoryName);
        public Subcategory GetSubcategoryByName(string subcategoryName);
    }
    public class DatabaseServise : IDatabaseServise
    {
        private readonly MyDbContext _context;
        private readonly DbContextOptions<MyDbContext> _dbContextOptions;

        public DatabaseServise(MyDbContext context, DbContextOptions<MyDbContext> dbContextOptions)
        {
            _context = context;
            _dbContextOptions = dbContextOptions;
        }

        public IEnumerable<SliderImage> GetAllSliderImages()
        {
            return _context.SliderImages.AsEnumerable();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            IEnumerable<Product> products = _context.Products.AsEnumerable();
            return products;
        }
        public Product GetProductById(int id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.id == id)!;
            return product;
        }

        public IEnumerable<Product> GetProductsByCategory(string category, int count = 0)
        {
            IEnumerable<Product> products;
            if (count == 0)
                products = _context.Products.Where(p => p.Category == category);
            else
                products = _context.Products.Where(p => p.Category == category).Take(count);
            return products;
        }

        public IEnumerable<Product> GetProductsBySubcategory(string subcategory, int count = 0, int skip = 0, bool isPromo = false)
        {
            IEnumerable<Product> products;
            if (!isPromo)
            {
                if (count == 0)
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory);
                else
                {
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory)
                        .Skip(skip)
                        .Take(count);
                }
            }
            else
            {
                if (count == 0)
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory && p.Promo != null);
                else
                {
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory && p.Promo != null)
                        .Skip(skip)
                        .Take(count);
                }
            }
            return products;
        }

        public IEnumerable<Product> GetProductsByCountry(string country, int count = 0)
        {
            IEnumerable<Product> products;
            if (count == 0)
                products = _context.Products.Where(p => p.Country == country);
            else
                products = _context.Products.Where(p => p.Country == country).Take(count);
            return products;
        }

        public IEnumerable<Product> GetProductsByBrand(string brand, int count = 0)
        {
            IEnumerable<Product> products;
            if (count == 0)
                products = _context.Products.Where(p => p.Brand == brand);
            else
                products = _context.Products.Where(p => p.Brand == brand).Take(count);
            return products;
        }

        public IEnumerable<Product> GetProductsByTitle(string title, int count = 0)
        {
            IEnumerable<Product> products;
            if (count == 0)
                products = _context.Products.Where(p => p.Title.Contains(title));
            else
                products = _context.Products.Where(p => p.Title.Contains(title)).Take(count);
            return products;
        }

        public IEnumerable<Product> GetPromoProducts(int count = 0)
        {
            IEnumerable<Product> products;
            if (count == 0)
                products = _context.Products.Where(p => p.Promo != null);
            else
                products = _context.Products.Where(p => p.Promo != null).Take(count);
            return products;
        }

        public IEnumerable<Product> GetProductsBySubcategoryWithPromo(string subcategory, int count = 0, int skip = 0)
        {
            IEnumerable<Product> products;
            if (count == 0)
                products = _context.Products
                    .Where(p => p.Subcategory == subcategory && p.Promo != null);
            else
            {
                if (skip != 0)
                {
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory && p.Promo != null)
                        .Skip(skip)
                        .Take(count);
                }
                else
                {
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory && p.Promo != null)
                        .Take(count);
                }
            }
            return products;
        }

        public IEnumerable<Product> GetProductsBySubcategoryOrderByPriceDescending(string subcategory) => _context.Products
                                                                                                                .Where(p => p.Subcategory == subcategory).OrderByDescending(p => p.Price).AsEnumerable();


        public IEnumerable<Product> GetProductsBySubcategoryOrderByPriceDescending(string subcategory, int count = 0, int skip = 0, bool isPromo = false)
        {
            IEnumerable<Product> products;

            if (!isPromo)
            {
                if (count == 0)
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory).OrderByDescending(p => p.Price);
                else
                {

                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory).OrderByDescending(p => p.Price)
                        .Skip(skip)
                        .Take(count);
                }
            }
            else
            {
                if (count == 0)
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory && p.Promo != null)
                        .OrderByDescending(p => p.Price);
                else
                {

                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory && p.Promo != null)
                        .OrderByDescending(p => p.Price)
                        .Skip(skip)
                        .Take(count);
                }
            }
            return products;
        }

        public IEnumerable<Product> GetProductsBySubcategoryOrderByPrice(string subcategory) => _context.Products
                                                                                                    .Where(p => p.Subcategory == subcategory).OrderBy(p => p.Price);

        public IEnumerable<Product> GetProductsBySubcategoryOrderByPrice(string subcategory, int count = 0, int skip = 0, bool isPromo = false)
        {
            IEnumerable<Product> products;

            if (!isPromo)
            {

                if (count == 0)
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory).OrderBy(p => p.Price);
                else
                {
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory).OrderBy(p => p.Price)
                        .Skip(skip)
                        .Take(count);

                }
            }
            else
            {
                if (count == 0)
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory && p.Promo != null).OrderBy(p => p.Price);
                else
                {

                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory && p.Promo != null).OrderBy(p => p.Price)
                        .Skip(skip)
                        .Take(count);
                }
            }
            return products;
        }

        public IEnumerable<Product> GetProductsBySubcategoryPromoFirstly(string subcategory, int count = 0, int skip = 0, bool isPromo = false)
        {
            IEnumerable<Product> products;

            if (!isPromo)
            {

                if (count == 0)
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory).OrderByDescending(p => !string.IsNullOrEmpty(p.Promo));
                else
                {
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory).OrderByDescending(p => !string.IsNullOrEmpty(p.Promo))
                        .Skip(skip)
                        .Take(count);

                }
            }
            else
            {
                if (count == 0)
                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory && p.Promo != null).OrderByDescending(p => !string.IsNullOrEmpty(p.Promo));
                else
                {

                    products = _context.Products
                        .Where(p => p.Subcategory == subcategory && p.Promo != null).OrderByDescending(p => !string.IsNullOrEmpty(p.Promo))
                        .Skip(skip)
                        .Take(count);
                }
            }
            return products;
        }
       
        public async Task<IEnumerable<Product>> GetProductsByCountryAsync(string country, int count = 0)
        {
            using (var _context = new MyDbContext(_dbContextOptions))
            {
                IQueryable<Product> products = _context.Products.Where(p => p.Country == country);

                if (count > 0)
                {
                    products = products.Take(count);
                }

                return await products.ToListAsync();
            }
        }

        public Subcategory GetSubcategoryByName(string subcategoryName) => _context.Subcategories
            .Where(s => s.SubcategoryName == subcategoryName)
            .First();


        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category, int count = 0)
        {
            using (var _context = new MyDbContext(_dbContextOptions))
            {
                IQueryable<Product> products = _context.Products.Where(p => p.Category == category);
                if (count > 0)
                    products = products.Take(count);
                return await products.ToListAsync();
            }
        }
        public async Task<IEnumerable<Product>> GetProductsBySubcategoryAsync(string subcategory, int count = 0)
        {
            using (var _context = new MyDbContext(_dbContextOptions))
            {
                IQueryable<Product> products = _context.Products.Where(p => p.Subcategory == subcategory);
                if (count > 0)
                    products = products.Take(count);
                return await products.ToListAsync();
            }
        }
        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string brand, int count = 0)
        {
            using (var _context = new MyDbContext(_dbContextOptions))
            {
                IQueryable<Product> products = _context.Products.Where(p => p.Brand == brand);
                if (count > 0)
                    products = products.Take(count);
                return await products.ToListAsync();
            }
        }
        public async Task<IEnumerable<Product>> GetProductsByTitleAsync(string title, int count = 0)
        {
            using (var _context = new MyDbContext(_dbContextOptions))
            {
                IQueryable<Product> products = _context.Products.Where(p => p.Title.Contains(title));
                if (count > 0)
                    products = products.Take(count);
                return await products.ToListAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetPromoProductsAsync(int count = 0)
        {
            using (var _context = new MyDbContext(_dbContextOptions))
            {
                IQueryable<Product> products = _context.Products.Where(p => p.Promo != null);
                if (count > 0)
                    products = products.Take(count);
                return await products.ToListAsync();
            }
        }

        public async Task<IEnumerable<SliderImage>> GetAllSliderImagesAsync()
        {
            using (var _context = new MyDbContext(_dbContextOptions))
            {
                IQueryable<SliderImage> sliderImages = _context.SliderImages;

                return await sliderImages.ToListAsync();
            }
        }

        public void CreateProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void UpdateProduct(int id, [FromBody] Product product)
        {

            Product _product = _context.Products.FirstOrDefault(p => p.id == id)!;
            if (_product != null)
            {
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

        public Category GetProductCategory(string categoryName) => _context.Categories.Where(c => c.CategoryName == categoryName).First();

        public IEnumerable<Subcategory> GetAllSubcategoriesByProductCategoryName(string categoryName)
        {
            var category = GetProductCategory(categoryName);
            var subcategories = _context.Subcategories.Where(s => s.CategoryId == category.CategoryId);
            var a = subcategories.Count();
            return subcategories;
        }
    }
}
