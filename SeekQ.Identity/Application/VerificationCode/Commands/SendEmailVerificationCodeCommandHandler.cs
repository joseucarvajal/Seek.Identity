using App.Common.Exceptions;
using App.Common.SeedWork;
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
    public class SendEmailVerificationCodeCommandHandler
    {
        public class Command : IRequest<ApplicationUser>
        {
            public string Email { get; set; }

            public Command(string email)
            {
                Email = email;
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator(Utils utils)
                {
                    RuleFor(x => x.Email)
                    .Custom((phoneNumberOrEmail, context) =>
                    {
                        if (!utils.IsValidEmail(phoneNumberOrEmail))
                        {
                            context.AddFailure("Add a valid email address");
                        }
                    });
                }
            }

            public class Handler : IRequestHandler<Command, ApplicationUser>
            {
                private readonly TwilioVerifySettings _settings;
                private readonly SignUpService _signUpService;
                private readonly Utils _utils;

                public Handler(
                    IOptions<TwilioVerifySettings> settings, 
                    SignUpService signUpService,
                    Utils utils
                )
                {
                    _settings = settings.Value;
                    _signUpService = signUpService;
                    _utils = utils;
                }

                public async Task<ApplicationUser> Handle(Command request, CancellationToken cancellationToken)
                {           

                    //throws and exception is data is not valid
                    ManualValidateRequest(request);                    

                    try
                    {
                        ApplicationUser user = await _signUpService.CreateUserFromPhoneOrEmailAsync(request.Email);
                                                
                       
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
                    var validator = new CommandValidator(_utils);
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
