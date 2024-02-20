using Mango.Web.Models;
using Mango.Web.Service.IService;

namespace Mango.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> ApplyCouponAsync(CartDto cartDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.POST,
                Url = Helpers.ShoppingCartAPIBase + "/api/cart/TestTest",
                Data = cartDto
            });

            return result;
        }

        public async Task<ResponseDto> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.POST,
                Url = Helpers.ShoppingCartAPIBase + "/api/cart/EmailCartRequest",
                Data = cartDto
            });
        }

        public async Task<ResponseDto> GetCartByUserAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.GET,
                Url = Helpers.ShoppingCartAPIBase + "/api/cart/GetCart/" + userId,
            });
        }

        public async Task<ResponseDto> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.POST,
                Url = Helpers.ShoppingCartAPIBase + "/api/cart/RemoveCart",
                Data = cartDetailsId
            });
        }

        public async Task<ResponseDto> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.POST,
                Url = Helpers.ShoppingCartAPIBase + "/api/cart/CartUpsert",
                Data = cartDto
            });
        }
    }
}
