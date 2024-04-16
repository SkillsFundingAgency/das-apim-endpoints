using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Application.Apprenticeship;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.UnitTests.Application.Apprenticeship;

public class WhenGettingApprenticeshipStartDate
{
	private readonly Mock<ILogger<GetApprenticeshipStartDateQueryHandler>> _mocklogger;

    public WhenGettingApprenticeshipStartDate()
    {
		_mocklogger = new Mock<ILogger<GetApprenticeshipStartDateQueryHandler>>();
    }

    [Test, MoqAutoData]
	public async Task Then_Gets_ApprenticeshipStartDate_From_ApiClient(
		ApprenticeshipStartDateResponse expectedResponse,
		DateTime effectiveFrom,
        DateTime effectiveTo,
        Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> mockApprenticeshipsApiClient,
		Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiApiClient,
		Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> mockCollectionCalendarApiClient)
	{
		//  Arrange
        expectedResponse.Standard = new StandardInfo
        {
            CourseCode = 456.ToString(),
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            Versions = new List<StandardVersionInfo>
            {
                new() { EffectiveFrom = effectiveFrom, EffectiveTo = effectiveTo, Version = "1.1" }
            }
        };

		mockApprenticeshipsApiClient.Setup(x => x.Get<GetApprenticeshipStartDateResponse>(It.IsAny<GetApprenticeshipStartDateRequest>()))
			.ReturnsAsync(new GetApprenticeshipStartDateResponse
			{
				AccountLegalEntityId = 1,
				ApprenticeshipKey = expectedResponse.ApprenticeshipKey,
				ActualStartDate = expectedResponse.ActualStartDate,
				PlannedEndDate = expectedResponse.PlannedEndDate,
				UKPRN = 123,
				CourseCode = 456
			});

		mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
			.ReturnsAsync(new GetAccountLegalEntityResponse
			{
				LegalEntityName = expectedResponse.EmployerName!
			});

        mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
            .ReturnsAsync(new GetProviderResponse
            {
                Name = expectedResponse.ProviderName!
            });

        mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetTrainingProgrammeVersionsResponse>(It.IsAny<GetTrainingProgrammeVersionsRequest>()))
            .ReturnsAsync(new GetTrainingProgrammeVersionsResponse
            {
                TrainingProgrammeVersions = new List<TrainingProgramme>
                {
					new()
                    {
                        CourseCode = expectedResponse.Standard.CourseCode,
                        EffectiveFrom = expectedResponse.Standard.EffectiveFrom,
						EffectiveTo = expectedResponse.Standard.EffectiveTo,
						Version = expectedResponse.Standard.Versions[0].Version
                    }
                }
            });

        var handler = new GetApprenticeshipStartDateQueryHandler(_mocklogger.Object, mockApprenticeshipsApiClient.Object, mockCommitmentsV2ApiApiClient.Object, mockCollectionCalendarApiClient.Object);

		//  Act
		var result = await handler.Handle(new GetApprenticeshipStartDateQuery( Guid.NewGuid()), CancellationToken.None);

		//  Assert
		result.Should().BeEquivalentTo(expectedResponse);
	}
}

