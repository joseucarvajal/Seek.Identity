namespace SeekQ.Identity.Application.UserLanguageKnow.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Common.Exceptions;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using SeekQ.Identity.Data;
    using SeekQ.Identity.Models;
    using SeekQ.Identity.Application.ViewModels;

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
            private ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<UserLanguageKnow> Handle(Command request, CancellationToken cancellationToken)
            {
                string ApplicationUserId = request.ApplicationUserId;
                int LanguageKnowId = request.LanguageKnowId;

                var obj = await _dbContext.UserLanguageKnows.FindAsync(request.ApplicationUserId, request.LanguageKnowId);

                if (obj != null)
                {
                    throw new AppException("Language has been registered to user alredy.");
                }

                UserLanguageKnow userLanguage = new UserLanguageKnow();
                userLanguage.LanguageKnowId = LanguageKnowId;
                userLanguage.ApplicationUserId = ApplicationUserId;

                _dbContext.UserLanguageKnows.Add(userLanguage);
                await _dbContext.SaveChangesAsync();

                return userLanguage;
            }
        }
    }
}