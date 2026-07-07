using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetProvidersNotAllowed;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetProvidersNotAllowed;

public class GetProvidersNotAllowedQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenReturnsResults(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllowedProvidersResponse response,
            GetProvidersNotAllowedQuery query,
            GetProvidersNotAllowedQueryHandler sut)
    {
        // Arrange
        apiClientMock.Setup(c => c.GetWithResponseCode<GetAllowedProvidersResponse>(It.IsAny<GetProvidersNotAllowedRequest>())).ReturnsAsync(new ApiResponse<GetAllowedProvidersResponse>(response, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, new CancellationToken());

        // Assert
        result.Should().BeEquivalentTo(response);
    }

    [Test, MoqAutoData]
    public void WhenHandlingRequestAndApiReturnsInvalidResponseCode_ThenThrowsApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProvidersNotAllowedQuery query,
        GetProvidersNotAllowedQueryHandler sut)
    {
        // Arrange
        apiClientMock.Setup(c => c.GetWithResponseCode<GetAllowedProvidersResponse>(It.IsAny<GetProvidersNotAllowedRequest>())).ReturnsAsync(new ApiResponse<GetAllowedProvidersResponse>(new GetAllowedProvidersResponse(), HttpStatusCode.InternalServerError, ""));

        // Act & Assert
        Assert.ThrowsAsync<ApiResponseException>(() => sut.Handle(query, new CancellationToken()));
    }
}
