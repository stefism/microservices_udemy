using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CouponAPIController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public List<Coupon> Get()
        {
            try
            {
                return _db.Coupons.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public Coupon Get(int id)
        {
            try
            {
                return _db.Coupons.FirstOrDefault(c => c.CouponId == id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
