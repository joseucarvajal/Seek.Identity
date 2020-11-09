namespace SeekQ.Identity.Application.UserLanguageKnow.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.SeedWork;
    using Dapper;
    using MediatR;
    using ViewModels;
    using Microsoft.Data.SqlClient;
    using System.Linq;

    public class GetUserLanguageKnowCommandHandler
    {
        public class Query : IRequest<IEnumerable<UserLanguageKnowViewModel>>
        {
            public Query(Guid userId)
            {
                UserId = userId;
            }

            public Guid UserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<UserLanguageKnowViewModel>>
        {
            private CommonGlobalAppSingleSettings _commonGlobalAppSingleSettings;

            public Handler(CommonGlobalAppSingleSettings commonGlobalAppSingleSettings)
            {
                _commonGlobalAppSingleSettings = commonGlobalAppSingleSettings;
            }

            public async Task<IEnumerable<UserLanguageKnowViewModel>> Handle(
                Query query,
                CancellationToken cancellationToken)
            {
                using (IDbConnection conn = new SqlConnection(_commonGlobalAppSingleSettings.MssqlConnectionString))
                {
                    try
                    {
                        string sql =
                        @"
                            SELECT  lk.Id as LanguageKnowId,
                                    lk.Name as LanguageKnowName

                        FROM AspNetUsers u
                            INNER JOIN UserLanguageKnows ulk ON ulk.ApplicationUserId = u.Id
                            INNER JOIN LanguageKnows lk ON ulk.LanguageKnowId = lk.Id

                        WHERE u.Id = @IdUser";

                        var result = await conn.QueryAsync<UserLanguageKnowViewModel>(sql, new { IdUser = query.UserId });

                        return result.AsEnumerable();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }
    }
}