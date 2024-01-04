using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship
{
    public class WhenGettingApprenticeshipPrice
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipPrice_From_ApiClient(
            ApprenticeshipPriceResponse expectedResponse,
            Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> mockApprenticeshipsApiClient,
            Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiApiClient,
			Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> mockCollectionCalendarApiClient)
        {
            //  Arrange
            mockApprenticeshipsApiClient.Setup(x=>x.Get<GetApprenticeshipPriceResponse>(It.IsAny<GetApprenticeshipPriceRequest>()))
                .ReturnsAsync(new GetApprenticeshipPriceResponse
                {
                    AccountLegalEntityId = 1,
                    ApprenticeshipKey = expectedResponse.ApprenticeshipKey,
                    ApprenticeshipActualStartDate = expectedResponse.ApprenticeshipActualStartDate,
                    ApprenticeshipPlannedEndDate = expectedResponse.ApprenticeshipPlannedEndDate,
                    AssessmentPrice = expectedResponse.AssessmentPrice,
                    EarliestEffectiveDate = expectedResponse.EarliestEffectiveDate,
                    FundingBandMaximum = expectedResponse.FundingBandMaximum,
                    TrainingPrice = expectedResponse.TrainingPrice
                });

            mockCommitmentsV2ApiApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse
                {
                    LegalEntityName = expectedResponse.EmployerName!
                });

			mockCollectionCalendarApiClient.Setup(x => x.Get<GetAcademicYearsResponse>(It.IsAny<GetAcademicYearsRequest>())).ReturnsAsync(new GetAcademicYearsResponse
            {
				HardCloseDate = expectedResponse.HardCloseDate!.Value
			});

			var controller = new ApprenticeshipController(mockApprenticeshipsApiClient.Object, mockCommitmentsV2ApiApiClient.Object, mockCollectionCalendarApiClient.Object);

            //  Act
            var result = await controller.GetApprenticeshipPrice(Guid.NewGuid());

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<ApprenticeshipPriceResponse>();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
