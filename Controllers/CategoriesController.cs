using Microsoft.AspNetCore.Mvc;
using Shop_Mvc.Models;
using System.Diagnostics;
using Shop_Mvc.Data;
using Shop_Mvc.Services;
using Newtonsoft.Json;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;

namespace Shop_Mvc.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseServise _DatabaseServise;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoriesController(ILogger<HomeController> logger, IDatabaseServise databaseServise, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _DatabaseServise = databaseServise;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult SubcategoriesView(string categoryName)
        {
            var cartProducts = GetProductsFromCookie();
            var promoProducts = SetFieldIsInCart(_DatabaseServise.GetPromoProducts(6), GetProductsFromCookie());

            var model = new SubcategoriesViewModel()
            {
                Category = _DatabaseServise.GetProductCategory(categoryName),
                promoProducts = promoProducts,
                Subcategories = _DatabaseServise.GetAllSubcategoriesByProductCategoryName(categoryName)
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductsView(string subcategoryName, int currentPage = 1, bool isOnlySales = false, string sortedBy = "Стандартно", int pageSize = 40)
        {
            IEnumerable<Product> products;

            products = SortBy(sortedBy, subcategoryName, isOnlySales, 0, 0);
            int count = products.Count();
            products = SetFieldIsInCart(products.Skip((currentPage - 1) * pageSize).Take(pageSize), GetProductsFromCookie());


            var model = new ProductsViewModel()
            {
                products = products,
                productsCount = count,
                pageNumber = (int)Math.Ceiling((double)count / pageSize),
                pageSize = pageSize,
                currentPage = currentPage,
                onlySales = isOnlySales,
                sortedBy = sortedBy,
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult FullProductsSearchView(string searchBy = "", int currentPage = 1, bool isOnlySales = false, string sortedBy = "Стандартно", int pageSize = 40)
        {
            IEnumerable<Product> products;

            products = SortSearchList(sortedBy, searchBy, isOnlySales, 0, 0);
            int count = products.Count();
            products = SetFieldIsInCart(products.Skip((currentPage - 1) * pageSize).Take(pageSize), GetProductsFromCookie());

            var model = new FullProductSearchViewModel()
            {
                products = products,
                productsCount = count,
                pageNumber = (int)Math.Ceiling((double)count / pageSize),
                pageSize = pageSize,
                currentPage = currentPage,
                onlySales = isOnlySales,
                sortedBy = sortedBy,
                searchBy = searchBy,
            };
            return View(model);
        }

        private IEnumerable<Product> SortBy(string sortBy, string subcategory, bool isPromo, int count = 0, int skip = 0)
        {
            switch (sortBy)
            {
                case "Стандартно":
                    return _DatabaseServise.GetProductsBySubcategory(subcategory, count, skip, isPromo);
                case "Спочатку дорожче":
                    return _DatabaseServise.GetProductsBySubcategoryOrderByPriceDescending(subcategory, count, skip, isPromo);
                case "Спочатку дешевше":
                    return _DatabaseServise.GetProductsBySubcategoryOrderByPrice(subcategory, count, skip, isPromo);
                case "Спочатку акційні":
                    return _DatabaseServise.GetProductsBySubcategoryPromoFirstly(subcategory, count, skip, isPromo);
                case "За знижкою":
                    var data = _DatabaseServise.GetProductsBySubcategoryPromoFirstly(subcategory, count, skip, isPromo);
                    data = data
                               .OrderBy(p => p.Promo != null ? ParsePromoValue(p.Promo) : 0);
                    return data;
                default: return _DatabaseServise.GetProductsBySubcategory(subcategory, count, skip, isPromo);
            }
        }

        private IEnumerable<Product> SortSearchList(string sortBy, string searchBy, bool isPromo, int count = 0, int skip = 0)
        {
            switch (sortBy)
            {
                case "Стандартно":
                    return _DatabaseServise.SearchProducts(searchBy, count, skip, isPromo);
                case "Спочатку дорожче":
                    return _DatabaseServise.SearchProductsOrderByPriceDescending(searchBy, count, skip, isPromo);
                case "Спочатку дешевше":
                    return _DatabaseServise.SearchProductsOrderByPrice(searchBy, count, skip, isPromo);
                case "Спочатку акційні":
                    return _DatabaseServise.SearchProductsPromoFirstly(searchBy, count, skip, isPromo);
                case "За знижкою":
                    var data = _DatabaseServise.SearchProductsPromoFirstly(searchBy, count, skip, isPromo);
                    data = data
                               .OrderBy(p => p.Promo != null ? ParsePromoValue(p.Promo) : 0);
                    return data;
                default: return _DatabaseServise.GetProductsBySubcategory(searchBy, count, skip, isPromo);
            }
        }
        private int ParsePromoValue(string promo)
        {
            if (int.TryParse(promo.Trim('%'), out int value))
            {
                return value;
            }
            return 0;
        }

        private IEnumerable<Product> SetFieldIsInCart(IEnumerable<Product> products, List<CartProduct> cartProducts)
        {
            var commonIds = cartProducts.Select(p => p.product_Id).ToList();

            for (var i = 0; i < commonIds.Count; i++)
            {
                var id = commonIds[i];
                foreach (var product in products.Where(p => p.id == id))
                {
                    product.inCart = cartProducts[i];
                }
            }
            return products;
        }

        public List<CartProduct> GetProductsFromCookie()
        {
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["MyCookie"];
            var userString = _httpContextAccessor.HttpContext.Request.Cookies["UserCookie"];
            if (!userString.IsNullOrEmpty())
            {
                var user = JsonConvert.DeserializeObject<User>(userString);
                List<CartProduct> cartProducts = _DatabaseServise.GetCartProductsAsync(user.Id).GetAwaiter().GetResult();
                return cartProducts;
            }
            else
            {
                if (!string.IsNullOrEmpty(cookieValue))
                {
                    var products = JsonConvert.DeserializeObject<List<CartProduct>>(cookieValue);

                    return products;
                }
            }
            return new List<CartProduct>();
        }

    }
}
