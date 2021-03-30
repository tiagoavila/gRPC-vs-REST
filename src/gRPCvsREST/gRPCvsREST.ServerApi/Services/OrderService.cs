using gRPCvsREST.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace gRPCvsREST.ServerApi.Services
{
    public class OrderService
    {
        public List<OrderDto> GetOrders()
        {
            List<OrderDto> orders = new List<OrderDto>(10);

            for (int i = 0; i < 10; i++)
            {
                Guid orderId = Guid.NewGuid();

                var order = new OrderDto
                {
                    Id = orderId,
                    ClientName = $"Client {i + 1}",
                    CreatedDate = DateTime.UtcNow,
                    Discount = 0,
                    Total = 100,
                    Items = Enumerable.Range(0, 20).Select(value => new OrderItemDto
                    {
                        OrderId = orderId,
                        ProductId = Guid.NewGuid(),
                        ProductName = $"Product {value}",
                        Quantity = 5,
                        UnitPrice = 5
                    }).ToList()
                };

                orders.Add(order);
            }

            return orders;
        }
    }
}
