namespace SeekQ.Identity.Application.Profile.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.Exceptions;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using SeekQ.Identity.Data;
    using SeekQ.Identity.Models;

    public class UpdateUserCommandHandler
    {
        public class Command : IRequest<ApplicationUser>
        {
            public Guid UserId { get; set; }

            public bool MakeFirstNamePublic { get; set; }
            public bool MakeLastNamePublic { get; set; }
            public bool MakeBirthDatePublic { get; set; }

            public string NickName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime BirthDate { get; set; }
            public string School { get; set; }
            public string Job { get; set; }
            public string About { get; set; }

            public int? GenderId { get; set; }
            public UserGender Gender { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.NickName)
                    .NotNull().NotEmpty().WithMessage("NickName is required")
                    .MaximumLength(20).WithMessage("NickName cannot have more than 50 characters.");

                RuleFor(x => x.FirstName)
                    .NotNull().NotEmpty().WithMessage("FirstName is required")
                    .MaximumLength(50).WithMessage("FirstName cannot have more than 50 characters.");

                RuleFor(x => x.LastName)
                    .NotNull().NotEmpty().WithMessage("LastName is required")
                    .MaximumLength(50).WithMessage("LastName cannot have more than 50 characters.");

                RuleFor(x => x.School)
                    .NotNull().NotEmpty().WithMessage("School is required")
                    .MaximumLength(50).WithMessage("School cannot have more than 50 characters.");

                RuleFor(x => x.Job)
                    .NotNull().NotEmpty().WithMessage("Job is required")
                    .MaximumLength(50).WithMessage("Job cannot have more than 50 characters.");

                RuleFor(x => x.About)
                    .NotNull().NotEmpty().WithMessage("About is required")
                    .MaximumLength(1000).WithMessage("About cannot have more than 50 characters.");
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
                bool MakeFirstNamePublic = request.MakeFirstNamePublic;
                bool MakeLastNamePublic = request.MakeLastNamePublic;
                bool MakeBirthDatePublic = request.MakeBirthDatePublic;
                string NickName = request.NickName;
                string FirstName = request.FirstName;
                DateTime BirthDate = request.BirthDate;
                string School = request.School;
                string Job = request.Job;
                string About = request.About;

                ApplicationUser user = await _userManager.FindByIdAsync(request.UserId.ToString());
                user.MakeFirstNamePublic = MakeFirstNamePublic;
                user.MakeLastNamePublic = MakeLastNamePublic;
                user.MakeBirthDatePublic = MakeBirthDatePublic;
                user.NickName = NickName;
                user.FirstName = FirstName;
                user.BirthDate = BirthDate;
                user.School = School;
                user.Job = Job;
                user.About = About;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded == false)
                {
                    throw new AppException(result.Errors.ElementAt(0).Description);
                }

                return user;
            }
        }
    }
}