namespace SeekQ.Identity.Application.Profile.Commands
{
    using System;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.SeedWork;
    using Dapper;
    using Dapper.Contrib.Extensions;
    using FluentValidation;
    using MediatR;
    using Microsoft.Data.SqlClient;
    using SeekQ.Identity.Models;

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
            private CommonGlobalAppSingleSettings _commonGlobalAppSingleSettings;

            public Handler(CommonGlobalAppSingleSettings commonGlobalAppSingleSettings)
            {
                _commonGlobalAppSingleSettings = commonGlobalAppSingleSettings;
            }

            public async Task<ApplicationUser> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    using (IDbConnection conn = new SqlConnection(_commonGlobalAppSingleSettings.MssqlConnectionString))
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

                        ApplicationUser user = new ApplicationUser
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

                        var result = await conn.ExecuteAsync(
                            @"Insert Into AspNetUsers
                                (   Id,
                                    MakeFirstNamePublic,
                                    MakeLastNamePublic,
                                    MakeBirthDatePublic,
                                    NickName,
                                    FirstName,
                                    LastName,
                                    BirthDate,
                                    School,
                                    Job,
                                    About,
                                    GenderId,
                                    EmailConfirmed,
                                    PhoneNumberConfirmed,
                                    TwoFactorEnabled,
                                    LockoutEnabled,
                                    AccessFailedCount
                                )
                                values (
                                    @UserId,
                                    @MakeFirstNamePublic,
                                    @MakeLastNamePublic,
                                    @MakeBirthDatePublic,
                                    @NickName,
                                    @FirstName,
                                    @LastName,
                                    @BirthDate,
                                    @School,
                                    @Job,
                                    @About,
                                    @GenderId,
                                    0,
                                    0,
                                    0,
                                    0,
                                    0
                                );"
                            , new
                            {
                                UserId,
                                MakeFirstNamePublic,
                                MakeLastNamePublic,
                                MakeBirthDatePublic,
                                NickName,
                                FirstName,
                                LastName,
                                BirthDate,
                                School,
                                Job,
                                About,
                                GenderId
                            });

                        return user;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}