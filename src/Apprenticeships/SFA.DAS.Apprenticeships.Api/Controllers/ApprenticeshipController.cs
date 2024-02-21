﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.Application.Apprenticeship;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using CreateApprenticeshipPriceChangeRequest = SFA.DAS.Apprenticeships.Api.Models.CreateApprenticeshipPriceChangeRequest;

namespace SFA.DAS.Apprenticeships.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApprenticeshipController : ControllerBase
    {
        private readonly IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> _apiClient;
		private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeshipController> _logger;

		public ApprenticeshipController(
			ILogger<ApprenticeshipController> logger,
			IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apiClient,
			IMediator mediator)
        {
            _logger = logger;
            _apiClient = apiClient;
            _mediator = mediator;
		}

        [HttpGet]
        [Route("{apprenticeshipKey}/price")]
        public async Task<ActionResult> GetApprenticeshipPrice(Guid apprenticeshipKey)
        {
			try
			{
				var apprenticeshipPriceResponse = await _mediator.Send(new GetApprenticeshipPriceQuery(apprenticeshipKey));

				if (apprenticeshipPriceResponse == null)
				{
					return NotFound();
				}

				return Ok(apprenticeshipPriceResponse);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Error attempting to get ApprenticeshipPrice");
				return BadRequest();
			}
        }

        [HttpGet]
        [Route("{apprenticeshipHashedId}/key")]
        public async Task<ActionResult> GetApprenticeshipKey(string apprenticeshipHashedId)
        {
            return Ok(await _apiClient.Get<Guid>(new GetApprenticeshipKeyRequest { ApprenticeshipHashedId = apprenticeshipHashedId }));
        }
        
        [HttpPost]
        [Route("{apprenticeshipKey}/priceHistory")]
        public async Task<ActionResult> CreateApprenticeshipPriceChange(Guid apprenticeshipKey,
            [FromBody] CreateApprenticeshipPriceChangeRequest request)
        {
            await _apiClient.PostWithResponseCode<object>(new PostCreateApprenticeshipPriceChangeRequest(
                apprenticeshipKey,
                request.ProviderId,
                request.EmployerId,
                request.UserId,
                request.TrainingPrice,
                request.AssessmentPrice,
                request.TotalPrice,
                request.Reason,
                request.EffectiveFromDate
            ), false);
            return Ok();
        }

        [HttpGet]
        [Route("{apprenticeshipKey}/priceHistory/pending")]
        public async Task<ActionResult> GetPendingPriceChange(Guid apprenticeshipKey)
        {
	        var response = await _apiClient.Get<GetPendingPriceChangeApiResponse>(new GetPendingPriceChangeRequest(apprenticeshipKey));
	        return Ok(new GetPendingPriceChangeResponse(response));
        }

        [HttpDelete]
        [Route("{apprenticeshipKey}/priceHistory/pending")]
        public async Task<ActionResult> CancelPendingPriceChange(Guid apprenticeshipKey)
        {
            await _apiClient.Delete(new CancelPendingPriceChangeRequest(apprenticeshipKey));
            return Ok();
        }
    }
}
