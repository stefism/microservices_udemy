using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                var result = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(result);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Product product = _db.Products.First(p => p.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Post(ProductDto productDto) //Нямаме [FromBody], защото може да има файл, а и при пост май всичко се пращаше в бодито.
        {
            try
            {
                Product currentProduct = _mapper.Map<Product>(productDto);
                _db.Products.Add(currentProduct);
                _db.SaveChanges();

                if(productDto.Image != null)
                {
                    string fileName = currentProduct.ProductId + Path.GetExtension(productDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    string filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using(var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }

                    string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    currentProduct.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    currentProduct.ImageLocalPath = filePath;
                } 
                else
                {
                    currentProduct.ImageUrl = "hppts://placehold.co/600x400";
                }

                _db.Products.Update(currentProduct);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDto>(currentProduct);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put(ProductDto productDto)
        {
            try
            {
                Product currentProduct = _mapper.Map<Product>(productDto);

                if (productDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(currentProduct.ImageLocalPath))
                    {
                        string currentProductFilePath = Path.Combine(Directory.GetCurrentDirectory(), currentProduct.ImageLocalPath);
                        FileInfo file = new FileInfo(currentProductFilePath);

                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    string fileName = currentProduct.ProductId + Path.GetExtension(productDto.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    string filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }

                    string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    currentProduct.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    currentProduct.ImageLocalPath = filePath;
                }

                _db.Products.Update(currentProduct);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProductDto>(currentProduct);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Product product = _db.Products.First(p => p.ProductId == id);
                if(!string.IsNullOrEmpty(product.ImageLocalPath))
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                    FileInfo file = new FileInfo(filePath);

                    if(file.Exists)
                    {
                        file.Delete();
                    }
                }

                _db.Products.Remove(_db.Products.First(c => c.ProductId == id));
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
