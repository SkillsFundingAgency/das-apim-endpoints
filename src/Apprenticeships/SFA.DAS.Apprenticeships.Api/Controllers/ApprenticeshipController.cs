using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using CreateApprenticeshipPriceChangeRequest = SFA.DAS.Apprenticeships.Api.Models.CreateApprenticeshipPriceChangeRequest;
using CreateApprenticeshipStartDateChangeRequest = SFA.DAS.Apprenticeships.Api.Models.CreateApprenticeshipStartDateChangeRequest;
using GetProviderResponse = SFA.DAS.Apprenticeships.Api.Models.GetProviderResponse;

namespace SFA.DAS.Apprenticeships.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApprenticeshipController : ControllerBase
{
    private readonly ILearningApiClient<LearningApiConfiguration> _apiClient;
    private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiCommitmentsClient;
    private readonly IMediator _mediator;
    private readonly ILogger<ApprenticeshipController> _logger;

    public ApprenticeshipController(
        ILogger<ApprenticeshipController> logger,
        ILearningApiClient<LearningApiConfiguration> apiClient,
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiCommitmentsClient,
        IMediator mediator)
    {
        _logger = logger;
        _apiClient = apiClient;
        _apiCommitmentsClient = apiCommitmentsClient;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{apprenticeshipHashedId}/key")]
    public async Task<ActionResult> GetApprenticeshipKey(string apprenticeshipHashedId)
    {
        return Ok(await _apiClient.Get<Guid>(new GetApprenticeshipKeyRequest { ApprenticeshipHashedId = apprenticeshipHashedId }));
    }


}