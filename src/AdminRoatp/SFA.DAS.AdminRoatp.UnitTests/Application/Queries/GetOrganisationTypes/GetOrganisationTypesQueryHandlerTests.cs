using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisationTypes;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisationTypes;
public class GetOrganisationTypesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_OrganisationTypesFound_ReturnsOrganisationTypes(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationTypesQueryHandler sut,
        GetOrganisationTypesResponse apiResponse,
        GetOrganisationTypesQuery query)
    {
        // Arrange
        apiClientMock.Setup(m => m.GetWithResponseCode<GetOrganisationTypesResponse>(It.Is<GetOrganisationTypesRequest>(r => r.GetUrl.Equals(new GetOrganisationTypesRequest().GetUrl)))).ReturnsAsync(new ApiResponse<GetOrganisationTypesResponse>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.OrganisationTypes.Should().BeEquivalentTo(apiResponse.OrganisationTypes);
        apiClientMock.Verify(m => m.GetWithResponseCode<GetOrganisationTypesResponse>(It.Is<GetOrganisationTypesRequest>(r => r.GetUrl.Equals(new GetOrganisationTypesRequest().GetUrl))), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Handle_OrganisationTypesNotFound_ReturnsEmpty(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationTypesQueryHandler sut,
        GetOrganisationTypesQuery query)
    {
        // Arrange
        GetOrganisationTypesResponse apiResponse = new();
        apiClientMock.Setup(m => m.GetWithResponseCode<GetOrganisationTypesResponse>(It.Is<GetOrganisationTypesRequest>(r => r.GetUrl.Equals(new GetOrganisationTypesRequest().GetUrl)))).ReturnsAsync(new ApiResponse<GetOrganisationTypesResponse>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.OrganisationTypes.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationTypesQueryHandler sut,
        GetOrganisationTypesQuery query)
    {
        // Arrange
        GetOrganisationTypesResponse apiResponse = new();
        apiClientMock.Setup(m => m.GetWithResponseCode<GetOrganisationTypesResponse>(It.Is<GetOrganisationTypesRequest>(r => r.GetUrl.Equals(new GetOrganisationTypesRequest().GetUrl)))).ReturnsAsync(new ApiResponse<GetOrganisationTypesResponse>(apiResponse, HttpStatusCode.InternalServerError, ""));

        // Act
        Func<Task> result = () => sut.Handle(query, It.IsAny<CancellationToken>());

        // Assert
        await result.Should().ThrowAsync<ApiResponseException>();
    }
}