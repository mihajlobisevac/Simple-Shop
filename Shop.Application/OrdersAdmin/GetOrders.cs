using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;

namespace Shop.Application.OrdersAdmin
{
    [Service]
    public class GetOrders
    {
        private readonly IOrderManager _orderManager;

        public GetOrders(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public IEnumerable<Response> Do(int status) => _orderManager.GetOrdersByStatus((OrderStatus)status, Projection);

        public class Response
        {
            public int Id { get; set; }
            public string OrderRef { get; set; }
            public string Email { get; set; }
        }

        private static Func<Order, Response> Projection = (order) =>
             new Response
             {
                 Id = order.Id,
                 OrderRef = order.OrderRef,
                 Email = order.Email
             };
    }
}
