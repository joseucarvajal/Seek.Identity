namespace SeekQ.Identity.Application.Language.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.SeedWork;
    using Dapper;
    using MediatR;
    using Microsoft.Data.SqlClient;
    using ViewModels;

    public class GetAllLanguagesQueryHandler
    {
        public class Query : IRequest<IEnumerable<LanguageViewModel>>
        {

        }

        public class Handler : IRequestHandler<Query, IEnumerable<LanguageViewModel>>
        {
            private CommonGlobalAppSingleSettings _commonGlobalAppSingleSettings;

            public Handler(CommonGlobalAppSingleSettings commonGlobalAppSingleSettings)
            {
                _commonGlobalAppSingleSettings = commonGlobalAppSingleSettings;
            }

            public async Task<IEnumerable<LanguageViewModel>> Handle(
                Query query,
                CancellationToken cancellationToken)
            {
                try
                {
                    using (IDbConnection conn = new SqlConnection(_commonGlobalAppSingleSettings.MssqlConnectionString))
                    {
                        string sql =
                            @"
                        SELECT  Id as LanguageId,
                                Name as LanguageName
                        FROM LanguageKnows";

                        var result = await conn.QueryAsync<LanguageViewModel>(sql);

                        return result.AsEnumerable();
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
