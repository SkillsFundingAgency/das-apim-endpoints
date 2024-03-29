using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AparRegister.Api.ApiResponses;
using SFA.DAS.AparRegister.Application.ProviderRegister.Queries;

namespace SFA.DAS.AparRegister.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ProvidersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProvidersController(IMediator mediator)
        {
            _mediator = mediator;
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
        [ProducesResponseType(typeof(ProvidersApiResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetProviders()
        {
            try
            {
                var result = await _mediator.Send(new GetProvidersQuery());
                var providersApiResponse = (ProvidersApiResponse)result;
                return Ok(providersApiResponse);
            }
            catch (Exception e)
            {
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}