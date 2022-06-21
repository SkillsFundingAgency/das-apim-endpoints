using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.CreateSubscriptionKey;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.RenewSubscriptionKey;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscription;
using SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Queries.GetApiProductSubscriptions;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
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
        [Route("{accountIdentifier}/products")]
        public async Task<IActionResult> GetAvailableProducts([FromRoute]string accountIdentifier, string accountType)
        {
            try
            {
                var result = await _mediator.Send(new GetApiProductSubscriptionsQuery 
                { 
                    AccountType = accountType, 
                    AccountIdentifier = accountIdentifier
                });
                return Ok((ProductSubscriptionsApiResponse)result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get API products");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("{id}/products/{productId}")]
        public async Task<IActionResult> GetProductSubscription([FromRoute] string id, [FromRoute] string productId, string accountType)
        {
            try
            {
                var result = await _mediator.Send(new GetApiProductSubscriptionQuery
                {
                    AccountIdentifier = id,
                    AccountType = accountType,
                    ProductId = productId
                });

                if (result.Product == null || result.Subscription == null)
                {
                    return NotFound();
                }
                
                var response = ProductSubscriptionApiResponseItem.Map(result.Product,
                    new List<GetApiProductSubscriptionsResponseItem> { result.Subscription });
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to get API product");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{id}/products/{productId}")]
        public async Task<IActionResult> CreateProductSubscription([FromRoute] string id, [FromRoute] string productId, string accountType)
        {
            try
            {
                await _mediator.Send(new CreateSubscriptionKeyCommand
                {
                    AccountIdentifier = id,
                    AccountType = accountType,
                    ProductId = productId
                });

                return Created($"{id}/products/{productId}?accountType={accountType}",null);
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