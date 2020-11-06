namespace SeekQ.Identity.Application.Profile.Queries
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

    public class GetUserQueryHandler
    {
        public class Query : IRequest<IEnumerable<UserViewModel>>
        {
            public Query(Guid idUser)
            {
                IdUser = idUser;
            }

            public Guid IdUser { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<UserViewModel>>
        {
            private CommonGlobalAppSingleSettings _commonGlobalAppSingleSettings;

            public Handler(CommonGlobalAppSingleSettings commonGlobalAppSingleSettings)
            {
                _commonGlobalAppSingleSettings = commonGlobalAppSingleSettings;
            }

            public async Task<IEnumerable<UserViewModel>> Handle(
                Query query,
                CancellationToken cancellationToken)
            {
                using (IDbConnection conn = new SqlConnection(_commonGlobalAppSingleSettings.MssqlConnectionString))
                {
                    string sql =
                        @"
                        SELECT  Id as IdUser,
                                MakeFirstNamePublic,
                                MakeLastNamePublic,
                                MakeBirthDatePublic,
                                NickName,
                                FirstName,
                                LastName,
                                Email,
                                PhoneNumber,
                                BirthDate,
                                School,
                                Job,
                                About,
                                GenderId,
                                ug.Name as GenderName,
                                lk.LanguageKnowId,
                                lk.Name as LanguageKnowName

                        FROM AspNetUsers u
                            INNER JOIN UserGender ug ON u.GenderId = ug.Id
                            INNER JOIN UserLanguageKnow ulk ON ulk.ApplicationUserId = u.Id
                            INNER JOIN LanguageKnow lk ON ulk.LanguageKnowId = lk.Id

		                WHERE Id = @IdUser";

                    var result = await conn.QueryAsync<UserViewModel>(sql, new { query.IdUser });

                    return result.AsEnumerable();
                }
            }
        }
    }
}