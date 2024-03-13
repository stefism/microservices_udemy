using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult OrderIndex()
        {
            return View();
        }

        public async Task<IActionResult> OrderDetail(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new();
            string userId = ReturnUserId();

            var response = await _orderService.GetOrder(orderId);

            if(response != null && response.IsSuccess)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(response.Result.ToString());
            }
            if(!User.IsInRole(Helpers.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return NotFound();
            }
            
            return View(orderHeaderDto);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<OrderHeaderDto> list;
            string userId = "";
            
            if(!User.IsInRole(Helpers.RoleAdmin))
            {
                userId = ReturnUserId();
            }

            ResponseDto response = _orderService.GetAllOrders(userId).GetAwaiter().GetResult();

            if(response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(response.Result.ToString());
            } 
            else
            {
                list = new List<OrderHeaderDto>();
            }

            return Json(new { data = list });
        }

        private string ReturnUserId()
        {
            return User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        }
    }
}
