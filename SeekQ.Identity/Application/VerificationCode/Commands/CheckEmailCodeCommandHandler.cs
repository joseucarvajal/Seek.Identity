using App.Common.Exceptions;
using App.Common.SeedWork;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class CheckEmailCodeCommandHandler
    {
        public class Command : IRequest<ApplicationUser>
        {
            public Guid UserId { get; set; }
            public string Email { get; set; }
            public string CodeToVerify { get; set; }

            public Command(Guid userId, string phoneNumber, string codeToVerify)
            {
                UserId = userId;
                Email = phoneNumber;                     
                CodeToVerify = codeToVerify;
            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator(Utils utils)
                {
                    RuleFor(x => x.UserId)
                        .NotNull().NotEmpty().WithMessage("User id is not valid");

                    RuleFor(x => x.Email)
                    .Custom((phoneNumberOrEmail, context) =>
                    {
                        if (!utils.IsValidEmail(phoneNumberOrEmail))
                        {
                            context.AddFailure("Add a valid email address");
                        }
                    });

                    RuleFor(x => x.CodeToVerify)
                        .NotNull().NotEmpty().WithMessage("Please enter a verification code")
                        .Length(6).WithMessage("Verification code should have 6 characters");
                }
            }

            public class Handler : IRequestHandler<Command, ApplicationUser>
            {
                private readonly UserManager<ApplicationUser> _userManager;
                private readonly SignUpService _signUpService;
                private readonly Utils _utils;

                public Handler(
                    UserManager<ApplicationUser> userManager,
                    SignUpService signUpService,                    
                    Utils utils
                )
                {
                    _userManager = userManager;
                    _signUpService = signUpService;
                    _utils = utils;
                }

                public async Task<ApplicationUser> Handle(Command request, CancellationToken cancellationToken)
                {
                    try
                    {
                        ManualValidateRequest(request);

                        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

                        if(user.EmailConfirmationCode != request.CodeToVerify)
                        {
                            throw new AppException("The verification code is not valid. Please try again.");
                        }

                        return await _signUpService.UpdateUserAsConfirmed(
                            request.UserId, 
                            request.Email
                        );
                    }
                    catch (AppException)
                    {                        
                        throw;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        throw new AppException("The verification code is not valid. Please try again.");
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
