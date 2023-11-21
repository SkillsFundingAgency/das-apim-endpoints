using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
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
            GetApprenticeshipPriceResponse expectedResponse,
            Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> mockApprenticeshipsApiClient)
        {
            //  Arrange
            mockApprenticeshipsApiClient.Setup(x=>x.Get<GetApprenticeshipPriceResponse>(It.IsAny<GetApprenticeshipPriceRequest>()))
                .ReturnsAsync(expectedResponse);
            var controller = new ApprenticeshipController(mockApprenticeshipsApiClient.Object);

            //  Act
            var result = await controller.GetApprenticeshipPrice(Guid.NewGuid());

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<GetApprenticeshipPriceResponse>();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
