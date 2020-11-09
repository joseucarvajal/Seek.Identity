namespace SeekQ.Identity.Api.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Profile.Gender.Queries;
    using Application.Profile.Gender.ViewModel;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

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