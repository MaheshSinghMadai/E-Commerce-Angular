using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Helper;

namespace WebAPI.Controllers
{
    public class ProductController : BaseApiController
    {
        private IGenericRepository<Product> repo;
        public ProductController(IGenericRepository<Product> repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams sp)
        {
            var spec = new ProductSpecification(sp);

            return await CreatePagedResult(repo, spec, sp.PageIndex, sp.PageSize);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await repo.GetByIdAsync(id);
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
           var spec = new BrandListSpecification();
           return Ok(await repo.ListAsync(spec));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            repo.Add(product);
            await repo.SaveAllAsync();
            
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id)) {
                return BadRequest("Cannot update this product");
            }

            repo.Update(product);
            await repo.SaveAllAsync();
            return NoContent();
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            repo.Remove(product);
            await repo.SaveAllAsync();

            return NoContent();
        }
        private bool ProductExists(int id)
        {
            return repo.Exists(id);
        }
    }
}
