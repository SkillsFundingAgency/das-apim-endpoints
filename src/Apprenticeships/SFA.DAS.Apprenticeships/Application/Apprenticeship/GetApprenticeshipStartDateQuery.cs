using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apprenticeships.Extensions;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
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
	private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;
	private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiCommitmentsClient;
	private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;

    public GetApprenticeshipStartDateQueryHandler(
		ILogger<GetApprenticeshipStartDateQueryHandler> logger,
		ILearningApiClient<LearningApiConfiguration> learningApiClient,
		ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiCommitmentsClient,
		ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient)
	{
		_logger = logger;
		_learningApiClient = learningApiClient;
		_apiCommitmentsClient = apiCommitmentsClient;
		_collectionCalendarApiClient = collectionCalendarApiClient;
    }

	public async Task<ApprenticeshipStartDateResponse?> Handle(GetApprenticeshipStartDateQuery request, CancellationToken cancellationToken)
	{
		var apprenticeStartDateInnerModel = await _learningApiClient.Get<GetLearningStartDateResponse>(new GetLearningStartDateRequest { LearningKey = request.ApprenticeshipKey });

		if (apprenticeStartDateInnerModel == null)
		{
			_logger.LogWarning("No Learning StartDate returned from innerApi for learning key:{key}", request.ApprenticeshipKey);
			return null; 
		}

        var standard = await _apiCommitmentsClient.Get<GetTrainingProgrammeVersionsResponse>(new GetTrainingProgrammeVersionsRequest(apprenticeStartDateInnerModel.CourseCode));

        string? employerName = await GetEmployerName(apprenticeStartDateInnerModel);

		var providerName = await GetProviderName(apprenticeStartDateInnerModel);

		var currentAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByDateRequest(DateTime.Now));
        if (!currentAcademicYear.HardCloseDate.HasValue)
        {
            throw new AcademicYearDataIncompleteException(PreviousOrCurrentAcademicYear.Current);
        }
        var previousAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByDateRequest(DateTime.Now.AddYears(-1)));
        if (!previousAcademicYear.HardCloseDate.HasValue)
        {
            throw new AcademicYearDataIncompleteException(PreviousOrCurrentAcademicYear.Previous);
        }

        var apprenticeshipStartDateOuterModel = new ApprenticeshipStartDateResponse
		{
			ApprenticeshipKey = apprenticeStartDateInnerModel.LearningKey,
			ActualStartDate = apprenticeStartDateInnerModel.ActualStartDate,
			PlannedEndDate = apprenticeStartDateInnerModel.PlannedEndDate,
			EmployerName = employerName,
			ProviderName = providerName,
			EarliestStartDate = await GetEarliestNewStartDate(apprenticeStartDateInnerModel.ActualStartDate, apprenticeStartDateInnerModel.SimplifiedPaymentsMinimumStartDate),
			LatestStartDate = await GetLatestNewStartDate(apprenticeStartDateInnerModel.ActualStartDate),
			LastFridayOfSchool = apprenticeStartDateInnerModel.ApprenticeDateOfBirth.GetLastFridayInJuneOfSchoolYearApprenticeTurned16(),
			Standard = ToStandardInfo(standard, apprenticeStartDateInnerModel.CourseVersion),
            CurrentAcademicYear = ToAcademicYearDetails(currentAcademicYear),
            PreviousAcademicYear = ToAcademicYearDetails(previousAcademicYear)
        };

        if (apprenticeshipStartDateOuterModel.Standard.CourseCode == null)
        {
            _logger.LogWarning("No course/standard data available for apprenticeshipKey:{key}", request.ApprenticeshipKey);
        }

        if (apprenticeshipStartDateOuterModel.Standard.StandardVersion == null)
        {
            _logger.LogWarning("No standard version data available for apprenticeshipKey:{key}", request.ApprenticeshipKey);
        }

        return apprenticeshipStartDateOuterModel;
	}

	private async Task<string?> GetEmployerName(GetLearningStartDateResponse apprenticeStartDateInnerModel)
	{
		if (!apprenticeStartDateInnerModel.AccountLegalEntityId.HasValue)
		{
			_logger.LogError("No AccountLegalEntityId returned from innerApi for apprenticeshipKey:{key}", apprenticeStartDateInnerModel.LearningKey);
			return null;
		}

		var employer = await _apiCommitmentsClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(apprenticeStartDateInnerModel.AccountLegalEntityId.Value));
		
		if(employer == null)
		{
			_logger.LogError("No AccountLegalEntity returned from innerApi for apprenticeshipKey:{key}", apprenticeStartDateInnerModel.LearningKey);
			return null;
		}

		return employer.LegalEntityName;
	}

	private async Task<string?> GetProviderName(GetLearningStartDateResponse apprenticeStartDateInnerModel)
	{
		if (apprenticeStartDateInnerModel.UKPRN < 1)
		{
			_logger.LogError("Invalid UKPRN '{UKPRN}' returned from innerApi for apprenticeshipKey:{key}", apprenticeStartDateInnerModel.UKPRN, apprenticeStartDateInnerModel.LearningKey);
			return null;
		}

		var provider = await _apiCommitmentsClient.Get<GetProviderResponse>(new GetProviderRequest(apprenticeStartDateInnerModel.UKPRN));

		if (provider == null)
		{
			_logger.LogError("No provider returned from CommitmentsClient for UKPRN:{UKPRN}", apprenticeStartDateInnerModel.UKPRN);
			return null;
		}

		return provider.Name;
	}

    private static StandardInfo ToStandardInfo(GetTrainingProgrammeVersionsResponse response, string courseVersion)
    {
        return new StandardInfo
        {
			 CourseCode = response.TrainingProgrammeVersions.FirstOrDefault(x => x.Version == courseVersion)?.CourseCode,
			 EffectiveFrom = response.TrainingProgrammeVersions.Any(x => x.EffectiveFrom == null) ? null : response.TrainingProgrammeVersions.MinBy(x => x.EffectiveFrom)?.EffectiveFrom,
             EffectiveTo = response.TrainingProgrammeVersions.Any(x => x.EffectiveTo == null) ? null : response.TrainingProgrammeVersions.MaxBy(x => x.EffectiveTo)?.EffectiveTo,
			 StandardVersion = response.TrainingProgrammeVersions.Select(x => new StandardVersionInfo
             {
                 VersionEarliestStartDate = x.VersionEarliestStartDate,
                 VersionLatestStartDate = x.VersionLatestStartDate,
                 Version = x.Version
             }).FirstOrDefault(x => x.Version == courseVersion)
        };
    }

    private static AcademicYearDetails ToAcademicYearDetails(GetAcademicYearsResponse response)
    {
        return new AcademicYearDetails
        {
            AcademicYear = response.AcademicYear,
            StartDate = response.StartDate,
            EndDate = response.EndDate,
            HardCloseDate = response.HardCloseDate.Value
        };
    }

    private async Task<DateTime?> GetEarliestNewStartDate(DateTime? currentActualStartDate, DateTime simplifiedPayentsMinimumStartDate)
    {
        if (currentActualStartDate == null) return null;

        var academicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByDateRequest(currentActualStartDate.Value));

        _logger.LogInformation("currentActualStartDate: {0} | academicYearStartDate: {1} | simplifiedPaymentsMinimumStartDate: {2}", currentActualStartDate, academicYear.StartDate, simplifiedPayentsMinimumStartDate);

        return academicYear.StartDate > simplifiedPayentsMinimumStartDate ? academicYear.StartDate : simplifiedPayentsMinimumStartDate;
    }

    private async Task<DateTime?> GetLatestNewStartDate(DateTime? currentActualStartDate)
    {
		if (currentActualStartDate == null) return null; 

		var nextYear = currentActualStartDate.Value.AddYears(1);

        var academicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByDateRequest(nextYear));

        return academicYear.EndDate;
    }
}