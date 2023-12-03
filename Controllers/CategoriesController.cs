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
        public IActionResult ProductsView() 
        {
            return View(); 
        }
    }
}
