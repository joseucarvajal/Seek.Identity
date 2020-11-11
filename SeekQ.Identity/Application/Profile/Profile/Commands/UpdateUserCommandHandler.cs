namespace SeekQ.Identity.Application.Profile.Profile.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.Exceptions;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Profile;
    using SeekQ.Identity.Data;

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
                try
                {
                    string UserId = request.UserId.ToString();

                    var existingUser = await _userManager.FindByIdAsync(UserId);

                    if (existingUser == null)
                    {
                        throw new AppException($"The UserId {UserId} doesn't exist.");
                    }

                    bool MakeFirstNamePublic = request.MakeFirstNamePublic;
                    bool MakeLastNamePublic = request.MakeLastNamePublic;
                    bool MakeBirthDatePublic = request.MakeBirthDatePublic;
                    string NickName = request.NickName;
                    string FirstName = request.FirstName;
                    string LastName = request.LastName;
                    DateTime BirthDate = request.BirthDate;
                    string School = request.School;
                    string Job = request.Job;
                    string About = request.About;
                    int? GenderId = request.GenderId;

                    existingUser.MakeFirstNamePublic = MakeFirstNamePublic;
                    existingUser.MakeLastNamePublic = MakeLastNamePublic;
                    existingUser.MakeBirthDatePublic = MakeBirthDatePublic;
                    existingUser.NickName = NickName;
                    existingUser.FirstName = FirstName;
                    existingUser.LastName = LastName;
                    existingUser.BirthDate = BirthDate;
                    existingUser.School = School;
                    existingUser.Job = Job;
                    existingUser.About = About;
                    existingUser.GenderId = GenderId;

                    var result = await _userManager.UpdateAsync(existingUser);

                    if (result.Succeeded == false)
                    {
                        throw new AppException(result.Errors.ElementAt(0).Description);
                    }

                    return existingUser;
                }
                catch (Exception e)
                {
                    throw new AppException(e.Message);
                }
            }
        }
    }
}