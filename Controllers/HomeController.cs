using Microsoft.AspNetCore.Mvc;
using Shop_Mvc.Models;
using System.Diagnostics;
using Shop_Mvc.Data;
using Shop_Mvc.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Shop_Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabaseServise _DatabaseServise;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(ILogger<HomeController> logger, IDatabaseServise DatabaseServise, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _DatabaseServise = DatabaseServise;
            _memoryCache = memoryCache;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var cartProduct = GetProductsFromCookie();

                var model = new IndexViewModel();

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
            // Try to set data to cache memory
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



                model.promoProducts = SetFieldIsInCart(promoProducts,cartProduct);
                model.sliderImages = sliderImages;
                model.mainPartialViewModel = mainPartialViewModel;
                model.domesticManufacturerProducts = SetFieldIsInCart(domesticManufacturerProducts, cartProduct);
                model.ownBrandProducts = SetFieldIsInCart(ownBrandProducts, cartProduct);
                model.breadProducts = SetFieldIsInCart(breadProducts,cartProduct);
                model.meatProducts = SetFieldIsInCart(meatProducts,cartProduct);
                model.cheeseProducts = SetFieldIsInCart(cheeseProducts,cartProduct);
                model.milkProducts = SetFieldIsInCart(milkProducts,cartProduct);
                model.gobletProducts = SetFieldIsInCart(gobletProducts,cartProduct);
                model.coffeeProducts = SetFieldIsInCart(coffeeProducts,cartProduct);
                model.teaProducts = SetFieldIsInCart(teaProducts,cartProduct);
                model.vegetableProducts = SetFieldIsInCart(vegetableProducts,cartProduct);
                model.fruitProducts = SetFieldIsInCart(fruitProducts,cartProduct);
                model.sweetProducts = SetFieldIsInCart(sweetProducts,cartProduct);
                model.cartProducts = cartProduct;

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
            var products = _memoryCache.Get("PromoProducts_key") as IEnumerable<Product>;
            var cartProducts = GetProductsFromCookie();
            products = SetFieldIsInCart(products, cartProducts);
            return PartialView("StartProductsPartialView", products);
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

        //returns partial view of list of products taken from cache to Index.cshtml
        public IActionResult GetProductPartialView(string name)
        {
            IEnumerable<Product> products;
            switch (name)
            {
                case "Акційні товари":
                    {
                        products = _memoryCache.Get("PromoProducts_key") as IEnumerable<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Власні торгові марки":
                    {
                        products = _memoryCache.Get("OwnBrandProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Вітчизняний виробник":
                    {
                        products = _memoryCache.Get("DomesticManufacturerProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Хліб":
                    {
                        products = _memoryCache.Get("BreadProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "М'ясо":
                    {
                        products = _memoryCache.Get("MeatProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Сири":
                    {
                        products = _memoryCache.Get("CheeseProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Молочні продукти":
                    {
                        products = _memoryCache.Get("MilkProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Бакалія":
                    {
                        products = _memoryCache.Get("GobletProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Кава":
                    {
                        products = _memoryCache.Get("CoffeeProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Чай":
                    {
                        products = _memoryCache.Get("TeaProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Овочі":
                    {
                        products = _memoryCache.Get("VegetableProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Фрукти":
                    {
                        products = _memoryCache.Get("FruitProducts_key") as List<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                case "Цукурки":
                    {
                        products = _memoryCache.Get("SweetProducts_key") as IEnumerable<Product>;
                        var cartProducts = GetProductsFromCookie();
                        products = SetFieldIsInCart(products, cartProducts);
                        return PartialView("StartProductPartialView", products);
                    }
                default: return PartialView("StartProductPartialView", new List<Product>());

            }
        }

        public List<CartProduct> GetProductsFromCookie()
        {
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["MyCookie"];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                var products = JsonConvert.DeserializeObject<List<CartProduct>>(cookieValue);

                return products;
            }
            return new List<CartProduct>();
        }

        private IEnumerable<Product> SetFieldIsInCart(IEnumerable<Product> products, List<CartProduct> cartProducts)
        {
            var commonIds = cartProducts.Select(p => p.Id).ToList();

            for (var i = 0; i<commonIds.Count; i++)
            {
                var id = commonIds[i];
                foreach (var product in products.Where(p => p.id == id))
                {
                    product.inCart = cartProducts[i];
                }
            }
            return products;
        }


        [HttpGet]
        public IActionResult ProductSearchPartialView(string param)
        {
            var products = _DatabaseServise.SearchProduct(param, 7);
            products = SetFieldIsInCart(products, GetProductsFromCookie());
            return PartialView(products);
        }



        [HttpGet]
        public IActionResult ProductView(int id)
        {
            Product product = _DatabaseServise.GetProductById(id);
            IEnumerable<Product> product_list = _DatabaseServise.GetProductsBySubcategory(product.Subcategory, 4);
            product_list = SetFieldIsInCart(product_list, GetProductsFromCookie());
            ProductViewModel productViewModel = new ProductViewModel(product, product_list);
            return View("ProductView", productViewModel);
        }

    }
}