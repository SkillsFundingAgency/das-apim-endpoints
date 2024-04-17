using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Controllers;
using SFA.DAS.Apprenticeships.Api.Models;
using SFA.DAS.Apprenticeships.InnerApi;
using SFA.DAS.Apprenticeships.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.Interfaces;
using GetProviderResponse = SFA.DAS.Apprenticeships.Api.Models.GetProviderResponse;

namespace SFA.DAS.Apprenticeships.Api.UnitTests.Controllers.Apprenticeship;

#pragma warning disable CS8618
public class WhenGettingPendingStartDateChange
{
    private Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>> _mockApprenticeshipsApiClient;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _mockCommitmentsApiClient;
    private ApprenticeshipController _sut;
    private Fixture _fixture;
    private Guid _apprenticeshipKey;
    private GetPendingStartDateChangeApiResponse _apiResponse;
    private GetProviderResponse _providerResponse;
    private GetAccountLegalEntityResponse _accountLegalEntityResponse;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();

        _mockApprenticeshipsApiClient = new Mock<IApprenticeshipsApiClient<ApprenticeshipsApiConfiguration>>();
        _mockCommitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

        _apprenticeshipKey = _fixture.Create<Guid>();
        _apiResponse = _fixture.Create<GetPendingStartDateChangeApiResponse>();
        _providerResponse = _fixture.Create<GetProviderResponse>();
        _accountLegalEntityResponse = _fixture.Create<GetAccountLegalEntityResponse>();

        _mockApprenticeshipsApiClient.Setup(x =>
                x.Get<GetPendingStartDateChangeApiResponse>(
                    It.Is<GetPendingStartDateChangeRequest>(r => r.ApprenticeshipKey == _apprenticeshipKey)))
            .ReturnsAsync(_apiResponse);
        _mockCommitmentsApiClient.Setup(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()))
            .ReturnsAsync(_providerResponse);
        _mockCommitmentsApiClient.Setup(x =>
                x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
            .ReturnsAsync(_accountLegalEntityResponse);

        _sut = new ApprenticeshipController(Mock.Of<ILogger<ApprenticeshipController>>(),
            _mockApprenticeshipsApiClient.Object, _mockCommitmentsApiClient.Object, Mock.Of<IMediator>());
    }

    [Test]
    public async Task Then_Gets_PendingStartDateChange_From_ApiClient()
    {
        // Act
        var result = await _sut.GetPendingStartDateChange(_apprenticeshipKey);

        // Assert
        var okObjectResult = result.ShouldBeOfType<OkObjectResult>();
        var actualResponse = okObjectResult.Value.ShouldBeOfType<GetPendingStartDateChangeResponse>();
        actualResponse.Should().BeEquivalentTo(new GetPendingStartDateChangeResponse(_apiResponse,
            _providerResponse.Name, _apprenticeshipKey, _accountLegalEntityResponse.AccountName));
    }

    [Test]
    public async Task Then_Gets_Provider_From_ApiClient()
    {
        // Act
        await _sut.GetPendingStartDateChange(_apprenticeshipKey);

        // Assert
        _mockCommitmentsApiClient.Verify(x => x.Get<GetProviderResponse>(It.IsAny<GetProviderRequest>()), Times.Once);
    }

    [Test]
    public async Task Then_Gets_Employer_From_ApiClient()
    {
        // Act
        await _sut.GetPendingStartDateChange(_apprenticeshipKey);

        // Assert
        _mockCommitmentsApiClient.Verify(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()), Times.Once);
    }
}