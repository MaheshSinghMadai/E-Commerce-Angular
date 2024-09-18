using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private IProductRepository repo;
        public ProductController(IProductRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string brand, string type, string sort)
        {
            return Ok(await repo.GetProductsAsync(brand, type, sort));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await repo.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
           return Ok(await repo.GetProductBrands());
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await repo.GetProductTypes());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            repo.AddProduct(product);
            await repo.SaveChangesAsync();
            
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id)) {
                return BadRequest("Cannot update this product");
            }

            repo.UpdateProduct(product);
            await repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repo.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            repo.DeleteProduct(product);
            await repo.SaveChangesAsync();

            return NoContent();
        }
        private bool ProductExists(int id)
        {
            return repo.ProductExists(id);
        }
    }
}
