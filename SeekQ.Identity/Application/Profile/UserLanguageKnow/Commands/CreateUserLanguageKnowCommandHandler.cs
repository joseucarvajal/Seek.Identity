namespace SeekQ.Identity.Application.Profile.UserLanguageKnow.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.Exceptions;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using SeekQ.Identity.Data;
    using SeekQ.Identity.Models.Profile;

    public class CreateUserLanguageKnowCommandHandler
    {
        public class Command : IRequest<UserLanguageKnow>
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

        public class Handler : IRequestHandler<Command, UserLanguageKnow>
        {
            private ApplicationDbContext _applicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _applicationDbContext = applicationDbContext;
            }

            public async Task<UserLanguageKnow> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    string ApplicationUserId = request.ApplicationUserId;
                    int LanguageKnowId = request.LanguageKnowId;

                    var existingUserLanguageKnow = await _applicationDbContext.UserLanguageKnows
                                                    .AsNoTracking()
                                                    .SingleOrDefaultAsync(u => u.ApplicationUserId == ApplicationUserId && u.LanguageKnowId == LanguageKnowId);

                    if (existingUserLanguageKnow != null)
                    {
                        throw new AppException($"The ApplicationUserId {ApplicationUserId} and LanguageKnowId {LanguageKnowId} already exist");
                    }

                    UserLanguageKnow userLanguageKnow = new UserLanguageKnow()
                    {
                        Id = Guid.NewGuid(),
                        ApplicationUserId = ApplicationUserId,
                        LanguageKnowId = LanguageKnowId
                    };

                    _applicationDbContext.UserLanguageKnows.Add(userLanguageKnow);
                    await _applicationDbContext.SaveChangesAsync();

                    return userLanguageKnow;
                }
                catch (Exception e)
                {
                    throw new AppException(e.Message);
                }
            }
        }
    }
}