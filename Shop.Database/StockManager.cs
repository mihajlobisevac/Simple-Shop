using Microsoft.EntityFrameworkCore;
using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Database
{
    public class StockManager : IStockManager
    {
        private readonly ApplicationDbContext _context;

        public StockManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<int> CreateStock(Stock stock)
        {
            _context.Stock.Add(stock);

            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteStock(int id)
        {
            var stock = _context.Stock.FirstOrDefault(x => x.Id == id);
            _context.Stock.Remove(stock);

            return _context.SaveChangesAsync();
        }

        public Task<int> UpdateStockRange(List<Stock> stockList)
        {
            _context.Stock.UpdateRange(stockList);

            return _context.SaveChangesAsync();
        }

        public bool EnoughStock(int stockId, int qty)
        {
            return _context.Stock.FirstOrDefault(x => x.Id == stockId).Quantity >= qty;
        }

        public Stock GetStockWithProduct(int stockId) =>
            _context.Stock
                .Include(x => x.Product)
                .FirstOrDefault(x => x.Id == stockId);

        public Task PutStockOnHold(int stockId, int qty, string sessionId)
        {
            _context.Stock.FirstOrDefault(x => x.Id == stockId).Quantity -= qty;

            var stockOnHold = _context.StocksOnHold
                .Where(x => x.SessionId == sessionId)
                .ToList();

            if (stockOnHold.Any(x => x.StockId == stockId))
            {
                stockOnHold.Find(x => x.StockId == stockId).Quantity += qty;
            }
            else
            {
                _context.StocksOnHold.Add(new StockOnHold
                {
                    StockId = stockId,
                    SessionId = sessionId,
                    Quantity = qty,
                    ExpiryDate = DateTime.Now.AddMinutes(20)
                });
            }

            foreach (var stock in stockOnHold)
            {
                stock.ExpiryDate = DateTime.Now.AddMinutes(20);
            }

            return _context.SaveChangesAsync();
        }

        public Task RemoveStockFromHold(string sessionId)
        {
            var stockOnHold = _context
                .StocksOnHold
                .Where(x => x.SessionId == sessionId)
                .ToList();

            _context.StocksOnHold.RemoveRange(stockOnHold);

            return _context.SaveChangesAsync();
        }

        public Task RemoveStockFromHold(int stockId, int qty, string sessionId)
        {
            var stockOnHold = _context.StocksOnHold
                .FirstOrDefault(x => x.StockId == stockId && x.SessionId == sessionId);

            var stock = _context.Stock.FirstOrDefault(x => x.Id == stockId);

            stock.Quantity += qty;
            stockOnHold.Quantity -= qty;

            if (stockOnHold.Quantity <= 0)
            {
                _context.Remove(stockOnHold);
            }

            return _context.SaveChangesAsync();
        }

        public Task RetrieveExpiredStockOnHold()
        {
            var stocksOnHold = _context.StocksOnHold.Where(x => x.ExpiryDate < DateTime.Now).ToList();

            if (stocksOnHold.Count > 0)
            {
                var stocksToReturn = _context.Stock
                    .AsEnumerable()
                    .Where(x => stocksOnHold
                    .Any(y => y.StockId == x.Id))
                    .ToList();

                foreach (var stock in stocksToReturn)
                {
                    stock.Quantity += stocksOnHold.FirstOrDefault(x => x.StockId == stock.Id).Quantity;
                }

                _context.StocksOnHold.RemoveRange(stocksOnHold);

                return _context.SaveChangesAsync();
            }

            return Task.CompletedTask;
        }
    }
}
