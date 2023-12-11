using Microsoft.AspNetCore.Mvc;
using Shop_Mvc.Models;
using System.Diagnostics;
using Shop_Mvc.Data;
using Shop_Mvc.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.IO;

namespace Shop_Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseServise _DatabaseServise;
        private readonly IMemoryCache _memoryCache;
        public HomeController(ILogger<HomeController> logger, IDatabaseServise DatabaseServise, IMemoryCache memoryCache)
        {
            _logger = logger;
            _DatabaseServise = DatabaseServise;
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            var model = _memoryCache.Get("IndexViewModel_key") as IndexViewModel;

            if (model == null)
            {
                model = new IndexViewModel();

                var task = Task.Run(() => _DatabaseServise.GetAllSliderImagesAsync());

                var tasks = new List<Task<IEnumerable<Product>>>
        {
            _DatabaseServise.GetProductsByBrandAsync("Novus", 16),
            _DatabaseServise.GetProductsByCountryAsync("Україна", 16),
            _DatabaseServise.GetProductsBySubcategoryAsync("Хліб", 16),
            _DatabaseServise.GetProductsByTitleAsync("М`ясо", 16),
            _DatabaseServise.GetProductsBySubcategoryAsync("Сири", 16),
            _DatabaseServise.GetProductsBySubcategoryAsync("Молоко", 16),
            _DatabaseServise.GetProductsBySubcategoryAsync("Консервація", 16),
            _DatabaseServise.GetProductsBySubcategoryAsync("Кава", 16),
            _DatabaseServise.GetProductsBySubcategoryAsync("Чай", 16),
            _DatabaseServise.GetProductsBySubcategoryAsync("Овочі", 16),
            _DatabaseServise.GetProductsBySubcategoryAsync("Фрукти, ягоди", 16),
            _DatabaseServise.GetProductsBySubcategoryAsync("Цукерки", 16),
            _DatabaseServise.GetPromoProductsAsync(16),
        };

                await Task.WhenAll(tasks);
                await Task.WhenAll(task);

                var ownBrandProducts = tasks[0].Result;
                var domesticManufacturerProducts = tasks[1].Result;
                var breadProducts = tasks[2].Result;
                var meatProducts = tasks[3].Result;
                var cheeseProducts = tasks[4].Result;
                var milkProducts = tasks[5].Result;
                var gobletProducts = tasks[6].Result;
                var coffeeProducts = tasks[7].Result;
                var teaProducts = tasks[8].Result;
                var vegetableProducts = tasks[9].Result;
                var fruitProducts = tasks[10].Result;
                var sweetProducts = tasks[11].Result;
                var promoProducts = tasks[12].Result;

                var sliderImages = task.Result;

                var mainPartialViewModel = new MainPartialViewModel { sliderImages = sliderImages, products = promoProducts };
                _memoryCache.Set("MainPartialViewModel_key", mainPartialViewModel, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("SliderImages_key", sliderImages, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("PromoProducts_key", promoProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("DomesticManufacturerProducts_key", domesticManufacturerProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("OwnBrandProducts_key", ownBrandProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("BreadProducts_key", breadProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("MeatProducts_key", meatProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("CheeseProducts_key", cheeseProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("MilkProducts_key", milkProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("GobletProducts_key", gobletProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("CoffeeProducts_key", coffeeProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("TeaProducts_key", teaProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("VegetableProducts_key", vegetableProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("FruitProducts_key", fruitProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                _memoryCache.Set("SweetProducts_key", sweetProducts, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
                model.promoProducts = promoProducts;
                model.sliderImages = sliderImages;
                model.mainPartialViewModel = mainPartialViewModel;
                model.domesticManufacturerProducts = domesticManufacturerProducts;
                model.ownBrandProducts = ownBrandProducts;
                model.breadProducts = breadProducts;
                model.meatProducts = meatProducts;
                model.cheeseProducts = cheeseProducts;
                model.milkProducts = milkProducts;
                model.gobletProducts = gobletProducts;
                model.coffeeProducts = coffeeProducts;
                model.teaProducts = teaProducts;
                model.vegetableProducts = vegetableProducts;
                model.fruitProducts = fruitProducts;
                model.sweetProducts = sweetProducts;

                _memoryCache.Set("IndexViewModel_key", model, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });
            }
            stopwatch.Stop();
            var executionTime = stopwatch.Elapsed;
            _logger.LogInformation($"IndexAsync execution time: {executionTime}");
            return View("Index", model);
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

        public IActionResult GetMainPartialView()
        {
            var mainPartialViewModel = _memoryCache.Get("MainPartialViewModel_key") as MainPartialViewModel;
            return PartialView("MainPartialView", mainPartialViewModel);
        }

        public IActionResult GetStartProductPartialView()
        {
            return PartialView("StartProductPartialView", _memoryCache.Get("PromoProducts_key") as List<Product>);
        }

        public IActionResult GetSecondPartialView()
        {
            return PartialView("SecondPartialView");
        }
        public IActionResult GetThirdPartialView()
        {
            return PartialView("ThirdPartialView");
        }

        public IActionResult GetFourthPartialView()
        {
            return PartialView("FourthPartialView");
        }
        public IActionResult GetProductPartialView(string name)
        {
            var products = new List<Product>();
            switch (name)
            {
                case "Акційні товари":
                    products = _memoryCache.Get("PromoProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Власні торгові марки":
                    products = _memoryCache.Get("OwnBrandProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Вітчизняний виробник":
                    products = _memoryCache.Get("DomesticManufacturerProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Хліб":
                    products = _memoryCache.Get("BreadProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "М'ясо":
                    products = _memoryCache.Get("MeatProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Сири":
                    products = _memoryCache.Get("CheeseProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Молочні продукти":
                    products = _memoryCache.Get("MilkProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Бокалія":
                    products = _memoryCache.Get("GobletProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Кава":
                    products = _memoryCache.Get("CoffeeProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Чай":
                    products = _memoryCache.Get("TeaProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Овочі":
                    products = _memoryCache.Get("VegetableProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Фрукти":
                    products = _memoryCache.Get("FruitProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                case "Цукурки":
                    products = _memoryCache.Get("SweetProducts_key") as List<Product>;
                    return PartialView("StartProductPartialView", products);
                default: return PartialView("StartProductPartialView", products);

            }
        }

        [HttpGet]
        public IActionResult ProductView(int id)
        {
            Product product = _DatabaseServise.GetProductById(id);
            IEnumerable<Product> product_list = _DatabaseServise.GetProductsBySubcategory(product.Subcategory, 4);
            ProductViewModel productViewModel = new ProductViewModel(product, product_list);
            return View("ProductView", productViewModel);
        }

    }
}