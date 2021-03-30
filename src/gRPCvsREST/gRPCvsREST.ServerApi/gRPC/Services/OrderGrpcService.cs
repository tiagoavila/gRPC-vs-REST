using Grpc.Core;
using gRPCvsREST.Core.Dto;
using gRPCvsREST.ServerApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gRPCvsREST.ServerApi.gRPC.Services
{
    public class OrderGrpcService : Orders.OrdersBase
    {
        private readonly OrderService _orderService;

        public OrderGrpcService()
        {
            _orderService = new OrderService();
        }

        public override async Task<GetOrdersResponse> GetOrders(GerOrdersRequest request, ServerCallContext context)
        {
            List<OrderDto> orders = _orderService.GetOrders();
            return MapListOfOrdersToOrdersResponse(orders);
        }

        private GetOrdersResponse MapListOfOrdersToOrdersResponse(List<OrderDto> orders)
        {
            GetOrdersResponse getOrdersResponse = new GetOrdersResponse();

            foreach (var order in orders)
            {
                OrderResponse orderResponse = new OrderResponse
                {
                    Id = order.Id.ToString(),
                    Clientname = order.ClientName,
                    Discount = (double)order.Discount,
                    Total = (double)order.Total
                };

                foreach (var orderItem in order.Items)
                {
                    OrderItemResponse orderItemResponse = new OrderItemResponse
                    {
                        Orderid = orderItem.OrderId.ToString(),
                        Productid = orderItem.ProductId.ToString(),
                        Productname = orderItem.ProductName,
                        Quantity = orderItem.Quantity,
                        Unitprice = (double)orderItem.UnitPrice
                    };
                    orderResponse.Orderitems.Add(orderItemResponse);
                }

                getOrdersResponse.Orderslist.Add(orderResponse);
            }

            return getOrdersResponse;
        }
    }
}
