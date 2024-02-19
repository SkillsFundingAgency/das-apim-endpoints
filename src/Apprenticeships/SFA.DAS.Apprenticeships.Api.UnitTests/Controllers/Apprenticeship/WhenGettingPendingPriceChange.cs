using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship
{
    public class WhenGettingPendingPriceChange
    {
	    private Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> _mockApprenticeshipsApiClient;
	    private ApprenticeshipController _sut;
        private Fixture _fixture;

		[SetUp]
	    public void SetUp()
	    {
            _fixture = new Fixture();

		    _mockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
            _sut = new ApprenticeshipController(Mock.Of<ILogger<ApprenticeshipController>>(), _mockApprenticeshipsApiClient.Object, Mock.Of<IMediator>());
        }

        [Test]
        public async Task Then_Gets_PendingPriceChangePrice_From_ApiClient()
        {
            //  Arrange
            var apprenticeshipKey = _fixture.Create<Guid>();
            var apiResponse = _fixture.Create<GetPendingPriceChangeApiResponse>();
            _mockApprenticeshipsApiClient.Setup(x=>x.Get<GetPendingPriceChangeApiResponse>(It.Is<GetPendingPriceChangeRequest>(r => r.ApprenticeshipKey == apprenticeshipKey)))
                .ReturnsAsync(apiResponse);

            //  Act
            var result = await _sut.GetPendingPriceChange(apprenticeshipKey);

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<GetPendingPriceChangeResponse>();
            actualResponse.Should().BeEquivalentTo(apiResponse);
        }
    }
}
