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
            private ApplicationDbContext _applicationDbContext;

            public Handler(ApplicationDbContext applicationDbContext)
            {
                _applicationDbContext = applicationDbContext;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    string ApplicationUserId = request.ApplicationUserId;
                    int LanguageKnowId = request.LanguageKnowId;

                    var existingUserLanguageKnow = await _applicationDbContext.UserLanguageKnows
                                                    .AsNoTracking()
                                                    .SingleOrDefaultAsync(u => u.ApplicationUserId == ApplicationUserId && u.LanguageKnowId == LanguageKnowId);

                    if (existingUserLanguageKnow == null)
                    {
                        throw new AppException($"The UserId {ApplicationUserId} doesn't have the LanguageKnowId {LanguageKnowId}.");
                    }

                    _applicationDbContext.UserLanguageKnows.Remove(existingUserLanguageKnow);
                    await _applicationDbContext.SaveChangesAsync();

                    return true;
                }
                catch (Exception e)
                {
                    throw new AppException(e.Message);
                }
            }
        }
    }
}