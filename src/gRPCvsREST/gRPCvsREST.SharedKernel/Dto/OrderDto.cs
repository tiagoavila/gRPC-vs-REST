using System;
using System.Collections.Generic;

namespace gRPCvsREST.Core.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ClientName { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
