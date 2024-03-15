using Mango.Web.Enums;
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

            if (response != null && response.IsSuccess)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(response.Result.ToString());
            }
            if (!User.IsInRole(Helpers.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return NotFound();
            }

            return View(orderHeaderDto);
        }

        [HttpPost]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            await ChangeOrderStatus(orderId, Enum.GetName(OrderStatus.ReadyForPickup));
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            await ChangeOrderStatus(orderId, Enum.GetName(OrderStatus.Completed));
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            await ChangeOrderStatus(orderId, Enum.GetName(OrderStatus.Cancelled));
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeaderDto> list;
            string userId = "";

            if (!User.IsInRole(Helpers.RoleAdmin))
            {
                userId = ReturnUserId();
            }

            ResponseDto response = _orderService.GetAllOrders(userId).GetAwaiter().GetResult();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(response.Result.ToString());

                switch (status)
                {
                    case "approved":
                        list = list.Where(order => order.Status == Enum.GetName(OrderStatus.Approved));
                        break;
                    case "readyforpickup":
                        list = list.Where(order => order.Status == Enum.GetName(OrderStatus.ReadyForPickup));
                        break;
                    case "cancelled":
                        list = list.Where(order => order.Status == Enum.GetName(OrderStatus.Cancelled));
                        break;
                }
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

        private async Task<IActionResult> ChangeOrderStatus(int orderId, string orderStatus)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, orderStatus);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status update successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
            }

            TempData["error"] = "Something was wrong. Status is not updated";
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
        }
    }
}
