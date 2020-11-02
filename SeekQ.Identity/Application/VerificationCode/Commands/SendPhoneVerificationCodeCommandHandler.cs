using App.Common.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Options;
using SeekQ.Identity.Application.Services;
using SeekQ.Identity.Models;
using SeekQ.Identity.Twilio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Rest.Verify.V2.Service;

namespace SeekQ.Identity.Application.VerificationCode.Commands
{
    public class SendPhoneVerificationCodeCommandHandler
    {
        public class Command : IRequest<ApplicationUser>
        {
            public string PhoneNumber { get; set; }

            public Command(string phoneNumber)
            {
                PhoneNumber = phoneNumber;
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.PhoneNumber)
                        .NotNull().NotEmpty().WithMessage("Please enter a phone number")
                        //https://www.twilio.com/docs/glossary/what-e164
                        .Matches("^\\+[1-9]\\d{1,14}$").WithMessage("Please enter a valid phone number");
                }
            }

            public class Handler : IRequestHandler<Command, ApplicationUser>
            {
                private readonly TwilioVerifySettings _settings;
                private readonly SignUpService _signUpService;

                public Handler(
                    IOptions<TwilioVerifySettings> settings, 
                    SignUpService signUpService
                )
                {
                    _settings = settings.Value;
                    _signUpService = signUpService;
                }

                public async Task<ApplicationUser> Handle(Command request, CancellationToken cancellationToken)
                {           
                    if (!request.PhoneNumber.StartsWith("+"))
                    {
                        request.PhoneNumber = $"+{request.PhoneNumber}";
                    }

                    //throws and exception is data is not valid
                    ManualValidateRequest(request);                    

                    try
                    {
                        ApplicationUser user = await _signUpService.CreateUserFromPhoneOrEmailAsync(request.PhoneNumber);
                        
                        /*
                        var verification = await VerificationResource.CreateAsync(                                            
                                           to: request.PhoneNumber,
                                           channel: "sms",                                           
                                           pathServiceSid: _settings.VerificationServiceSID
                                       );

                        if(verification.Status != "pending")
                        {
                            throw new AppException("There was an error. Please verify your phone number and try again.");
                        }
                        */

                        return user;
                    }
                    catch (AppException) //User already exist
                    {
                        throw;
                    }
                    catch(Exception) //Twilio exception
                    {
                        throw new AppException("There was an error. Please verify your phone number and try again.");
                    }                    
                }

                private ValidationResult ManualValidateRequest(Command request)
                {
                    var validator = new CommandValidator();
                    var results = validator.Validate(request);
                    if (!results.IsValid)
                    {
                        throw new AppException(results.Errors[0].ErrorMessage);
                    }

                    return results;
                }
            }
        }
    }
}
