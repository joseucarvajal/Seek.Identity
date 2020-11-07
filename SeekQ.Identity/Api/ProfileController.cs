using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeekQ.Identity.Application.Profile.Commands;
using SeekQ.Identity.Application.Profile.Queries;
using SeekQ.Identity.Application.Profile.ViewModels;
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
    public class ProfileController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public ProfileController(
            IMediator mediator
        )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));            
        }

        [HttpPost]
        [Route("set/password")]
        [SwaggerOperation(Summary = "add password to an existing user")]
        public async Task<ActionResult<ApplicationUser>> SetUserPassword(
            [FromBody] SetUserPasswordCommandHandler.Command command
        )
        {
            return await _mediator.Send(command);
        }

        // POST api/v1/profile
        [HttpPost]
        [SwaggerOperation(Summary = "create new user")]
        [SwaggerResponse((int)HttpStatusCode.OK, "user created succesfully")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request")]
        public async Task<ActionResult<ApplicationUser>> CreateUser(
            [FromBody] CreateUserCommandHandler.Command command
        )
        {
            return await _mediator.Send(command);
        }

        // PUT api/v1/profile
        [HttpPut]
        [SwaggerOperation(Summary = "update an existing user")]
        [SwaggerResponse((int)HttpStatusCode.OK, "user updated succesfully")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request")]
        public async Task<ActionResult<ApplicationUser>> UpdateUser(
            [FromBody] UpdateUserCommandHandler.Command command
        )
        {
            return await _mediator.Send(command);
        }

        // GET api/v1/profile/{Id}
        [HttpGet("{idUser}")]
        [SwaggerOperation(Summary = "get an existing user")]
        public async Task<IEnumerable<UserViewModel>> GetUser(
            [SwaggerParameter(Description = "UserId is a Guid Type")]
            [FromRoute]
            Guid idUser
        )
        {
            return await _mediator.Send(new GetUserQueryHandler.Query(idUser));
        }

        // DELETE api/v1/profile/{userId}/language/{languageId}
        [HttpDelete("{idUser}")]
        [Route("language/{languageId}")]
        [SwaggerOperation(Summary = "delete user languages")]
        public async Task<bool> DeleteUserLanguage([FromRoute] Guid idUser, [FromRoute] int languageId)
        {
            return await _mediator.Send(new DeleteUserLanguageCommandHandler.Command(idUser, languageId));
        }
    }
}
