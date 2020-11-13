using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.UI.ViewModels.Admin;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.UI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel vm)
        {
            var managerUser = new IdentityUser
            {
                UserName = vm.Username
            };

            await _userManager.CreateAsync(managerUser, "password");

            await _userManager.AddClaimAsync(managerUser, new Claim("Role", "Manager"));

            return Ok();
        }
    }
}
