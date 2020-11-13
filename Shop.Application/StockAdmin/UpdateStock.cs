using Shop.Domain.Infrastructure;
using Shop.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Shop.Application.StockAdmin
{
    [Service]
    public class UpdateStock
    {
        private readonly IStockManager _stockManager;

        public UpdateStock(IStockManager stockManager)
        {
            _stockManager = stockManager;
        }

        public async Task<Response> Do(Request request)
        {
            var stockList = new List<Stock>();

            foreach (var item in request.Stock)
            {
                stockList.Add(new Stock
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Description = item.Description,
                    Quantity = item.Quantity
                });
            }

            await _stockManager.UpdateStockRange(stockList);

            return new Response
            {
                Stock = request.Stock
            };
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
        }

        public class Request
        {
            public IEnumerable<StockViewModel> Stock { get; set; }
        }

        public class Response
        {
            public IEnumerable<StockViewModel> Stock { get; set; }
        }
    }
}
