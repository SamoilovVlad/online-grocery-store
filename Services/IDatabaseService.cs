using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
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
        public IEnumerable<Product> SearchProducts(string parameter, int count);
        public IEnumerable<Product> SearchProducts(string parameter, int count = 0, int skip = 0, bool isPromo = false);
        public IEnumerable<Product> SearchProductsOrderByPriceDescending(string parameter, int count = 0, int skip = 0, bool isPromo = false);
        public IEnumerable<Product> SearchProductsOrderByPrice(string parameter, int count, int skip, bool isPromo);
        public IEnumerable<Product> SearchProductsPromoFirstly(string parameter, int count, int skip, bool isPromo);
        public User GetUserByEmail(string email);
        public Task<List<CartProduct>> GetCartProductsAsync(string userId);
        public Task SetCartProductsAsync(List<CartProduct> products);
        public CartProduct GetCartProduct(int productId, string UserId);
        public void DeleteUserCartProducts(string UserId);
        public void UpdateCartProduct(int productId, string userId, int counts);
        public void DeleteUserCartProduct(int productId, string userId);
        public void AddUserCartProduct(int productId, string userId, decimal price, string title, int count, string promo, string brand, string country);
        public int GetUserCartProductCount(int productId, string userId);
        public void AddProductToFavorite(int productId, string userId, string title, decimal price, string country, string brand, string promo);
        public void RemoveProductFromFavorite(int productId, string userId);
        public IEnumerable<UserFavoriteProduct> GetUserFavoriteProducts(string userId);
        public void DeleteUserFavoriteProducts(List<int> idToRemove, string userId);
        public void CreateUserCart(string userId, string cartName);
        public IEnumerable<Cart> GetUserCarts(string userId);
        public IEnumerable<UserCartProduct> GetProductsForCart(string userId, int cartId);
        public void DeleteUserCart(int cartId, string userId);
        public Cart GetCartById(int cartId);
        public void UpdateUserCartProduct(int cartId, int productId, int count);
        public void DeleteProfileUserCartProduct(int cartId, int productId);
    }
    public class DatabaseServise : IDatabaseServise
    {
        private readonly MyDbContext _context;
        private readonly DbContextOptions<MyDbContext> _dbContextOptions;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public DatabaseServise(MyDbContext context, DbContextOptions<MyDbContext> dbContextOptions, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _dbContextOptions = dbContextOptions;
            _userManager = userManager;
            _signInManager = signInManager;
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

        public IEnumerable<Product> SearchProducts(string parameter, int count) => _context.Products
                                                                                .Where(p => p.Title.Contains(parameter) ||
                                                                                       p.Brand.Contains(parameter) ||
                                                                                       p.Price.ToString().Contains(parameter)).Take(count);
        public IEnumerable<Product> SearchProducts(string parameter, int count = 0, int skip = 0, bool isPromo = false)
        {
            IEnumerable<Product> products;
            if (count == 0)
            {
                if (isPromo)
                {
                    products = _context.Products.Where(p => (p.Title.Contains(parameter) ||
                                                       p.Brand.Contains(parameter) ||
                                                       p.Price.ToString().Contains(parameter)) && p.Promo != null);
                }
                else
                {
                    products = _context.Products.Where(p => p.Title.Contains(parameter) ||
                                                       p.Brand.Contains(parameter) ||
                                                       p.Price.ToString().Contains(parameter));
                }
            }
            else
            {
                if (isPromo)
                {
                    products = _context.Products.Where(p => (p.Title.Contains(parameter) ||
                                                       p.Brand.Contains(parameter) ||
                                                       p.Price.ToString().Contains(parameter)) && p.Promo != null).Skip(skip).Take(count);
                }
                else
                {
                    products = _context.Products.Where(p => p.Title.Contains(parameter) ||
                                                       p.Brand.Contains(parameter) ||
                                                       p.Price.ToString().Contains(parameter)).Skip(skip).Take(count);
                }
            }
            return products;
        }

        public IEnumerable<Product> SearchProductsOrderByPriceDescending(string parameter, int count = 0, int skip = 0, bool isPromo = false)
        {
            IEnumerable<Product> products;

            if (!isPromo)
            {
                if (count == 0)
                {
                    products = _context.Products
                      .Where(p => p.Title.Contains(parameter) || p.Brand.Contains(parameter) || p.Price.ToString().Contains(parameter))
                      .OrderByDescending(p => p.Price);
                }
                else
                {

                    products = _context.Products
                        .Where(p => p.Title.Contains(parameter) || p.Brand.Contains(parameter) || p.Price.ToString().Contains(parameter))
                        .OrderByDescending(p => p.Price)
                        .Skip(skip)
                        .Take(count);
                }
            }
            else
            {
                if (count == 0)
                {
                    products = _context.Products
                       .Where(p => (p.Title.Contains(parameter) || p.Brand.Contains(parameter) || p.Price.ToString().Contains(parameter)) && p.Promo != null)
                       .OrderByDescending(p => p.Price);
                }
                else
                {

                    products = _context.Products
                        .Where(p => (p.Title.Contains(parameter) || p.Brand.Contains(parameter) || p.Price.ToString().Contains(parameter)) && p.Promo != null)
                        .OrderByDescending(p => p.Price)
                        .Skip(skip)
                        .Take(count);
                }
            }
            return products;
        }

        public IEnumerable<Product> SearchProductsOrderByPrice(string parameter, int count, int skip, bool isPromo)
        {
            IEnumerable<Product> products;

            if (!isPromo)
            {
                if (count == 0)
                {
                    products = _context.Products
                      .Where(p => p.Title.Contains(parameter) || p.Brand.Contains(parameter) || p.Price.ToString().Contains(parameter))
                      .OrderBy(p => p.Price);
                }
                else
                {

                    products = _context.Products
                        .Where(p => p.Title.Contains(parameter) || p.Brand.Contains(parameter) || p.Price.ToString().Contains(parameter))
                        .OrderBy(p => p.Price)
                        .Skip(skip)
                        .Take(count);
                }
            }
            else
            {
                if (count == 0)
                {
                    products = _context.Products
                       .Where(p => (p.Title.Contains(parameter) || p.Brand.Contains(parameter) || p.Price.ToString().Contains(parameter)) && p.Promo != null)
                       .OrderBy(p => p.Price);
                }
                else
                {

                    products = _context.Products
                        .Where(p => (p.Title.Contains(parameter) || p.Brand.Contains(parameter) || p.Price.ToString().Contains(parameter)) && p.Promo != null)
                        .OrderBy(p => p.Price)
                        .Skip(skip)
                        .Take(count);
                }
            }
            return products;
        }

        public IEnumerable<Product> SearchProductsPromoFirstly(string parameter, int count, int skip, bool isPromo)
        {
            IEnumerable<Product> products;

            if (!isPromo)
            {

                if (count == 0)
                    products = _context.Products
                        .Where(p => p.Title.Contains(parameter) ||
                        p.Brand.Contains(parameter) ||
                        p.Price.ToString().Contains(parameter)).OrderByDescending(p => !string.IsNullOrEmpty(p.Promo));
                else
                {
                    products = _context.Products
                       .Where(p => p.Title.Contains(parameter) ||
                        p.Brand.Contains(parameter) ||
                        p.Price.ToString().Contains(parameter)).OrderByDescending(p => !string.IsNullOrEmpty(p.Promo))
                        .Skip(skip)
                        .Take(count);

                }
            }
            else
            {
                if (count == 0)
                    products = _context.Products
                        .Where(p => (p.Title.Contains(parameter) ||
                        p.Brand.Contains(parameter) ||
                        p.Price.ToString().Contains(parameter)) && p.Promo != null).OrderByDescending(p => !string.IsNullOrEmpty(p.Promo));
                else
                {

                    products = _context.Products
                        .Where(p => (p.Title.Contains(parameter) ||
                        p.Brand.Contains(parameter) ||
                        p.Price.ToString().Contains(parameter)) && p.Promo != null).OrderByDescending(p => !string.IsNullOrEmpty(p.Promo))
                        .Skip(skip)
                        .Take(count);
                }
            }
            return products;
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

        public User GetUserByEmail(string email)
        {
           User user = _userManager.Users
                       .Where(u => u.Email == email)
                       .FirstOrDefault();
            return user;
        }

        public async Task<List<CartProduct>> GetCartProductsAsync(string userId)
        {
            var userCartProducts = await _context.UserCartProducts.Where(p => p.user_Id == userId).ToListAsync();
            return userCartProducts;
        }

        public async Task SetCartProductsAsync(List<CartProduct> products)
        {
            _context.UserCartProducts.AddRange(products);
            await _context.SaveChangesAsync();
        }

        public CartProduct GetCartProduct(int productId, string UserId) => _context.UserCartProducts
                                                                           .Where(p => p.product_Id == productId && p.user_Id == UserId)
                                                                           .First();

        public void UpdateCartProduct(int productId, string userId, int count)
        {
            var product = _context.UserCartProducts.Where(p => p.product_Id == productId && p.user_Id == userId).First();
            product.count = count;
            _context.SaveChanges();
        }

        public void DeleteUserCartProducts(string UserId)
        {
            var products = _context.UserCartProducts.Where(p => p.user_Id == UserId);
            _context.UserCartProducts.RemoveRange(products);
            _context.SaveChanges();
            return;
        }
        public void DeleteUserCartProduct(int productId, string userId)
        {
            var cartProduct = _context.UserCartProducts.First(p => p.product_Id == productId && p.user_Id == userId);
            if (cartProduct != null)
            {
                _context.UserCartProducts.Remove(cartProduct);
                _context.SaveChanges();
            }
            return;
        }
        public void AddUserCartProduct(int productId, string userId, decimal price, string title, int count, string promo, string brand, string country)
        {
            CartProduct cartProduct = new CartProduct();
            cartProduct.user_Id = userId;
            cartProduct.product_Id = productId;
            cartProduct.price = price;
            cartProduct.title = title;
            cartProduct.count = count;
            cartProduct.promo = promo;
            cartProduct.brand = brand;
            cartProduct.country = country;
            _context.UserCartProducts.Add(cartProduct);
            _context.SaveChanges();
            return;
        }
        public int GetUserCartProductCount(int productId, string userId)
        {
            var userCartProduct = _context.UserCartProducts
                                   .FirstOrDefault(p => p.product_Id == productId && p.user_Id == userId);

            var count = userCartProduct != null ? userCartProduct.count : 0;
            return count;
        }
        public void AddProductToFavorite(int productId, string userId, string title, decimal price, string country, string brand, string promo)
        {
            var favoriteProduct = new UserFavoriteProduct()
            {
                product_Id = productId,
                user_Id = userId,
                title = title,
                price = price,
                country = country,
                brand = brand,
                promo = promo
            };
            _context.UserFavoriteProducts.Add(favoriteProduct);
            _context.SaveChanges();
        }
        public void RemoveProductFromFavorite(int productId, string userId)
        {
            var favoriteProductToRemove = _context
                                          .UserFavoriteProducts
                                          .FirstOrDefault(p => p.product_Id == productId && p.user_Id == userId);

            _context.UserFavoriteProducts.Remove(favoriteProductToRemove);
            _context.SaveChanges();
        }
        public IEnumerable<UserFavoriteProduct> GetUserFavoriteProducts(string userId)
        {
            var products = _context.UserFavoriteProducts.Where(p => p.user_Id == userId);
            return products;
        }
        public void DeleteUserFavoriteProducts(List<int> idToRemove, string userId)
        {
            var productsToRemove = _context.UserFavoriteProducts.Where(p => idToRemove.Contains(p.product_Id) && p.user_Id == userId).ToList();
            _context.UserFavoriteProducts.RemoveRange(productsToRemove);
            _context.SaveChanges();
        }
        public void CreateUserCart(string userId, string cartName)
        {
            int maxCartId = 0;
            try
            {
                maxCartId = _context.Carts.Max(c => c.id);
            }
            catch (Exception ex) { }
            maxCartId++;
            var cart = new Cart()
            {
                id = maxCartId,
                cartName = cartName,
                user_Id = userId,
            };
            List<UserCartProduct> userCartProducts = new List<UserCartProduct>();
            var cartProducts = _context.UserCartProducts.Where(p => p.user_Id == userId);
            foreach(var product in cartProducts)
            {
                var p = new UserCartProduct()
                {
                    cart_Id = maxCartId,
                    product_Id = product.product_Id,
                    user_Id = userId,
                    count = product.count,
                    promo = product.promo,
                    title = product.title,
                    price = product.price,
                    country = product.country,
                    brand = product.brand
                };
                userCartProducts.Add(p);
            }
            _context.Carts.Add(cart);
            _context.CartProducts.AddRange(userCartProducts);
            _context.SaveChanges();
        }
        public IEnumerable<Cart> GetUserCarts(string userId) => _context
                                                                .Carts
                                                                .Where(c => c.user_Id == userId);
        public IEnumerable<UserCartProduct> GetProductsForCart(string userId, int cartId) => _context
                                                                                             .CartProducts
                                                                                             .Where(p => p.cart_Id == cartId && p.user_Id == userId);
        public void DeleteUserCart(int cartId, string userId)
        {
            Cart cartToDelete = _context.Carts.First(c => c.id == cartId && c.user_Id == userId);
            var productsToDelete = _context.CartProducts.Where(p => p.cart_Id == cartId && p.user_Id == userId);
            _context.CartProducts.RemoveRange(productsToDelete);
            _context.Carts.Remove(cartToDelete);
            _context.SaveChanges();
        }
        public Cart GetCartById(int cartId) => _context.Carts.FirstOrDefault(c => c.id == cartId);
        public void UpdateUserCartProduct(int cartId, int productId, int count)
        {
            var product = _context.CartProducts.FirstOrDefault(p => p.cart_Id == cartId && p.product_Id == productId);
            product.count = count;
            _context.SaveChanges();
        }
        public void DeleteProfileUserCartProduct(int cartId, int productId)
        {
            var product = _context.CartProducts.FirstOrDefault(p => p.cart_Id == cartId && p.product_Id == productId);
            _context.CartProducts.Remove(product);
            _context.SaveChanges();
        }
    }
}
