using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeekQ.Identity.Application.Profile.Commands;
using SeekQ.Identity.Application.Profile.Queries;
using SeekQ.Identity.Application.Profile.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace SeekQ.Identity.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LanguagesController : Controller
    {
        private readonly IMediator _mediator;

        public LanguagesController(
            IMediator mediator
        )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET api/v1/languages
        [HttpGet]
        [SwaggerOperation(Summary = "get all user languages")]
        public async Task<IEnumerable<LanguageKnowViewModel>> GetAllLanguages()
        {
            return await _mediator.Send(new GetLanguageKnowQueryHandler.Query());
        }

        // DELETE api/v1/user/{5a3e8e31-b2fb-43a7-b275-7464d2931f0e}/language/{2}
        [Route("user/{userId}/language/{languageId}")]
        [SwaggerOperation(Summary = "delete user languages")]
        public async Task<bool> DeleteUserLanguage([FromRoute] Guid userId, [FromRoute] int languageId)
        {
            return await _mediator.Send(new DeleteUserLanguageCommandHandler.Command(userId, languageId));
        }
    }
}