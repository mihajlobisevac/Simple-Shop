using Microsoft.EntityFrameworkCore;
using Shop.Domain.Enums;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Database
{
    public class OrderManager : IOrderManager
    {
        private readonly ApplicationDbContext _context;

        public OrderManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<int> CreateOrder(Order order)
        {
            _context.Orders.Add(order);

            return _context.SaveChangesAsync();
        }

        public IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Func<Order, TResult> selector)
        {
            return _context.Orders
                .Where(x => x.Status == status)
                .Select(selector)
                .ToList();
        }

        public TResult GetOrderById<TResult>(int id, Func<Order, TResult> selector)
        {
            return GetOrder(x => x.Id == id, selector);
        }

        public TResult GetOrderByReference<TResult>(string reference, Func<Order, TResult> selector)
        {
            return GetOrder(x => x.OrderRef == reference, selector);
        }

        public bool OrderReferenceExists(string reference)
        {
            return _context.Orders.Any(x => x.OrderRef == reference);
        }

        private TResult GetOrder<TResult>(Func<Order, bool> condition, Func<Order, TResult> selector)
        {
            return _context.Orders
                .Where(x => condition(x))
                .Include(x => x.OrderStocks)
                .ThenInclude(x => x.Stock)
                .ThenInclude(x => x.Product)
                .Select(selector)
                .FirstOrDefault();
        }

        public Task<int> AdvanceOrder(int id)
        {
            _context.Orders.FirstOrDefault(x => x.Id == id).Status++;

            return _context.SaveChangesAsync();
        }
    }
}
