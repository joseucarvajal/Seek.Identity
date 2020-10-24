using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeekQ.Identity.Models;
using System.Threading.Tasks;

namespace SeekQ.Identity.Api.User
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }



        [HttpPost]
        [Route("initialcreate/fromphone/{phoneNumber}")]
        public async Task<ActionResult<ApplicationUser>> InitialCreateFromPhoneNumber(
        [FromRoute] string phoneNumber
    )
        {
            ApplicationUser appUser = new ApplicationUser
            {
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false
            };

            var result = await _userManager.CreateAsync(appUser);

            return Ok(appUser);
        }
    }
}
