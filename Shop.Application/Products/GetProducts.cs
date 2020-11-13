
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shop.Domain.Infrastructure;

namespace Shop.Application.Products
{
    [Service]
    public class GetProducts
    {
        private readonly IProductManager _context;

        public GetProducts(IProductManager context)
        {
            _context = context;
        }

        public IEnumerable<ProductViewModel> Do() =>
            _context.GetProductsWithStock(x => 
                new ProductViewModel
                {
                    Name = x.Name,
                    Description = x.Description,
                    Value = x.Value.GetValueString(),

                    StockCount = x.Stock.Sum(y => y.Quantity)
                });

        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public int StockCount { get; set; }
        }
    }
}
