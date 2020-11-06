namespace SeekQ.Identity.Application.Profile.Queries
{
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

    public class GetLanguageKnowQueryHandler
    {
        public class Query : IRequest<IEnumerable<LanguageKnowViewModel>>
        {
            
        }

        public class Handler : IRequestHandler<Query, IEnumerable<LanguageKnowViewModel>>
        {
            private CommonGlobalAppSingleSettings _commonGlobalAppSingleSettings;

            public Handler(CommonGlobalAppSingleSettings commonGlobalAppSingleSettings)
            {
                _commonGlobalAppSingleSettings = commonGlobalAppSingleSettings;
            }

            public async Task<IEnumerable<LanguageKnowViewModel>> Handle(
                Query query,
                CancellationToken cancellationToken)
            {
                using (IDbConnection conn = new SqlConnection(_commonGlobalAppSingleSettings.MssqlConnectionString))
                {
                    string sql =
                        @"
                        SELECT  Id as LanguageKnowId,
                                Name as LaguageKnowName
                        FROM LanguageKnow";

                    var result = await conn.QueryAsync<LanguageKnowViewModel>(sql);

                    return result.AsEnumerable();
                }
            }
        }
    }
}
