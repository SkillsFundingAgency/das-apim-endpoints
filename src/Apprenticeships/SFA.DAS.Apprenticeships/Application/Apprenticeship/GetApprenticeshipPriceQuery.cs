﻿using MediatR;
using Microsoft.Extensions.Logging;
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

public class GetApprenticeshipPriceQuery : IRequest<ApprenticeshipPriceResponse?>
{
	public Guid ApprenticeshipKey { get; set; }

	public GetApprenticeshipPriceQuery(Guid apprenticeshipKey)
	{
		ApprenticeshipKey = apprenticeshipKey;
	}
}

public class GetApprenticeshipPriceQueryHandler : IRequestHandler<GetApprenticeshipPriceQuery, ApprenticeshipPriceResponse?>
{
	private readonly ILogger<GetApprenticeshipPriceQueryHandler> _logger;
	private readonly ILearningApiClient<LearningApiConfiguration> _learningApiClient;
	private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiCommitmentsClient;
	private readonly ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> _collectionCalendarApiClient;

	public GetApprenticeshipPriceQueryHandler(
		ILogger<GetApprenticeshipPriceQueryHandler> logger,
		ILearningApiClient<LearningApiConfiguration> learningApiClient,
		ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiCommitmentsClient,
		ICollectionCalendarApiClient<CollectionCalendarApiConfiguration> collectionCalendarApiClient)
	{
		_logger = logger;
		_learningApiClient = learningApiClient;
		_apiCommitmentsClient = apiCommitmentsClient;
		_collectionCalendarApiClient = collectionCalendarApiClient;
	}

	public async Task<ApprenticeshipPriceResponse?> Handle(GetApprenticeshipPriceQuery request, CancellationToken cancellationToken)
	{
		var apprenticePriceInnerModel = await _learningApiClient.Get<GetLearningPriceResponse>(new GetLearningPriceRequest { LearningKey = request.ApprenticeshipKey });

		if (apprenticePriceInnerModel == null)
		{
			_logger.LogWarning($"No LearningPrice returned from innerApi for learning key:{request.ApprenticeshipKey}");
			return null;
		}

		string? employerName = await GetEmployerName(apprenticePriceInnerModel);

		var earliestEffectiveDate = await GetEarliestEffectiveDate();

		var providerName = await GetProviderName(apprenticePriceInnerModel);

		var apprenticeshipPriceOuterModel = new ApprenticeshipPriceResponse
		{
			ApprenticeshipKey = apprenticePriceInnerModel!.ApprenticeshipKey,
			ApprenticeshipActualStartDate = apprenticePriceInnerModel.ApprenticeshipActualStartDate,
			ApprenticeshipPlannedEndDate = apprenticePriceInnerModel.ApprenticeshipPlannedEndDate,
			AssessmentPrice = apprenticePriceInnerModel.AssessmentPrice,
			EarliestEffectiveDate = earliestEffectiveDate,
			FundingBandMaximum = apprenticePriceInnerModel.FundingBandMaximum,
			TrainingPrice = apprenticePriceInnerModel.TrainingPrice,
			EmployerName = employerName,
			ProviderName = providerName,
		};

		return apprenticeshipPriceOuterModel;
	}

	private async Task<string?> GetEmployerName(GetLearningPriceResponse apprenticePriceInnerModel)
	{
		if (!apprenticePriceInnerModel.AccountLegalEntityId.HasValue)
		{
			_logger.LogError($"No AccountLegalEntityId returned from innerApi for apprenticeshipKey:{apprenticePriceInnerModel.ApprenticeshipKey}");
			return null;
		}

		var employer = await _apiCommitmentsClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(apprenticePriceInnerModel.AccountLegalEntityId.Value));
		
		if(employer == null)
		{
			_logger.LogError($"No AccountLegalEntity returned from innerApi for apprenticeshipKey:{apprenticePriceInnerModel.ApprenticeshipKey}");
			return null;
		}

		return employer.LegalEntityName;
	}

	private async Task<string?> GetProviderName(GetLearningPriceResponse apprenticePriceInnerModel)
	{
		if (apprenticePriceInnerModel.UKPRN < 1)
		{
			_logger.LogError($"Invalid UKPRN '{apprenticePriceInnerModel.UKPRN}' returned from innerApi for apprenticeshipKey:{apprenticePriceInnerModel.ApprenticeshipKey}");
			return null;
		}

		var provider = await _apiCommitmentsClient.Get<GetProviderResponse>(new GetProviderRequest(apprenticePriceInnerModel.UKPRN));

		if (provider == null)
		{
			_logger.LogError($"No provider returned from CommitmentsClient for UKPRN:{apprenticePriceInnerModel.UKPRN}");
			return null;
		}

		return provider.Name;
	}


	private async Task<DateTime> GetEarliestEffectiveDate()
	{
		var searchDate = DateTime.Now;
		var currentAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByDateRequest(searchDate));
		if(currentAcademicYear == null)
		{
			throw new NotFoundException<GetAcademicYearsResponse>($"No current academic year returned from innerApi for date {searchDate.ToString("yyyy-MMM-dd")}");
		}

        if (!currentAcademicYear.HardCloseDate.HasValue)
        {
            throw new AcademicYearDataIncompleteException(PreviousOrCurrentAcademicYear.Current);
        }

		searchDate = currentAcademicYear.StartDate.AddDays(-1);
		var previousAcademicYear = await _collectionCalendarApiClient.Get<GetAcademicYearsResponse>(new GetAcademicYearByDateRequest(searchDate));
		if (previousAcademicYear == null)
		{
			throw new NotFoundException<GetAcademicYearsResponse>($"No previous academic year returned from innerApi for date {searchDate.ToString("yyyy - MMM - dd")}");
		}

        if (!previousAcademicYear.HardCloseDate.HasValue)
        {
            throw new AcademicYearDataIncompleteException(PreviousOrCurrentAcademicYear.Previous);
        }

        if (DateTime.Now > previousAcademicYear.HardCloseDate)
		{
			//earliest allowed date is 1st Aug current academic year
			return currentAcademicYear.StartDate;
		}
		else
		{
			//earliest allowed date is 1st Aug previous academic year
			return previousAcademicYear.StartDate;
		}
	}
}