using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ShoppingCartController : BaseApiController
    {
        private readonly ICartService cartService;
        public ShoppingCartController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
        {
            var cart = await cartService.GetCartAsync(id);

            return Ok(cart ?? new ShoppingCart {Id = id});
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var updatedCart = await cartService.SetCartAsync(cart);
            
            if(updatedCart == null)
            {
                return BadRequest("Problem with cart");
            }
            return updatedCart;
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<ActionResult> DeleteCart(string id)
        {
            var result = await cartService.DeleteCartAsync(id);

            if (!result) return BadRequest("Problem Deleting Cart");

            return Ok();
        }
    }
}
