﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.StockAdmin;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Manager")]
    public class StockController : Controller
    {
        [HttpGet("")]
        public IActionResult GetStock([FromServices] GetStock getStock) => 
            Ok(getStock.Do());

        [HttpPost("")]
        public async Task<IActionResult> CreateStock([FromBody] CreateStock.Request request, [FromServices] CreateStock createStock) => 
            Ok(await createStock.Do(request));

        [HttpPut("")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStock.Request request, [FromServices] UpdateStock updateStock) =>
            Ok(await updateStock.Do(request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id, [FromServices] DeleteStock deleteStock) => 
            Ok(await deleteStock.Do(id));
    }
}
