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
            var products = _DatabaseServise.GetProductsBySubcategory(subcategoryName, pageSize, (currentPage - 1) * pageSize);
            var productsCount = _DatabaseServise.GetProductsBySubcategory(subcategoryName).Count();

            var model = new ProductsViewModel()
            {
                products = products,
                productsCount = productsCount,
                pageNumber = (int)Math.Ceiling((double) productsCount / pageSize),
                pageSize = pageSize,
                currentPage = currentPage,
                onlySales = isOnlySales,
                sortedBy = sortedBy,
            };
            return View(model); 
        }
    }
}
