using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon"); //this defined in Program.cs
            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (jsonResponse.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(jsonResponse.Result));
            }

            return new CouponDto();
        }
    }
}
