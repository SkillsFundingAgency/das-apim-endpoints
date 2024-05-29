using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.Apprenticeships.Application.Apprenticeship;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship
{
    public class WhenGettingApprenticeshipStartDate
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipStartDate_From_ApiClient(
            ApprenticeshipStartDateResponse expectedResponse,
            Mock<ILogger<ApprenticeshipController>> mockLogger,
            Mock<IMediator> mockMediator,
            Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> mockApprenticeshipsApiClient)
        {
            //  Arrange
            mockMediator.Setup(x => x.Send(It.IsAny<GetApprenticeshipStartDateQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

            var controller = new ApprenticeshipController(mockLogger.Object, mockApprenticeshipsApiClient.Object, Mock.Of<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>(), mockMediator.Object);

            //  Act
            var result = await controller.GetApprenticeshipStartDate(Guid.NewGuid());

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<ApprenticeshipStartDateResponse>();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
