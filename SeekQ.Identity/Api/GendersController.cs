using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeekQ.Identity.Application.Gender.Queries;
using SeekQ.Identity.Application.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace SeekQ.Identity.Api
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GendersController : Controller
    {
        private readonly IMediator _mediator;

        public GendersController(
            IMediator mediator
        )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET api/v1/genders
        [HttpGet]
        [SwaggerOperation(Summary = "get all available genders")]
        public async Task<IEnumerable<UserGenderViewModel>> GetAllGenders()
        {
            return await _mediator.Send(new GetAllGendersQueryHandler.Query());
        }
    }
}