using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.RenewSubscriptionKey;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProducts;
using SFA.DAS.SharedOuterApi.Infrastructure;

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
        [Route("products/{accountIdentifier}")]
        public async Task<IActionResult> GetAvailableProducts([FromRoute]string accountIdentifier, string accountType)
        {
            try
            {
                var result = await _mediator.Send(new GetApiProductsQuery 
                { 
                    AccountType = accountType, 
                    AccountIdentifier = accountIdentifier
                });
                return Ok((ProductsApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get API products");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpPost]
        [Route("{id}/renew/{productId}")]
        public async Task<IActionResult> RenewSubscriptionKey([FromRoute]string id, [FromRoute]string productId)
        {
            try
            {
                await _mediator.Send(new RenewSubscriptionKeyCommand
                {
                    AccountIdentifier = id,
                    ProductId = productId
                });

                return NoContent();
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}