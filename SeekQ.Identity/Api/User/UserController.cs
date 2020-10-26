using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SeekQ.Identity.Application.Commands;
using SeekQ.Identity.Models;
using SeekQ.Identity.Twilio;
using System;
using System.Threading.Tasks;
using Twilio.Rest.Verify.V2.Service;

namespace SeekQ.Identity.Api.User
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;        
        private readonly IMediator _mediator;

        public UserController(
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

        [HttpPost]
        [Route("sendverification/{phoneNumber}")]
        public async Task<ActionResult<Unit>> SendVerification(
        [FromRoute] string phoneNumber
    )
        {
            /*

            var verification = await VerificationResource.CreateAsync(
                               to: phoneNumber,
                               channel: "sms",
                               pathServiceSid: _settings.VerificationServiceSID
                           );
            */
            return await _mediator.Send(new SendPhoneVerificationCodeCommandHandler.Command(phoneNumber));
        }
    }
}
