using gRPCvsREST.Core.Dto;
using gRPCvsREST.ServerApi.gRPC;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace gRPCvsREST.BenchmarkApi.Services
{
    public class ServerApiService
    {
        private readonly HttpClient _httpClient;
        private readonly Orders.OrdersClient _ordersGrpcClient;

        public ServerApiService(HttpClient httpClient, Orders.OrdersClient ordersGrpcClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:64292");

            _ordersGrpcClient = ordersGrpcClient;
        }

        public async Task<List<OrderDto>> GetOrdersUsingRestAsync()
        {
            var response = await _httpClient.GetAsync("/api/Order/get-orders");

            return await DeserializeResponseObject<List<OrderDto>>(response);
        }

        public async Task<List<OrderDto>> GetOrdersUsingGrpcAsync()
        {
            GetOrdersResponse response = await _ordersGrpcClient.GetOrdersAsync(new GerOrdersRequest());

            if (response != null)
            {
                List<OrderDto> orderDtos = new List<OrderDto>(response.Orderslist.Count);
                foreach (var order in response.Orderslist)
                {
                    var orderDto = new OrderDto
                    {
                        ClientName = order.Clientname,
                        Discount = (decimal)order.Discount,
                        Id = new Guid(order.Id),
                        Total = (decimal)order.Total,
                        Items = new List<OrderItemDto>(order.Orderitems.Count)
                    };

                    foreach (var item in order.Orderitems)
                    {
                        var orderItemDto = new OrderItemDto
                        {
                            OrderId = new Guid(item.Orderid),
                            ProductId = new Guid(item.Productid),
                            ProductName = item.Productname,
                            Quantity = item.Quantity,
                            UnitPrice = (decimal)item.Unitprice
                        };
                        orderDto.Items.Add(orderItemDto);
                    }

                    orderDtos.Add(orderDto);
                }

                return orderDtos;
            }

            return new List<OrderDto>(0);
        }

        protected async Task<T> DeserializeResponseObject<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }
    }
}
