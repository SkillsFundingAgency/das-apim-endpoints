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

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship
{
    public class WhenGettingPendingPriceChange
    {
        #pragma warning disable CS8618
        private Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> _mockApprenticeshipsApiClient;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _mockCommitmentsApiClient;
	    private ApprenticeshipController _sut;
        private Fixture _fixture;
        private Guid _apprenticeshipKey;
        private GetPendingPriceChangeApiResponse _apiResponse;
        private GetProviderResponse _providerResponse;
        private GetAccountLegalEntityResponse _accountLegalEntityResponse;

        [SetUp]
	    public void SetUp()
	    {
            _fixture = new Fixture();

		    _mockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
            _mockCommitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apprenticeshipKey = _fixture.Create<Guid>();
            _apiResponse = _fixture.Create<GetPendingPriceChangeApiResponse>();
            _providerResponse = _fixture.Create<GetProviderResponse>();
            _accountLegalEntityResponse = _fixture.Create<GetAccountLegalEntityResponse>();

            _mockApprenticeshipsApiClient.Setup(x => x.Get<GetPendingPriceChangeApiResponse>(It.Is<GetPendingPriceChangeRequest>(r => r.ApprenticeshipKey == _apprenticeshipKey)))
                .ReturnsAsync(_apiResponse);
            _mockCommitmentsApiClient.Setup(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(_providerResponse);
            _mockCommitmentsApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(_accountLegalEntityResponse);

            _sut = new ApprenticeshipController(Mock.Of<ILogger<ApprenticeshipController>>(), _mockApprenticeshipsApiClient.Object, _mockCommitmentsApiClient.Object, Mock.Of<IMediator>());
        }

        [Test]
        public async Task Then_Gets_PendingPriceChangePrice_From_ApiClient()
        {
            //  Act
            var result = await _sut.GetPendingPriceChange(_apprenticeshipKey);

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<GetPendingPriceChangeResponse>();
            actualResponse.Should().BeEquivalentTo(_apiResponse);
        }

        [Test]
        public async Task Then_Gets_ProviderName_From_ApiClient()
        {
            //  Act
            var result = await _sut.GetPendingPriceChange(_apprenticeshipKey);

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<GetPendingPriceChangeResponse>();
            actualResponse.ProviderName.Should().BeEquivalentTo(_providerResponse.Name);
        }

        [Test]
        public async Task Then_Gets_EmployerName_From_ApiClient()
        {
            //  Act
            var result = await _sut.GetPendingPriceChange(_apprenticeshipKey);

            //  Assert
            var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
            var actualResponse = okObjectResult.Value.ShouldBeOfType<GetPendingPriceChangeResponse>();
            actualResponse.EmployerName.Should().BeEquivalentTo(_accountLegalEntityResponse.AccountName);
        }
    }
}
