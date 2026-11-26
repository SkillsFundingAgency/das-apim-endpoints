using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderRestrictedApprenticeships;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetProviderRestrictedApprenticeships;

public class GetProviderRestrictedApprenticeshipsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenHandleingSuccessfulResponse_ThenReturnsCourses(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderRestrictedApprenticeshipsQuery query,
        GetProviderRestrictedApprenticeshipsResponse apiResponse,
        GetProviderRestrictedApprenticeshipsQueryHandler sut)
    {
        // Arrange
        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderRestrictedApprenticeshipsResponse>(It.Is<GetProviderRestrictedApprenticeshipsRequest>(c => c.GetUrl.Equals(new GetProviderRestrictedApprenticeshipsRequest(query.Ukprn).GetUrl))))
            .ReturnsAsync(new ApiResponse<GetProviderRestrictedApprenticeshipsResponse>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert

        result.Courses.Should().BeEquivalentTo(apiResponse.Courses);

        apiClientMock.Verify(a =>
            a.GetWithResponseCode<GetProviderRestrictedApprenticeshipsResponse>(It.Is<GetProviderRestrictedApprenticeshipsRequest>(c => c.GetUrl.Equals(new GetProviderRestrictedApprenticeshipsRequest(query.Ukprn).GetUrl))),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenHandleingUnsuccessfulResponse_ThenThrowsException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderRestrictedApprenticeshipsQuery query,
        GetProviderRestrictedApprenticeshipsQueryHandler sut)
    {
        // Arrange
        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderRestrictedApprenticeshipsResponse>(It.IsAny<GetProviderRestrictedApprenticeshipsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderRestrictedApprenticeshipsResponse>(new GetProviderRestrictedApprenticeshipsResponse(), HttpStatusCode.InternalServerError, ""));

        // Act
        Func<Task> result = () => sut.Handle(query, CancellationToken.None);

        // Assert
        await result.Should().ThrowAsync<ApiResponseException>();
    }
}
