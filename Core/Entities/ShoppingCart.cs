using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class ShoppingCart
    {
        [Required]
        public string Id { get; set; }
        public List<CartItem> Items { get; set; }
        public int DeliveryMethodId { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
