﻿using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentService(
        IConfiguration config,
        ICartService cartService,
        IGenericRepository<Core.Entities.Product> _repo,
        IGenericRepository<DeliveryMethod> dmRepo) : IPaymentService
    {
        public async Task<ShoppingCart> CreateOrUpdatePaymentIntent(string cartId)
        {
            StripeConfiguration.ApiKey = config["StripeConfiguration:SecretKey"];

            var cart = await cartService.GetCartAsync(cartId);
            if (cart == null) return null;

            var shippingPrice = 0m;
            if (cart.DeliveryMethodId > 0)
            {
                var deliveryMethod = await dmRepo.GetByIdAsync(cart.DeliveryMethodId);
                if (deliveryMethod == null) return null;
                shippingPrice = deliveryMethod.Price;
            }

            foreach(var item in cart.Items)
            {
                var productItem = await _repo.GetByIdAsync(item.ProductId);
                if (productItem == null) return null;

                if(item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var paymentService = new PaymentIntentService();
            PaymentIntent intent = null;

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) 
                    + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = ["card"]
                };

                intent = await paymentService.CreateAsync(options);
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
                    + (long)shippingPrice * 100,
                };
                intent = await paymentService.UpdateAsync(cart.PaymentIntentId, options);
            }

            await cartService.SetCartAsync(cart);

            return cart;
        }
    }
}