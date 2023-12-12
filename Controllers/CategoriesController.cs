using Microsoft.AspNetCore.Mvc;
using Shop_Mvc.Models;
using System.Diagnostics;
using Shop_Mvc.Data;
using Shop_Mvc.Services;


namespace Shop_Mvc.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseServise _DatabaseServise;

        public CategoriesController(ILogger<HomeController> logger, IDatabaseServise databaseServise)
        {
            _logger = logger;
            _DatabaseServise = databaseServise;
        }

        [HttpGet]
        public IActionResult SubcategoriesView(string categoryName)
        {
            var model = new SubcategoriesViewModel()
            {
                Category = _DatabaseServise.GetProductCategory(categoryName),
                promoProducts = _DatabaseServise.GetPromoProducts(6),
                Subcategories = _DatabaseServise.GetAllSubcategoriesByProductCategoryName(categoryName)
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult ProductsView(int currentPage, bool isOnlySales, string sortedBy, int pageSize, string subcategoryName)
        {
            IEnumerable<Product> products;

            products = SortBy(sortedBy,subcategoryName, isOnlySales, 0 ,0);
            int count = products.Count();
            products = products.Skip((currentPage-1)*pageSize).Take(pageSize);
            

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

        private IEnumerable<Product>SortBy(string sortBy, string subcategory, bool isPromo, int count = 0, int skip = 0)
        {
            switch (sortBy) 
            {
                case "Стандарт":
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
        private int ParsePromoValue(string promo)
        {
            if (int.TryParse(promo.Trim('%'), out int value))
            {
                return value;
            }
            return 0;
        }

    }
}
