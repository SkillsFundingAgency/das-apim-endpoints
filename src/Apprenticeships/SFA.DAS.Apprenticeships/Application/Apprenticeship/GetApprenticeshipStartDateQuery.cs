using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Apprenticeships.Application.Apprenticeship;

public class GetApprenticeshipStartDateQuery : IRequest<ApprenticeshipStartDateResponse?>
{
	public Guid ApprenticeshipKey { get; set; }

	public GetApprenticeshipStartDateQuery(Guid apprenticeshipKey)
	{
		ApprenticeshipKey = apprenticeshipKey;
	}
}

public class GetApprenticeshipStartDateQueryHandler : IRequestHandler<GetApprenticeshipStartDateQuery, ApprenticeshipStartDateResponse?>
{
	private readonly ILogger<GetApprenticeshipStartDateQueryHandler> _logger;
	private readonly IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> _apprenticeshipsApiClient;
	private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiCommitmentsClient;
	private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;

    public GetApprenticeshipStartDateQueryHandler(
		ILogger<GetApprenticeshipStartDateQueryHandler> logger,
		IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration> apprenticeshipsApiClient,
		ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiCommitmentsClient,
		ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient)
	{
		_logger = logger;
		_apprenticeshipsApiClient = apprenticeshipsApiClient;
		_apiCommitmentsClient = apiCommitmentsClient;
		_collectionCalendarApiClient = collectionCalendarApiClient;
    }

	public async Task<ApprenticeshipStartDateResponse?> Handle(GetApprenticeshipStartDateQuery request, CancellationToken cancellationToken)
	{
		var apprenticeStartDateInnerModel = await _apprenticeshipsApiClient.Get<GetApprenticeshipStartDateResponse>(new GetApprenticeshipStartDateRequest { ApprenticeshipKey = request.ApprenticeshipKey });

		if (apprenticeStartDateInnerModel == null)
		{
			_logger.LogWarning($"No ApprenticeshipStartDate returned from innerApi for apprenticeshipKey:{request.ApprenticeshipKey}");
			return null; 
		}

        var standard = await _apiCommitmentsClient.Get<GetTrainingProgrammeVersionsResponse>(new GetTrainingProgrammeVersionsRequest(apprenticeStartDateInnerModel.CourseCode.ToString()));

        string? employerName = await GetEmployerName(apprenticeStartDateInnerModel);

		var providerName = await GetProviderName(apprenticeStartDateInnerModel);

		var apprenticeshipStartDateOuterModel = new ApprenticeshipStartDateResponse
		{
			ApprenticeshipKey = apprenticeStartDateInnerModel!.ApprenticeshipKey,
			ActualStartDate = apprenticeStartDateInnerModel.ActualStartDate,
			PlannedEndDate = apprenticeStartDateInnerModel.PlannedEndDate,
			EmployerName = employerName,
			ProviderName = providerName,
			Standard = ToStandardInfo(standard)
		};

		return apprenticeshipStartDateOuterModel;
	}

	private async Task<string?> GetEmployerName(GetApprenticeshipStartDateResponse apprenticeStartDateInnerModel)
	{
		if (!apprenticeStartDateInnerModel.AccountLegalEntityId.HasValue)
		{
			_logger.LogError($"No AccountLegalEntityId returned from innerApi for apprenticeshipKey:{apprenticeStartDateInnerModel.ApprenticeshipKey}");
			return null;
		}

		var employer = await _apiCommitmentsClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(apprenticeStartDateInnerModel.AccountLegalEntityId.Value));
		
		if(employer == null)
		{
			_logger.LogError($"No AccountLegalEntity returned from innerApi for apprenticeshipKey:{apprenticeStartDateInnerModel.ApprenticeshipKey}");
			return null;
		}

		return employer.LegalEntityName;
	}

	private async Task<string?> GetProviderName(GetApprenticeshipStartDateResponse apprenticeStartDateInnerModel)
	{
		if (apprenticeStartDateInnerModel.UKPRN < 1)
		{
			_logger.LogError($"Invalid UKPRN '{apprenticeStartDateInnerModel.UKPRN}' returned from innerApi for apprenticeshipKey:{apprenticeStartDateInnerModel.ApprenticeshipKey}");
			return null;
		}

		var provider = await _apiCommitmentsClient.Get<GetProviderResponse>(new GetProviderRequest(apprenticeStartDateInnerModel.UKPRN));

		if (provider == null)
		{
			_logger.LogError($"No provider returned from CommitmentsClient for UKPRN:{apprenticeStartDateInnerModel.UKPRN}");
			return null;
		}

		return provider.Name;
	}

    private static StandardInfo ToStandardInfo(GetTrainingProgrammeVersionsResponse response)
    {
        return new StandardInfo
        {
			 CourseCode = response.TrainingProgrammeVersions.MaxBy(x => x.Version)?.CourseCode,
			 EffectiveFrom = response.TrainingProgrammeVersions.MaxBy(x => x.Version)?.EffectiveFrom,
             EffectiveTo = response.TrainingProgrammeVersions.MaxBy(x => x.Version)?.EffectiveTo,
			 Versions = response.TrainingProgrammeVersions.Select(x => new StandardVersionInfo
             {
                 EffectiveFrom = x.EffectiveFrom,
                 EffectiveTo = x.EffectiveTo,
                 Version = x.Version
             }).ToList()
        };
    }

}