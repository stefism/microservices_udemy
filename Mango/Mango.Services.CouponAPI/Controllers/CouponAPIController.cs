using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                var result = _mapper.Map<CouponDto>(_db.Coupons.FirstOrDefault(c => c.CouponId == id));

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
    }
}
