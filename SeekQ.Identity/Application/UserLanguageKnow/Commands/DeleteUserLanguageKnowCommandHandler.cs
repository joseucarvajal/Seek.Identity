namespace SeekQ.Identity.Application.UserLanguageKnow.Commands
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.Exceptions;
    using App.Common.SeedWork;
    using Dapper;
    using Dapper.Contrib.Extensions;
    using FluentValidation;
    using MediatR;
    using Microsoft.Data.SqlClient;
    using SeekQ.Identity.Application.ViewModels;
    using SeekQ.Identity.Models;

    public class DeleteUserLanguageKnowCommandHandler
    {
        public class Command : IRequest<bool>
        {
            public Command(string userId, int languageKnowId)
            {
                ApplicationUserId = userId;
                LanguageKnowId = languageKnowId;
            }

            public string ApplicationUserId { get; set; }
            public int LanguageKnowId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private CommonGlobalAppSingleSettings _commonGlobalAppSingleSettings;

            public Handler(CommonGlobalAppSingleSettings commonGlobalAppSingleSettings)
            {
                _commonGlobalAppSingleSettings = commonGlobalAppSingleSettings;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    string ApplicationUserId = request.ApplicationUserId;
                    int LanguageKnowId = request.LanguageKnowId;

                    using (IDbConnection conn = new SqlConnection(_commonGlobalAppSingleSettings.MssqlConnectionString))
                    {
                        string sql = @"SELECT ulk.Id
                                        FROM UserLanguageKnows ulk
                                                INNER JOIN LanguageKnows lk ON ulk.LanguageKnowId = lk.Id
                                        WHERE ApplicationUserId = @ApplicationUserId
                                        AND   LanguageKnowId = @LanguageKnowId";
                        var result = await conn.QueryAsync<Guid>(sql, new { ApplicationUserId, LanguageKnowId });

                        if (result.ToList().Count == 0) return false;

                        var isSuccess = conn.Delete(new UserLanguageKnow { Id = result.FirstOrDefault() });
                        return isSuccess;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}