using App.Common.Exceptions;
using App.Common.SeedWork;
using App.Common.SeedWork.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SeekQ.Identity.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeekQ.Identity.Application.Profile.Commands
{
    public class ChangeUserPasswordCommandHandler
    {
        public class Command : IRequest<ApplicationUser>
        {
            public string PhoneNumberOrEmail { get; set; }
            public string Password { get; set; }
            public string PasswordConfirm { get; set; }            

            public Command()
            {

            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator(Utils utils)
                {
                    RuleFor(x => x.PhoneNumberOrEmail)
                        .NotNull().NotEmpty().WithMessage("Please enter a valid phone number or email");

                    RuleFor(x => x.PhoneNumberOrEmail)                            
                        //https://www.twilio.com/docs/glossary/what-e164
                        .Matches("^\\+[1-9]\\d{1,14}$").WithMessage("Please enter a valid phone number")
                            .When(c => !c.PhoneNumberOrEmail.Contains("@"));

                    RuleFor(x => x.PhoneNumberOrEmail)
                        .Custom((phoneNumberOrEmail, context) =>
                        {
                            if (phoneNumberOrEmail.Contains('@') && !utils.IsValidEmail(phoneNumberOrEmail))
                            {
                                context.AddFailure("Add a valid email address");
                            }
                        });

                    RuleFor(x => x.Password)
                        .NotNull().NotEmpty().WithMessage("Please enter a password");

                    RuleFor(x => x.PasswordConfirm)
                        .NotNull().NotEmpty().WithMessage("Please enter a password confirmation");

                    RuleFor(x => x.PasswordConfirm)
                        .Equal(c => c.Password).WithMessage("Please make sure passwords match");
                }
            }

            public class Handler : IRequestHandler<Command, ApplicationUser>
            {
                private UserManager<ApplicationUser> _userManager;
                private Utils _utils;

                public Handler(UserManager<ApplicationUser> userManager, Utils utils)
                {
                    _userManager = userManager;
                    _utils = utils;
                }

                public async Task<ApplicationUser> Handle(Command request, CancellationToken cancellationToken)
                {
                    //Email has been already validated with fluent validation
                    var isEmail = request.PhoneNumberOrEmail.Contains('@');

                    ApplicationUser appUser = new ApplicationUser
                    {
                        UserName = request.PhoneNumberOrEmail,
                        Email = isEmail ? request.PhoneNumberOrEmail : "",
                        PhoneNumber = !isEmail ? request.PhoneNumberOrEmail : "",
                        EmailConfirmed = isEmail,
                        PhoneNumberConfirmed = !isEmail
                    };

                    var result = await _userManager.CreateAsync(appUser);
                    if(result.Succeeded == false)
                    {
                        throw new AppException(result.Errors.ElementAt(0).Description);
                    }

                    return appUser;
                }               
            }
        }

    }
}
