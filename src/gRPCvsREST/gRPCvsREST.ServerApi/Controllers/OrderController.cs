using gRPCvsREST.Core.Dto;
using gRPCvsREST.ServerApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace gRPCvsREST.ServerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController()
        {
            _orderService = new OrderService();
        }

        [HttpGet("get-orders")]
        public IActionResult GetOrders()
        {
            List<OrderDto> orders = _orderService.GetOrders();
            return Ok(orders);
        }
    }
}
