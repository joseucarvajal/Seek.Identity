﻿namespace SeekQ.Identity.Application.Profile.Profile.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.SeedWork;
    using Dapper;
    using MediatR;
    using Microsoft.Data.SqlClient;
    using System.Linq;
    using ViewModel;
    using App.Common.Exceptions;

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
                    try
                    {
                        string sql =
                        @"
                            SELECT u.Id as IdUser,
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
                            ug.Name as GenderName

                        FROM AspNetUsers u
                            LEFT JOIN UserGenders ug ON u.GenderId = ug.Id

                        WHERE u.Id = @IdUser";

                        var result = await conn.QueryAsync<UserViewModel>(sql, new { IdUser = query.IdUser });

                        return result.AsEnumerable();
                    }
                    catch (Exception e)
                    {
                        throw new AppException(e.Message);
                    }
                }
            }
        }
    }
}