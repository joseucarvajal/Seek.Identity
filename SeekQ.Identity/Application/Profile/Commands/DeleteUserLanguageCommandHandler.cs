namespace SeekQ.Identity.Application.Profile.Commands
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

    public class DeleteUserLanguageCommandHandler
    {
        public class Command : IRequest<bool>
        {
            public Guid ApplicationUserId { get; set; }
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
            private ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var obj = await _dbContext.UserLanguageKnows.FindAsync(request.ApplicationUserId, request.LanguageKnowId);

                if (obj == null)
                {
                    throw new AppException("User Language does not exists.");
                }

                _dbContext.UserLanguageKnows.Remove(obj);

                return await _dbContext.SaveChangesAsync() > 0;
            }
        }
    }
}