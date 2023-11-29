using Microsoft.AspNetCore.Mvc;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;

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
        public async Task<ActionResult> CreateApprenticeshipPriceChange(
            long? providerId,
            long? employerId,
            Guid apprenticeshipKey,
            string userId,
            decimal? trainingPrice,
            decimal? assessmentPrice,
            decimal? totalPrice,
            string reason)
        {
            return Ok(await _apiClient.PostWithResponseCode<object>(new CreateApprenticeshipPriceChangeRequest{ ApprenticeshipKey = apprenticeshipKey, Data = new CreateApprenticeshipPriceChangeRequestData
            {
                Ukprn = providerId,
                EmployerId = employerId,
                UserId = userId,
                TrainingPrice = trainingPrice,
                AssessmentPrice = assessmentPrice,
                TotalPrice = totalPrice,
                Reason = reason
            } }));
        }
    }
}
