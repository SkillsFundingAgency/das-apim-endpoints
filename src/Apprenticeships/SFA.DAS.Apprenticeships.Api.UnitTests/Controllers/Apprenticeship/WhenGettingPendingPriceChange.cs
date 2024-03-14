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
using SFA.DAS.Apprenticeships.InnerApi;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apprenticeships.Responses;
using GetProviderResponse = SFA.DAS.Apprenticeships.Api.Models.GetProviderResponse;
using System.Configuration.Provider;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship
{
    public class WhenGettingPendingPriceChange
    {
	    private Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> _mockApprenticeshipsApiClient;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _mockCommitmentsApiClient;
	    private ApprenticeshipController _sut;
        private Fixture _fixture;

		[SetUp]
	    public void SetUp()
	    {
            _fixture = new Fixture();

		    _mockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
            _mockCommitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _sut = new ApprenticeshipController(Mock.Of<ILogger<ApprenticeshipController>>(), _mockApprenticeshipsApiClient.Object, _mockCommitmentsApiClient.Object, Mock.Of<IMediator>());
        }

        [Test]
        public async Task Then_Gets_PendingPriceChangePrice_From_ApiClient()
        {
            //  Arrange
            var apprenticeshipKey = _fixture.Create<Guid>();
            var apiResponse = _fixture.Create<GetPendingPriceChangeApiResponse>();
            _mockApprenticeshipsApiClient.Setup(x=>x.Get<GetPendingPriceChangeApiResponse>(It.Is<GetPendingPriceChangeRequest>(r => r.ApprenticeshipKey == apprenticeshipKey)))
                .ReturnsAsync(apiResponse);
            _mockCommitmentsApiClient.Setup(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(new GetProviderResponse { Name = _fixture.Create<string>() });
            _mockCommitmentsApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(_fixture.Create<GetAccountLegalEntityResponse>());

            //  Act
            var result = await _sut.GetPendingPriceChange(apprenticeshipKey);

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<GetPendingPriceChangeResponse>();
            actualResponse.Should().BeEquivalentTo(apiResponse);
        }

        [Test]
        public async Task Then_Gets_ProviderName_From_ApiClient()
        {
            //  Arrange
            var apprenticeshipKey = _fixture.Create<Guid>();
            var apiResponse = _fixture.Create<GetPendingPriceChangeApiResponse>();
            var providerName = _fixture.Create<string>();
            _mockApprenticeshipsApiClient.Setup(x => x.Get<GetPendingPriceChangeApiResponse>(It.Is<GetPendingPriceChangeRequest>(r => r.ApprenticeshipKey == apprenticeshipKey)))
                .ReturnsAsync(apiResponse);
            _mockCommitmentsApiClient.Setup(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(new GetProviderResponse { Name = providerName });
            _mockCommitmentsApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(_fixture.Create<GetAccountLegalEntityResponse>());

            //  Act
            var result = await _sut.GetPendingPriceChange(apprenticeshipKey);

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<GetPendingPriceChangeResponse>();
            actualResponse.ProviderName.Should().BeEquivalentTo(providerName);
        }

        [Test]
        public async Task Then_Gets_EmployerName_From_ApiClient()
        {
            //  Arrange
            var apprenticeshipKey = _fixture.Create<Guid>();
            var apiResponse = _fixture.Create<GetPendingPriceChangeApiResponse>();
            var employerName = _fixture.Create<string>();
            _mockApprenticeshipsApiClient.Setup(x => x.Get<GetPendingPriceChangeApiResponse>(It.Is<GetPendingPriceChangeRequest>(r => r.ApprenticeshipKey == apprenticeshipKey)))
                .ReturnsAsync(apiResponse);
            _mockCommitmentsApiClient.Setup(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(_fixture.Create<GetProviderResponse>());
            _mockCommitmentsApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse() { AccountName = employerName });

            //  Act
            var result = await _sut.GetPendingPriceChange(apprenticeshipKey);

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<GetPendingPriceChangeResponse>();
            actualResponse.EmployerName.Should().BeEquivalentTo(employerName);
        }
    }
}
