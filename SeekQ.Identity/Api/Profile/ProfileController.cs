﻿namespace SeekQ.Identity.Api.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Application.Profile.Profile.Commands;
    using Application.Profile.Profile.Queries;
    using Application.Profile.Profile.ViewModel;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using SeekQ.Identity.Models;
    using Swashbuckle.AspNetCore.Annotations;

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

        // POST api/v1/profile
        [HttpPost]
        [Route("set/password")]
        [SwaggerOperation(Summary = "add password to an existing user")]
        public async Task<ActionResult<ApplicationUser>> SetUserPassword(
            [FromBody] SetUserPasswordCommandHandler.Command command
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
    }
}