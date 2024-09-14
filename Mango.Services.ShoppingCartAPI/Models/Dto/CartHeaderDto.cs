namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponId { get; set; }

        public double Discount { get; set; }
        public double CartTotal { get; set; }
    }
}
