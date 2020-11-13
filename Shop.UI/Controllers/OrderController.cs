using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.OrdersAdmin;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Manager")]
    public class OrderController : Controller
    {
        [HttpGet("")]
        public IActionResult GetOrders(int status, [FromServices] GetOrders getOrders) => Ok(getOrders.Do(status));

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id, [FromServices] GetOrder getOrder) => Ok(getOrder.Do(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromServices] UpdateOrder updateOrder)
        {
            var success = await updateOrder.Do(id) > 0;

            if (success)
                return Ok();

            return BadRequest();
        }
    }
}
