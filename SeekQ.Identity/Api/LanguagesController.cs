using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeekQ.Identity.Application.Language.Queries;
using SeekQ.Identity.Application.ViewModels;
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
        [SwaggerOperation(Summary = "get all available languages")]
        public async Task<IEnumerable<LanguageKnowViewModel>> GetAllLanguages()
        {
            return await _mediator.Send(new GetAllLanguagesQueryHandler.Query());
        }
    }
}