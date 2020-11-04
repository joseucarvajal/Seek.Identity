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
    public class CheckPhoneCodeCommandHandler
    {
        public class Command : IRequest<ApplicationUser>
        {
            public Guid UserId { get; set; }
            public string PhoneNumber { get; set; }
            public string CodeToVerify { get; set; }

            public Command(Guid userId, string phoneNumber, string codeToVerify)
            {
                UserId = userId;
                PhoneNumber = phoneNumber;                     
                CodeToVerify = codeToVerify;
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.UserId)
                        .NotNull().NotEmpty().WithMessage("User id is not valid");

                    RuleFor(x => x.PhoneNumber)
                        .NotNull().NotEmpty().WithMessage("Please enter a phone number")
                        //https://www.twilio.com/docs/glossary/what-e164
                        .Matches("^\\+[1-9]\\d{1,14}$").WithMessage("Please enter a valid phone number");

                    RuleFor(x => x.CodeToVerify)
                        .NotNull().NotEmpty().WithMessage("Please enter a verification code")
                        .Length(6).WithMessage("Verification code should have 6 characters");
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
                    try
                    {
                        ManualValidateRequest(request);
                        
                        var verification = await VerificationCheckResource.CreateAsync(
                                           to: request.PhoneNumber,
                                           code: request.CodeToVerify,
                                           pathServiceSid: _settings.VerificationServiceSID
                                       );

                        if(verification.Status != "approved")
                        {
                            throw new AppException("The verification code is not valid. Please try again.");
                        }

                        return await _signUpService.UpdateUserAsConfirmed(
                            request.UserId, 
                            request.PhoneNumber
                        );
                    }
                    catch (AppException)
                    {                        
                        throw;
                    }
                    catch(Exception e) //Twilio exception
                    {
                        Console.WriteLine(e.ToString());
                        throw new AppException("The verification code is not valid. Please try again.");
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
