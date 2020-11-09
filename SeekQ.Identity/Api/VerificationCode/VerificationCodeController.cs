using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeekQ.Identity.Application.VerificationCode.Commands;
using SeekQ.Identity.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace SeekQ.Identity.Api.VerificationCode
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VerificationCodeController : ControllerBase
    {        
        private readonly IMediator _mediator;

        public VerificationCodeController(
            IMediator mediator
        )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));            
        }

        [HttpPost]
        [Route("send/{phoneNumberOrEmail}")]
        [SwaggerOperation(Summary = "Send code verification to a mobile phone via Twilio or Email and creates user")]        
        public async Task<ActionResult<ApplicationUser>> SendVerification(
            [SwaggerParameter(Description = "Phone number with indicative e.g. (+57...)")]
            [FromRoute] string phoneNumberOrEmail
        )
        {
            if (!phoneNumberOrEmail.IsNullOrEmpty() && phoneNumberOrEmail.Contains('@'))
            {
                return await _mediator.Send(
                    new SendEmailVerificationCodeCommandHandler.Command(phoneNumberOrEmail)
                );
            }

            return await _mediator.Send(
                new SendPhoneVerificationCodeCommandHandler.Command(phoneNumberOrEmail)
            );          

        }

        [HttpPost]
        [Route("check")]
        [SwaggerOperation(Summary = "Checks whether a validation code is valid or not and updates user")]
        public async Task<ActionResult<ApplicationUser>> Validate(
            [FromBody] CheckCodeParams verifyPhoneOrEmailCodeParams
        )
        {
            Guid userId = verifyPhoneOrEmailCodeParams.UserId;
            string phoneOrEmail = verifyPhoneOrEmailCodeParams.PhoneOrEmail;
            string codeToVerify = verifyPhoneOrEmailCodeParams.CodeToVerify;

            if (!phoneOrEmail.IsNullOrEmpty() && phoneOrEmail.Contains('@'))
            {
                return await _mediator.Send(
                    new CheckEmailCodeCommandHandler.Command(
                        userId,
                        phoneOrEmail,
                        codeToVerify
                    )
               );
            }                

            return await _mediator.Send(
                new CheckPhoneCodeCommandHandler.Command(
                    userId, 
                    phoneOrEmail, 
                    codeToVerify
                )
            );
        }
    }
}
