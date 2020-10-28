using App.Common.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Options;
using SeekQ.Identity.Twilio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Rest.Verify.V2.Service;

namespace SeekQ.Identity.Application.CodeVerification.Commands
{
    public class VerifyPhoneCodeCommandHandler
    {
        public class Command : IRequest<Unit>
        {
            public string PhoneNumber { get; set; }
            public string CodeToVerify { get; set; }

            public Command(string phoneNumber, string codeToVerify)
            {
                PhoneNumber = phoneNumber;                     
                CodeToVerify = codeToVerify;
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.PhoneNumber)
                        .NotNull().NotEmpty().WithMessage("Please enter a phone number")
                        //https://www.twilio.com/docs/glossary/what-e164
                        .Matches("^\\+[1-9]\\d{1,14}$").WithMessage("Please enter a valid phone number");

                    RuleFor(x => x.CodeToVerify)
                        .NotNull().NotEmpty().WithMessage("Please enter a verification code")
                        .Length(6).WithMessage("Verification code should have 6 characters");
                }
            }

            public class Handler : IRequestHandler<Command, Unit>
            {
                private readonly TwilioVerifySettings _settings;

                public Handler(IOptions<TwilioVerifySettings> settings)
                {
                    _settings = settings.Value;
                }

                public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
                {           
                    ManualValidateRequest(request);

                    try
                    {
                        var verification = await VerificationCheckResource.CreateAsync(
                                           to: request.PhoneNumber,
                                           code: request.CodeToVerify,
                                           pathServiceSid: _settings.VerificationServiceSID
                                       );

                        if(verification.Status != "approved")
                        {
                            throw new AppException("The verification code is not valid. Please try again.");
                        }
                    }
                    catch(Exception) //Twilio exception
                    {
                        throw new AppException("The verification code is not valid. Please try again.");
                    }

                    return Unit.Value;
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
