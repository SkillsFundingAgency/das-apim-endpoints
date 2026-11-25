using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetUkrlp;

public class GetUkrlpQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_SuccessfulResponse_ReturnsData(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetUkrlpQueryHandler sut,
        GetUkrlpQuery query)
    {
        // Arrange
        var apiResponse = new UkrlpProviderModel
        {
            LegalName = "TestName1",
            TradingName = "TestAlias1",
            VerificationDetails =
            [
                new (VerificationAuthority.CharityCommission, "12345", false),
                new (VerificationAuthority.CompaniesHouse, "67890", false)
            ]
        };

        var expectedResponse = new GetUkrlpQueryResult
        {
            LegalName = "TestName1",
            TradingName = "TestAlias1",
            CharityNumber = "12345",
            CompanyNumber = "67890"
        };

        apiClientMock.Setup(a => a.GetWithResponseCode<UkrlpProviderModel>(It.Is<GetUkrlpRequest>(r => r.GetUrl.Equals(new GetUkrlpRequest(query.Ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<UkrlpProviderModel>(apiResponse, System.Net.HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        apiClientMock.Verify(a => a.GetWithResponseCode<UkrlpProviderModel>(It.Is<GetUkrlpRequest>(r => r.GetUrl.Equals(new GetUkrlpRequest(query.Ukprn).GetUrl))), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetUkrlpQueryHandler sut,
        GetUkrlpQuery query)
    {
        // Arrange
        UkrlpProviderModel apiResponse = new();

        apiClientMock.Setup(a => a.GetWithResponseCode<UkrlpProviderModel>(It.Is<GetUkrlpRequest>(r => r.GetUrl.Equals(new GetUkrlpRequest(query.Ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<UkrlpProviderModel>(apiResponse, System.Net.HttpStatusCode.InternalServerError, ""));

        // Act
        Func<Task> result = () => sut.Handle(query, CancellationToken.None);

        // Assert
        await result.Should().ThrowAsync<ApiResponseException>();
    }
}