using App.Common.Exceptions;
using App.Common.SeedWork;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SeekQ.Identity.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SeekQ.Identity.Application.Profile.Commands
{
    public class SetUserPasswordCommandHandler
    {
        public class Command : IRequest<ApplicationUser>
        {
            public Guid UserId { get; set; }
            public string Password { get; set; }
            public string PasswordConfirm { get; set; }            

            public Command()
            {

            }

            public class CommandValidator : AbstractValidator<Command>
            {
                public CommandValidator()
                {
                    RuleFor(x => x.UserId)
                        .NotNull().NotEmpty().WithMessage("User id can not be empty");

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

                public Handler(UserManager<ApplicationUser> userManager)
                {
                    _userManager = userManager;
                }

                public async Task<ApplicationUser> Handle(Command request, CancellationToken cancellationToken)
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(request.UserId.ToString());
                    var result = await _userManager.AddPasswordAsync(user, request.Password);
                    if(result.Succeeded == false)
                    {
                        throw new AppException(result.Errors.ElementAt(0).Description);
                    }

                    return user;
                }               
            }
        }

    }
}
