using System.Net;
using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderAllowedCourseDetails;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetProviderAllowedCourseDetails;

public class GetProviderAllowedCourseDetailsQueryHandlerTests
{
    [Test, AutoData]
    public async Task WhenGettingProviderAllowedCourseDetails_AndDetailsFound_ReturnsDetails(GetProviderAllowedCourseDetailsQuery query, GetProviderAllowedCourseDetailsResponse expectedResponse)
    {
        // Arrange
        var mockApiClient = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        mockApiClient.Setup(x => x.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse>(It.IsAny<GetProviderAllowedCourseDetailsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCourseDetailsResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));
        var sut = new GetProviderAllowedCourseDetailsQueryHandler(mockApiClient.Object);
        // Act
        var result = await sut.Handle(query, CancellationToken.None);
        // Assert
        Assert.That(result, Is.EqualTo(expectedResponse));
    }

    [Test, AutoData]
    public async Task WhenGettingProviderAllowedCourseDetails_AndInnerRespondsWithNoContentResponse_ReturnsNull(GetProviderAllowedCourseDetailsQuery query)
    {
        // Arrange
        var mockApiClient = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        mockApiClient.Setup(x => x.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse?>(It.IsAny<GetProviderAllowedCourseDetailsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCourseDetailsResponse?>(null, HttpStatusCode.NoContent, string.Empty));
        var sut = new GetProviderAllowedCourseDetailsQueryHandler(mockApiClient.Object);
        // Act
        var result = await sut.Handle(query, CancellationToken.None);
        // Assert
        Assert.That(result, Is.Null);
    }

    [Test, AutoData]
    public void WhenGettingProviderAllowedCourseDetails_AndInnerApiRespondsWithError_ThrowsException(GetProviderAllowedCourseDetailsQuery query)
    {
        // Arrange
        var mockApiClient = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        mockApiClient.Setup(x => x.GetWithResponseCode<GetProviderAllowedCourseDetailsResponse?>(It.IsAny<GetProviderAllowedCourseDetailsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderAllowedCourseDetailsResponse?>(null, HttpStatusCode.InternalServerError, string.Empty));
        var sut = new GetProviderAllowedCourseDetailsQueryHandler(mockApiClient.Object);
        // Act & Assert
        Assert.ThrowsAsync<ApiResponseException>(async () => await sut.Handle(query, CancellationToken.None));
    }
}
