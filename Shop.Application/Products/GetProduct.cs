using Shop.Domain.Models;
using Shop.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Application.Products
{
    [Service]
    public class GetProduct
    {
        private readonly IStockManager _stockManager;
        private readonly IProductManager _productManager;

        public GetProduct(IStockManager stockManager, IProductManager productManager)
        {
            _stockManager = stockManager;
            _productManager = productManager;
        }

        public async Task<ProductViewModel> Do(string name) 
        {
            await _stockManager.RetrieveExpiredStockOnHold();

            return _productManager.GetProductByName(name, Projection);
        }            

        public class ProductViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Value { get; set; }
            public IEnumerable<StockViewModel> Stock { get; set; }
        }

        public class StockViewModel
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
        }

        private static Func<Product, ProductViewModel> Projection = (product) =>
             new ProductViewModel
             {
                 Name = product.Name,
                 Description = product.Description,
                 Value = product.Value.GetValueString(),

                 Stock = product.Stock.Select(y => new StockViewModel
                 {
                     Id = y.Id,
                     Description = y.Description,
                     Quantity = y.Quantity
                 })
             };
    }
}
