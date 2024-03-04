using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
			Mock<ILogger<ApprenticeshipController>> mockLogger,
			Mock<IMediator> mockMediator,
			Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> mockApprenticeshipsApiClient)
        {
            //  Arrange
            mockApprenticeshipsApiClient.Setup(x => x.Get<Guid>(It.IsAny<GetApprenticeshipKeyRequest>()))
                .ReturnsAsync(expectedResponse);
			var controller = new ApprenticeshipController(mockLogger.Object, mockApprenticeshipsApiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), mockMediator.Object);

			//  Act
			var result = await controller.GetApprenticeshipKey("anyApprenticeshipHashedId");

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<Guid>();
            actualResponse.Should().Be(expectedResponse);
        }
    }
}
