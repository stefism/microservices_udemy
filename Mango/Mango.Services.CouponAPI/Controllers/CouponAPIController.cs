using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    //[Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto<List<CouponDto>> Get()
        {
            try
            {
                var result = _mapper.Map<List<CouponDto>>(_db.Coupons.ToList());

                return new ResponseDto<List<CouponDto>>()
                {
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<CouponDto>>()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto<CouponDto> Get(int id)
        {
            try
            {
                var result = _mapper.Map<CouponDto>(_db.Coupons.First(c => c.CouponId == id));

                return new ResponseDto<CouponDto>()
                {
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<CouponDto>()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto<CouponDto> GetByCode(string code)
        {
            try
            {
                var result = _mapper.Map<CouponDto>(_db.Coupons.First(c => c.CouponCode.ToLower() == code.ToLower()));

                return new ResponseDto<CouponDto>()
                {
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<CouponDto>()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto<CouponDto> Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon currentCoupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(currentCoupon);
                _db.SaveChanges();

                return new ResponseDto<CouponDto>()
                {
                    Result = _mapper.Map<CouponDto>(currentCoupon)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<CouponDto>()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto<CouponDto> Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon currentCoupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(currentCoupon);
                _db.SaveChanges();

                return new ResponseDto<CouponDto>()
                {
                    Result = _mapper.Map<CouponDto>(currentCoupon)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<CouponDto>()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto<CouponDto> Delete(int id)
        {
            try
            {
                _db.Coupons.Remove(_db.Coupons.First(c => c.CouponId == id));
                _db.SaveChanges();

                return new ResponseDto<CouponDto>()
                {
                    Message = "Removed successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<CouponDto>()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                }; ;
            }
        }
    }
}
