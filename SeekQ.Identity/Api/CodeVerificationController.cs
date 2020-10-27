using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeekQ.Identity.Application.Commands.CodeVerification;
using SeekQ.Identity.Models;
using System;
using System.Threading.Tasks;

namespace SeekQ.Identity.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CodeVerificationController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;        
        private readonly IMediator _mediator;

        public CodeVerificationController(
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


        [HttpGet]
        [Route("send/{phoneNumberOrEmail}")]
        public async Task<ActionResult<Unit>> SendVerification(
            [FromRoute] string phoneNumberOrEmail
        )
        {
            return await _mediator.Send(new SendPhoneCodeVerificationCommandHandler.Command(phoneNumberOrEmail));
        }

        [HttpPost]
        [Route("verify")]
        public async Task<ActionResult<Unit>> Validate(
            [FromBody] VerifyPhoneOrEmailCodeParams verifyPhoneOrEmailCodeParams
        )
        {
            string phoneOrEmail = verifyPhoneOrEmailCodeParams.PhoneOrEmail;
            string codeToVerify = verifyPhoneOrEmailCodeParams.CodeToVerify;

            return await _mediator.Send(new VerifyPhoneCodeCommandHandler.Command(phoneOrEmail, codeToVerify));
        }
    }
}
