using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.AparRegister.Api.ApiResponses;
using SFA.DAS.AparRegister.Application.ProviderRegister.Queries;

namespace SFA.DAS.AparRegister.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProvidersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProvidersController> _logger;

        public ProvidersController(IMediator mediator, ILogger<ProvidersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Gets list of providers
        /// </summary>
        /// <remarks>
        /// Returns full list of providers of all statuses
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(ProvidersApiResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProviders()
        {
            _logger.LogInformation("Getting Providers");
            try
            {
                var result = await _mediator.Send(new GetProvidersQuery());
                var providersApiResponse = (ProvidersApiResponse)result;
                return Ok(providersApiResponse);
            }
            catch (Exception e)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}