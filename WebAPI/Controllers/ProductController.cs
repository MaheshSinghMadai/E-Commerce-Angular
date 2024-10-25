using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ProductController(IUnitOfWork unit) : BaseApiController
    {

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams sp)
        {
            var spec = new ProductSpecification(sp);

            return await CreatePagedResult(unit.Repository<Product>(), spec, sp.PageIndex, sp.PageSize);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);
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
           return Ok(await unit.Repository<Product>().ListAsync(spec));
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await unit.Repository<Product>().ListAsync(spec));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            unit.Repository<Product>().Add(product);
            await unit.Complete();
            
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id)) {
                return BadRequest("Cannot update this product");
            }

            unit.Repository<Product>().Update(product);
            await unit.Complete();
            return NoContent();
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await unit.Repository<Product>().GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            unit.Repository<Product>().Remove(product);
            await unit.Complete();

            return NoContent();
        }
        private bool ProductExists(int id)
        {
            return unit.Repository<Product>().Exists(id);
        }
    }
}
