using Mango.Web.Models;
using Mango.Web.Service.IService;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.POST,
                Url = Helpers.ProductAPIBase + "/api/product",
                Data = productDto
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.DELETE,
                Url = Helpers.ProductAPIBase + "/api/Product/" + id
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.GET,
                Url = Helpers.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto?> GetProductAsync(string productCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.GET,
                Url = Helpers.ProductAPIBase + "/api/product/GetByCode/" + productCode
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.GET,
                Url = Helpers.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Enums.ApiType.PUT,
                Url = Helpers.ProductAPIBase + "/api/product",
                Data = productDto
            });
        }
    }
}
