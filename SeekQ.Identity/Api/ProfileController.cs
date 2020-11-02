﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeekQ.Identity.Application.Profile.Commands;
using SeekQ.Identity.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
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

    }
}
