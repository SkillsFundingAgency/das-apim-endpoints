using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
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
        UkprnLookupResponse apiResponse = new()
        {
            Success = true,
            Results = new List<ProviderDetails>
            {
                new ProviderDetails
                {
                    ProviderName = "TestName1",
                    ProviderAliases = new List<ProviderAlias>
                    {
                        new ProviderAlias
                        {
                            Alias = "TestAlias1"
                        },
                    },
                    VerificationDetails = new List<VerificationDetails>
                    {
                        new VerificationDetails
                        {
                            VerificationAuthority = "charity commission",
                            VerificationId = "12345"
                        },
                        new VerificationDetails
                        {
                            VerificationAuthority = "companies house",
                            VerificationId = "67890"
                        }
                    }

                }
            }
        };

        var expectedResponse = new GetUkrlpQueryResult
        {
            LegalName = "TestName1",
            TradingName = "TestAlias1",
            CharityNumber = "12345",
            CompanyNumber = "67890"
        };

        apiClientMock.Setup(a => a.GetWithResponseCode<UkprnLookupResponse>(It.Is<GetUkrlpRequest>(r => r.GetUrl.Equals(new GetUkrlpRequest(query.Ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<UkprnLookupResponse>(apiResponse, System.Net.HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedResponse);
        apiClientMock.Verify(a => a.GetWithResponseCode<UkprnLookupResponse>(It.Is<GetUkrlpRequest>(r => r.GetUrl.Equals(new GetUkrlpRequest(query.Ukprn).GetUrl))), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_SuccessfulResponse_ReturnsNull(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetUkrlpQueryHandler sut,
        GetUkrlpQuery query)
    {
        // Arrange
        UkprnLookupResponse apiResponse = new();

        apiClientMock.Setup(a => a.GetWithResponseCode<UkprnLookupResponse>(It.Is<GetUkrlpRequest>(r => r.GetUrl.Equals(new GetUkrlpRequest(query.Ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<UkprnLookupResponse>(apiResponse, System.Net.HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetUkrlpQueryHandler sut,
        GetUkrlpQuery query)
    {
        // Arrange
        UkprnLookupResponse apiResponse = new();

        apiClientMock.Setup(a => a.GetWithResponseCode<UkprnLookupResponse>(It.Is<GetUkrlpRequest>(r => r.GetUrl.Equals(new GetUkrlpRequest(query.Ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<UkprnLookupResponse>(apiResponse, System.Net.HttpStatusCode.InternalServerError, ""));

        // Act
        Func<Task> result = () => sut.Handle(query, CancellationToken.None);

        // Assert
        await result.Should().ThrowAsync<ApiResponseException>();
    }
}