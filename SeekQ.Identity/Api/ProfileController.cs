using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeekQ.Identity.Models;
using System;
using System.Threading.Tasks;

namespace SeekQ.Identity.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;        
        private readonly IMediator _mediator;

        public ProfileController(
            IMediator mediator,
            UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
