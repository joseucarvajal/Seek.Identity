namespace SeekQ.Identity.Api.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Profile.Language.Queries;
    using Application.Profile.Language.ViewModel;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "get all available languages")]
        public async Task<IEnumerable<LanguageViewModel>> GetAllLanguages()
        {
            return await _mediator.Send(new GetAllLanguagesQueryHandler.Query());
        }
    }
}