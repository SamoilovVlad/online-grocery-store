using Microsoft.AspNetCore.Mvc;
using Shop_Mvc.Models;
using System.Diagnostics;
using Shop_Mvc.Data;
using Shop_Mvc.Services;

namespace Shop_Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult GetMainPartialView()
        {
            return PartialView("MainPartialView");
        }
        [HttpGet]
        public IActionResult GetSecondPartialView()
        {
            return PartialView("SecondPartialView");
        }
        [HttpGet]
        public IActionResult GetThirdPartialView()
        {
            return PartialView("ThirdPartialView");
        }
        [HttpGet]
        public IActionResult GetFourthPartialView()
        {
            return PartialView("FourthPartialView");
        }
        [HttpGet]
        public IActionResult GetStartProductPartialView()
        {
            return PartialView("StartProductPartialView");
        }
        [HttpGet]
        public IActionResult GetProductPartialView(string category)
        {
            if (category == null)
            {
                return GetSecondPartialView();
            }
            else return GetStartProductPartialView();
        }
        [HttpGet]
        public IActionResult ProductView(int id)
        {
            Product product = _productService.GetProductById(id);
            List<Product> product_list = _productService.GetProductsBySubcategory(product.Subcategory, 4);
            ProductViewModel productViewModel = new ProductViewModel(product, product_list);
            return View("ProductView", productViewModel);
        }
    }
}