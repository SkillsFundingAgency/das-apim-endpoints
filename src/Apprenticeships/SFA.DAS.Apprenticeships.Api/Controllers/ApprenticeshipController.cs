using Microsoft.AspNetCore.Mvc;
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

        public ApprenticeshipController(IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        [Route("{apprenticeshipKey}/price")]
        public async Task<ActionResult> GetApprenticeshipPrice(Guid apprenticeshipKey)
        {
            return Ok(await _apiClient.Get<GetApprenticeshipPriceResponse>(new GetApprenticeshipPriceRequest{ ApprenticeshipKey = apprenticeshipKey }));
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
    }
}
