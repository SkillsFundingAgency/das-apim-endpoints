using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderNotRestrictedApprenticeships;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetProviderNotRestrictedApprenticeships;

public class GetProviderNotRestrictedApprenticeshipsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenHandleingSuccessfulResponse_ThenReturnsCourses(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderNotRestrictedApprenticeshipsQuery query,
        GetProviderNotRestrictedApprenticeshipsResponse apiResponse,
        GetProviderNotRestrictedApprenticeshipsQueryHandler sut)
    {
        // Arrange
        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderNotRestrictedApprenticeshipsResponse>(It.Is<GetProviderNotRestrictedApprenticeshipsRequest>(c => c.GetUrl.Equals(new GetProviderNotRestrictedApprenticeshipsRequest(query.Ukprn).GetUrl))))
            .ReturnsAsync(new ApiResponse<GetProviderNotRestrictedApprenticeshipsResponse>(apiResponse, HttpStatusCode.OK, ""));

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert

        result.Courses.Should().BeEquivalentTo(apiResponse.Courses);

        apiClientMock.Verify(a =>
            a.GetWithResponseCode<GetProviderNotRestrictedApprenticeshipsResponse>(It.Is<GetProviderNotRestrictedApprenticeshipsRequest>(c => c.GetUrl.Equals(new GetProviderNotRestrictedApprenticeshipsRequest(query.Ukprn).GetUrl))),
            Times.Once);
    }

    [Test, MoqAutoData]
    public async Task WhenHandleingUnsuccessfulResponse_ThenThrowsException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderNotRestrictedApprenticeshipsQuery query,
        GetProviderNotRestrictedApprenticeshipsQueryHandler sut)
    {
        // Arrange
        apiClientMock
            .Setup(a => a.GetWithResponseCode<GetProviderNotRestrictedApprenticeshipsResponse>(It.IsAny<GetProviderNotRestrictedApprenticeshipsRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderNotRestrictedApprenticeshipsResponse>(new GetProviderNotRestrictedApprenticeshipsResponse(), HttpStatusCode.InternalServerError, ""));

        // Act
        Func<Task> result = () => sut.Handle(query, CancellationToken.None);

        // Assert
        await result.Should().ThrowAsync<ApiResponseException>();
    }
}
