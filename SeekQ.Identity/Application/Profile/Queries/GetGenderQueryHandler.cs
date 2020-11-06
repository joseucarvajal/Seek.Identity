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

    public class GetGenderQueryHandler
    {
        public class Query : IRequest<IEnumerable<UserGenderViewModel>>
        {
            
        }

        public class Handler : IRequestHandler<Query, IEnumerable<UserGenderViewModel>>
        {
            private CommonGlobalAppSingleSettings _commonGlobalAppSingleSettings;

            public Handler(CommonGlobalAppSingleSettings commonGlobalAppSingleSettings)
            {
                _commonGlobalAppSingleSettings = commonGlobalAppSingleSettings;
            }

            public async Task<IEnumerable<UserGenderViewModel>> Handle(
                Query query,
                CancellationToken cancellationToken)
            {
                using (IDbConnection conn = new SqlConnection(_commonGlobalAppSingleSettings.MssqlConnectionString))
                {
                    string sql =
                        @"
                        SELECT  Id as GenderId,
                                Name as GenderName
                        FROM UserGender";

                    var result = await conn.QueryAsync<UserGenderViewModel>(sql);

                    return result.AsEnumerable();
                }
            }
        }
    }
}
