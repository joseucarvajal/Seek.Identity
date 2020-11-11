namespace SeekQ.Identity.Application.Profile.Profile.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.Exceptions;
    using FluentValidation;
    using MediatR;
    using Models;
    using Models.Profile;
    using SeekQ.Identity.Data;

    public class CreateUserCommandHandler
    {
        public class Command : IRequest<ApplicationUser>
        {
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
            private ApplicationDbContext _applicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _applicationDbContext = applicationDbContext;
            }

            public async Task<ApplicationUser> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    Guid UserId = Guid.NewGuid();
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

                    ApplicationUser user = new ApplicationUser()
                    {
                        Id = UserId.ToString(),
                        MakeFirstNamePublic = MakeFirstNamePublic,
                        MakeLastNamePublic = MakeLastNamePublic,
                        MakeBirthDatePublic = MakeBirthDatePublic,
                        NickName = NickName,
                        FirstName = FirstName,
                        BirthDate = BirthDate,
                        School = School,
                        Job = Job,
                        About = About,
                        GenderId = GenderId
                    };

                    _applicationDbContext.ApplicationUsers.Add(user);
                    await _applicationDbContext.SaveChangesAsync();

                    return user;
                }
                catch (Exception e)
                {
                    throw new AppException(e.Message);
                }
            }
        }
    }
}