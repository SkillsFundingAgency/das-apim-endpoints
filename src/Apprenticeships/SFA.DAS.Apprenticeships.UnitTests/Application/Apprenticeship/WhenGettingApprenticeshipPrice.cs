﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Apprenticeship;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Apprenticeship;

public class WhenGettingApprenticeshipPrice
{
	private readonly Mock<ILogger<GetApprenticeshipPriceQueryHandler>> _mocklogger;

    public WhenGettingApprenticeshipPrice()
    {
		_mocklogger = new Mock<ILogger<GetApprenticeshipPriceQueryHandler>>();
    }

    [Test, MoqAutoData]
	public async Task Then_Gets_ApprenticeshipPrice_From_ApiClient(
		ApprenticeshipPriceResponse expectedResponse,
		Mock<ILearningApiClient<LearningApiConfiguration>> mockApprenticeshipsApiClient,
		Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiApiClient,
		Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> mockCollectionCalendarApiClient)
	{
		//  Arrange
		mockApprenticeshipsApiClient.Setup(x => x.Get<GetLearningPriceResponse>(It.IsAny<GetLearningPriceRequest>()))
			.ReturnsAsync(new GetLearningPriceResponse
			{
				AccountLegalEntityId = 1,
				ApprenticeshipKey = expectedResponse.ApprenticeshipKey,
				ApprenticeshipActualStartDate = expectedResponse.ApprenticeshipActualStartDate,
				ApprenticeshipPlannedEndDate = expectedResponse.ApprenticeshipPlannedEndDate,
				AssessmentPrice = expectedResponse.AssessmentPrice,
				FundingBandMaximum = expectedResponse.FundingBandMaximum,
				TrainingPrice = expectedResponse.TrainingPrice,
				UKPRN = 123
			});

		mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
			.ReturnsAsync(new GetAccountLegalEntityResponse
			{
				LegalEntityName = expectedResponse.EmployerName!
			});

		mockCollectionCalendarApiClient.Setup(x => x.Get<GetAcademicYearsResponse>(It.IsAny<GetAcademicYearByDateRequest>())).ReturnsAsync(new GetAcademicYearsResponse
		{
			StartDate = expectedResponse.EarliestEffectiveDate,
			HardCloseDate = expectedResponse.EarliestEffectiveDate
		});

        mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
            .ReturnsAsync(new GetProviderResponse
            {
                Name = expectedResponse.ProviderName!
            });

        var handler = new GetApprenticeshipPriceQueryHandler(_mocklogger.Object, mockApprenticeshipsApiClient.Object, mockCommitmentsV2ApiApiClient.Object, mockCollectionCalendarApiClient.Object);

		//  Act
		var result = await handler.Handle(new GetApprenticeshipPriceQuery( Guid.NewGuid()), CancellationToken.None);

		//  Assert
		result.Should().BeEquivalentTo(expectedResponse);
	}
}

