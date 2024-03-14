using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.Application.Apprenticeship;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using CreateApprenticeshipPriceChangeRequest = SFA.DAS.Apprenticeships.Api.Models.CreateApprenticeshipPriceChangeRequest;
using GetProviderResponse = SFA.DAS.Apprenticeships.Api.Models.GetProviderResponse;

namespace SFA.DAS.Apprenticeships.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApprenticeshipController : ControllerBase
    {
        private readonly IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> _apiClient;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiCommitmentsClient;
        private readonly IMediator _mediator;
        private readonly ILogger<ApprenticeshipController> _logger;

		public ApprenticeshipController(
			ILogger<ApprenticeshipController> logger,
			IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apiClient,
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiCommitmentsClient,
            IMediator mediator)
        {
            _logger = logger;
            _apiClient = apiClient;
            _apiCommitmentsClient = apiCommitmentsClient;
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
            var response = await _apiClient.PostWithResponseCode<object>(new PostCreateApprenticeshipPriceChangeRequest(
            apprenticeshipKey,
            request.Requester,
				    request.UserId,
				    request.TrainingPrice,
				    request.AssessmentPrice,
				    request.TotalPrice,
				    request.Reason,
				    request.EffectiveFromDate), false);

			      if (string.IsNullOrEmpty(response.ErrorContent))
			      {
				        return Ok();
			      }
               
            _logger.LogError($"Error attempting to create apprenticeship price change. {response.StatusCode} returned from inner api.", response.StatusCode);
            return BadRequest();
		    }

        [HttpGet]
        [Route("{apprenticeshipKey}/priceHistory/pending")]
        public async Task<ActionResult> GetPendingPriceChange(Guid apprenticeshipKey)
        {
	          var response = await _apiClient.Get<GetPendingPriceChangeApiResponse>(new GetPendingPriceChangeRequest(apprenticeshipKey));

            if (response == null || response.PendingPriceChange == null)
            {
                _logger.LogWarning($"No pending price change found for apprenticeship {apprenticeshipKey}");
                return NotFound();
            }

            var ukprn = response.PendingPriceChange.Ukprn.GetValueOrDefault();
            var providerResponse = await _apiCommitmentsClient.Get<GetProviderResponse>(new GetProviderRequest(ukprn));

            if (providerResponse == null || string.IsNullOrEmpty(providerResponse.Name))
            {
                _logger.LogWarning($"No provider found for ukprn {ukprn}");
                return NotFound();
            }

            var accountLegalEntityId = response.PendingPriceChange.AccountLegalEntityId.GetValueOrDefault();
            var employerResponse = await _apiCommitmentsClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(accountLegalEntityId));
            if (employerResponse == null || string.IsNullOrEmpty(employerResponse.AccountName))
            {
                _logger.LogWarning($"No employer found for accountLegalEntityId {ukprn}");
                return NotFound();
            }

            return Ok(new GetPendingPriceChangeResponse(response, providerResponse.Name, apprenticeshipKey, employerResponse.AccountName));
        }

        [HttpDelete]
        [Route("{apprenticeshipKey}/priceHistory/pending")]
        public async Task<ActionResult> CancelPendingPriceChange(Guid apprenticeshipKey)
        {
            await _apiClient.Delete(new CancelPendingPriceChangeRequest(apprenticeshipKey));
            return Ok();
        }

        [HttpPatch]
        [Route("{apprenticeshipKey}/priceHistory/pending/reject")]
        public async Task<ActionResult> RejectPendingPriceChange(Guid apprenticeshipKey, [FromBody] RejectPriceChangeRequest request)
        {
            await _apiClient.Patch(new PatchRejectApprenticeshipPriceChangeRequest(apprenticeshipKey, request.Reason));
            return Ok();
        }

        [HttpPatch]
        [Route("{apprenticeshipKey}/priceHistory/pending/approve")]
        public async Task<ActionResult> ApprovePendingPriceChange(Guid apprenticeshipKey, [FromBody] ApprovePriceChangeRequest request)
        {
            await _apiClient.Patch(new PatchApproveApprenticeshipPriceChangeRequest(apprenticeshipKey, request.UserId));
            return Ok();
        }
    }
}
