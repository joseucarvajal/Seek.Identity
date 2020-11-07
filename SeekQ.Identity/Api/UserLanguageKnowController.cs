using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeekQ.Identity.Application.UserLanguageKnow.Commands;
using SeekQ.Identity.Application.UserLanguageKnow.Queries;
using SeekQ.Identity.Application.ViewModels;
using SeekQ.Identity.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SeekQ.Identity.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserLanguageKnowController : Controller
    {
        private readonly IMediator _mediator;

        public UserLanguageKnowController(
            IMediator mediator
        )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET api/v1/userlanguage/{Id}
        [HttpGet("{idUser}")]
        [SwaggerOperation(Summary = "get all know languages user")]
        public async Task<IEnumerable<UserLanguageViewModel>> GetUser(
            [SwaggerParameter(Description = "UserId is a Guid Type")]
            [FromRoute]
            Guid idUser
        )
        {
            return await _mediator.Send(new GetUserLanguageKnowCommandHandler.Query(idUser));
        }

        // POST api/v1/userlanguage
        [HttpPost]
        [SwaggerOperation(Summary = "add new language to a user")]
        [SwaggerResponse((int)HttpStatusCode.OK, "user language created succesfully")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request")]
        public async Task<ActionResult<UserLanguageKnow>> CreateUserLanguage(
            [FromBody] CreateUserLanguageKnowCommandHandler.Command command
        )
        {
            return await _mediator.Send(command);
        }

        // DELETE api/v1/userlanguage/{userId}/language/{languageId}
        [HttpDelete("{userId}/language/{languageId}")]
        [SwaggerOperation(Summary = "delete language to a user")]
        public async Task<bool> DeleteUserLanguage([FromRoute] Guid idUser, [FromRoute] int languageId)
        {
            return await _mediator.Send(new DeleteUserLanguageKnowCommandHandler.Command(idUser, languageId));
        }
    }
}