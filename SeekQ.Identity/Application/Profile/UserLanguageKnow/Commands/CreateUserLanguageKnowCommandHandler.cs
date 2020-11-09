namespace SeekQ.Identity.Application.Profile.UserLanguageKnow.Commands
{
    using System;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.SeedWork;
    using Dapper;
    using FluentValidation;
    using MediatR;
    using Microsoft.Data.SqlClient;
    using ViewModel;

    public class CreateUserLanguageKnowCommandHandler
    {
        public class Command : IRequest<UserLanguageKnowViewModel>
        {
            public string ApplicationUserId { get; set; }
            public int LanguageKnowId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler : IRequestHandler<Command, UserLanguageKnowViewModel>
        {
            private CommonGlobalAppSingleSettings _commonGlobalAppSingleSettings;

            public Handler(CommonGlobalAppSingleSettings commonGlobalAppSingleSettings)
            {
                _commonGlobalAppSingleSettings = commonGlobalAppSingleSettings;
            }

            public async Task<UserLanguageKnowViewModel> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    string ApplicationUserId = request.ApplicationUserId;
                    int LanguageKnowId = request.LanguageKnowId;

                    using (IDbConnection conn = new SqlConnection(_commonGlobalAppSingleSettings.MssqlConnectionString))
                    {
                        UserLanguageKnowViewModel userLanguageKnow = new UserLanguageKnowViewModel
                        {
                            ApplicationUserId = ApplicationUserId,
                            LanguageKnowId = LanguageKnowId
                        };

                        string insert = @"Insert Into UserLanguageKnows (Id, ApplicationUserId, LanguageKnowId)
                                            Values (@Id, @ApplicationUserId, @LanguageKnowId);";
                        var result = await conn.ExecuteAsync(insert, new { Id = Guid.NewGuid(), ApplicationUserId, LanguageKnowId });

                        return userLanguageKnow;
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