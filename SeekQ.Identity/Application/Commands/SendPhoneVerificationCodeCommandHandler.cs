using App.Common.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Options;
using SeekQ.Identity.Twilio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Rest.Verify.V2.Service;

namespace SeekQ.Identity.Application.Commands
{
    public class SendPhoneVerificationCodeCommandHandler
    {
        public class Command : IRequest<Unit>
        {
            public string PhoneNumber { get; set; }

            public Command(String phoneNumber)
            {
                PhoneNumber = phoneNumber;
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.PhoneNumber)
                        .NotNull().NotEmpty().WithMessage("Please enter phone number")
                        //https://www.twilio.com/docs/glossary/what-e164
                        .Matches("^\\+[1-9]\\d{1,14}$").WithMessage("Please enter a valid phone number");
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
                    if (!request.PhoneNumber.StartsWith("+"))
                    {
                        request.PhoneNumber = $"+{request.PhoneNumber}";
                    }

                    ManualValidateRequest(request);

                    try
                    {
                        var verification = await VerificationResource.CreateAsync(
                                           to: request.PhoneNumber,
                                           channel: "sms",
                                           pathServiceSid: _settings.VerificationServiceSID
                                       );

                    }
                    catch(Exception) //Twilio exception
                    {
                        throw new AppException("There was an error. Please verify your phone number and try again.");
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
