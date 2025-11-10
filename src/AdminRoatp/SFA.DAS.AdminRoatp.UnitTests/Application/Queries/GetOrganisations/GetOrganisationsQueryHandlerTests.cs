using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisations;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisations;
public class GetOrganisationsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_SuccessfulResponse_ReturnsData(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationsQuery query,
        GetOrganisationsQueryHandler sut
        )
    {
        // Arrange
        GetOrganisationsResponse apiResponse = new()
        {
            Organisations = new List<OrganisationResponse>
            { new() { Ukprn = 12345, LegalName = "Test1" }, new() { Ukprn = 56789, LegalName = "Test2" }}
        };
        apiClientMock.Setup(a => a.GetWithResponseCode<GetOrganisationsResponse>(It.Is<GetOrganisationsRequest>(c => c.GetUrl.Equals(new GetOrganisationsRequest().GetUrl)))).ReturnsAsync(new ApiResponse<GetOrganisationsResponse>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        apiClientMock.Verify(a => a.GetWithResponseCode<GetOrganisationsResponse>(It.Is<GetOrganisationsRequest>(c => c.GetUrl.Equals(new GetOrganisationsRequest().GetUrl))), Times.Once);
        result.Organisations.Count().Should().Be(2);
    }

    [Test, MoqAutoData]
    public async Task Handle_SuccessfulResponse_ReturnsEmpty(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationsQuery query,
        GetOrganisationsQueryHandler sut
        )
    {
        // Arrange
        GetOrganisationsResponse apiResponse = new();
        apiClientMock.Setup(a => a.GetWithResponseCode<GetOrganisationsResponse>(It.Is<GetOrganisationsRequest>(c => c.GetUrl.Equals(new GetOrganisationsRequest().GetUrl)))).ReturnsAsync(new ApiResponse<GetOrganisationsResponse>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Organisations.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationsQuery query,
        GetOrganisationsQueryHandler sut)
    {
        // Arrange
        var apiResponse = new ApiResponse<GetOrganisationsResponse>(It.IsAny<GetOrganisationsResponse>(), HttpStatusCode.InternalServerError, "");
        apiClientMock.Setup(a => a.GetWithResponseCode<GetOrganisationsResponse>(It.IsAny<GetOrganisationsRequest>())).ReturnsAsync(apiResponse);

        // Act
        Func<Task> result = () => sut.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await result.Should().ThrowAsync<ApiResponseException>();
    }
}
