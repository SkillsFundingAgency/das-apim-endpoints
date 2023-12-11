using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Models;
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
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiCommitmentsClient;

        public ApprenticeshipController(IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apiClient, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiCommitmentsClient)
        {
            _apiClient = apiClient;
            _apiCommitmentsClient = apiCommitmentsClient;
        }

        [HttpGet]
        [Route("{apprenticeshipKey}/price")]
        public async Task<ActionResult> GetApprenticeshipPrice(Guid apprenticeshipKey)
        {
            var apprenticePriceInnerModel = await _apiClient.Get<GetApprenticeshipPriceResponse>(new GetApprenticeshipPriceRequest { ApprenticeshipKey = apprenticeshipKey });
            
            if(apprenticePriceInnerModel == null)
            {
                return NotFound();
            }

            string? employerName = null;
            if(apprenticePriceInnerModel != null && apprenticePriceInnerModel.AccountLegalEntityId.HasValue)
            {
                var employer = await _apiCommitmentsClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(apprenticePriceInnerModel.AccountLegalEntityId.Value));
                employerName = employer?.LegalEntityName;
            }
            
            var apprenticeshipPriceOuterModel = new ApprenticeshipPriceResponse
            {
                ApprenticeshipKey = apprenticePriceInnerModel!.ApprenticeshipKey,
                ApprenticeshipActualStartDate = apprenticePriceInnerModel.ApprenticeshipActualStartDate,
                ApprenticeshipPlannedEndDate = apprenticePriceInnerModel.ApprenticeshipPlannedEndDate,
                AssessmentPrice = apprenticePriceInnerModel.AssessmentPrice,
                EarliestEffectiveDate = apprenticePriceInnerModel.EarliestEffectiveDate,
                FundingBandMaximum = apprenticePriceInnerModel.FundingBandMaximum,
                TrainingPrice = apprenticePriceInnerModel.TrainingPrice,
                EmployerName = employerName
            };

            return Ok(apprenticeshipPriceOuterModel);
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
