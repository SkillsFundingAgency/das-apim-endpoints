using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship
{
    public class WhenGettingApprenticeshipKey
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipKey_From_ApiClient(
            Guid expectedResponse,
            Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> mockApprenticeshipsApiClient,
            Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsV2ApiApiClient,
			Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> mockCollectionCalendarApiClient)
        {
            //  Arrange
            mockApprenticeshipsApiClient.Setup(x => x.Get<Guid>(It.IsAny<GetApprenticeshipKeyRequest>()))
                .ReturnsAsync(expectedResponse);
            var controller = new ApprenticeshipController(mockApprenticeshipsApiClient.Object, mockCommitmentsV2ApiApiClient.Object, mockCollectionCalendarApiClient.Object);

            //  Act
            var result = await controller.GetApprenticeshipKey("anyApprenticeshipHashedId");

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<Guid>();
            actualResponse.Should().Be(expectedResponse);
        }
    }
}
