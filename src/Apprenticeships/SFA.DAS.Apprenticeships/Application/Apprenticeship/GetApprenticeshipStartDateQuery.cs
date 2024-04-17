using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apprenticeships.Extensions;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.Interfaces;

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

		string? employerName = await GetEmployerName(apprenticeStartDateInnerModel);

		var providerName = await GetProviderName(apprenticeStartDateInnerModel);

		var apprenticeshipStartDateOuterModel = new ApprenticeshipStartDateResponse
		{
			ApprenticeshipKey = apprenticeStartDateInnerModel!.ApprenticeshipKey,
			ActualStartDate = apprenticeStartDateInnerModel.ActualStartDate,
			PlannedEndDate = apprenticeStartDateInnerModel.PlannedEndDate,
			EmployerName = employerName,
			ProviderName = providerName,
			EarliestStartDate = await GetEarliestNewStartDate(apprenticeStartDateInnerModel.ActualStartDate),
			LatestStartDate = await GetLatestNewStartDate(apprenticeStartDateInnerModel.ActualStartDate),
			LastFridayOfSchool = apprenticeStartDateInnerModel.ApprenticeDateOfBirth.GetLastFridayInJuneOfSchoolYearApprenticeTurned16()
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

    private async Task<DateTime?> GetEarliestNewStartDate(DateTime? currentActualStartDate)
    {
        if (currentActualStartDate == null) return null;

        var academicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearsRequest(currentActualStartDate.Value));

        return academicYear.StartDate;
    }

    private async Task<DateTime?> GetLatestNewStartDate(DateTime? currentActualStartDate)
    {
		if (currentActualStartDate == null) return null; 

		var nextYear = currentActualStartDate.Value.AddYears(1);

        var academicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearsRequest(nextYear));

        return academicYear.EndDate;
    }
}