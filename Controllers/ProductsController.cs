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
            return _DatabaseServise.GetProductById(id);
        }
        //[HttpGet("category/{category}")]
        //public List<Product> GetProductsByCategory(string category) { 
        //    return _DatabaseServise.GetProductsByCategory(category);
        //}
    }
}
