using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries;

namespace SFA.DAS.ApimDeveloper.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SubscriptionsController> _logger;

        public SubscriptionsController (IMediator mediator, ILogger<SubscriptionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> GetAvailableProducts(string accountType)
        {
            try
            {
                var result = await _mediator.Send(new GetApiProductsQuery { AccountType = accountType });
                return Ok((ProductsApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get API products");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}