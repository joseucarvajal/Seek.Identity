namespace SeekQ.Identity.Application.Profile.Gender.Queries
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
    using ViewModel;

    public class GetAllGendersQueryHandler
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
                try
                {
                    using (IDbConnection conn = new SqlConnection(_commonGlobalAppSingleSettings.MssqlConnectionString))
                    {
                        string sql =
                            @"
                        SELECT  Id as GenderId,
                                Name as GenderName
                        FROM UserGenders";

                        var result = await conn.QueryAsync<UserGenderViewModel>(sql);

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
