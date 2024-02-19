namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }

        //Е те това е тука ако не е нулабъл, демек ако не е <CartDetailsDto>? - дава бъг и не влиза в метода ApplyCoupon
        //на CartAPIController-а и не слага и не маха купони. Като е нулабъл и всичко работи чудно.
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
