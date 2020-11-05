using App.Common.Exceptions;
using App.Common.SeedWork;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Core;
using SeekQ.Identity.Application.Services;
using SeekQ.Identity.Models;
using SeekQ.Identity.Twilio;
using System;
using System.Threading;
using System.Threading.Tasks;

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
                            context.AddFailure("Please enter a valid email address");
                        }
                    });
                }
            }

            public class Handler : IRequestHandler<Command, ApplicationUser>
            {                
                private readonly SignUpService _signUpService;
                private readonly Utils _utils;
                private readonly IEmailService _emailService;

                public Handler(
                    SignUpService signUpService,
                    Utils utils,
                    IEmailService emailService
                )
                {
                    _signUpService = signUpService;
                    _utils = utils;
                    _emailService = emailService;
                }

                public async Task<ApplicationUser> Handle(Command request, CancellationToken cancellationToken)
                {           

                    //throws and exception is data is not valid
                    ManualValidateRequest(request);                    

                    try
                    {
                        ApplicationUser user = await _signUpService.CreateUserFromPhoneOrEmailAsync(request.Email);
                        await _emailService.SendAsync(
                            request.Email,
                            "SeekQ verification code",
                            $"Hi,<br/>This is your SeekQ sign-up verification code <b>{user.EmailConfirmationCode}</b><div style='display:flex; justify-content:center'><img src='http://localhost:32700/SeekQ_logo-1.png' style='width:20%'/></div>", true);
                       
                        return user;
                    }
                    catch (AppException) //User already exist
                    {                    
                        throw;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
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
