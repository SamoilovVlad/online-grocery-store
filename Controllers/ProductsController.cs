using Microsoft.AspNetCore.Mvc;
using Shop_Mvc.Models;
using Shop_Mvc.Data;
using Shop_Mvc.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop_Mvc.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDatabaseServise _DatabaseServise;
        public ProductsController(IDatabaseServise DatabaseServise)
        {
            _DatabaseServise = DatabaseServise;
        }
        // GET: api/<ProductsController>
        //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _DatabaseServise.GetAllProducts();
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _productService.GetProductById(id);
        }
        [HttpGet("category")]
        public List<Product> GetProductsByCategory([FromQuery] string category,[FromQuery] int n) { 
            return _productService.GetProductsByCategory(category, n);
        }
        [HttpGet("subcategory")]
        public List<Product> GetProductsBySubcategory([FromQuery] string subcategory, [FromQuery] int n)
        {
            return _productService.GetProductsBySubcategory(subcategory, n);
        }
        //[HttpGet("category/{category}")]
        //public List<Product> GetProductsByCategory(string category) { 
        //    return _DatabaseServise.GetProductsByCategory(category);
        //}
    }
}
