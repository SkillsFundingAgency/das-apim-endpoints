using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderCourse;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetProviderCourse;

public class GetProviderCourseQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task WhenHandlingRequest_ThenReturnsProviderCourse(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderCourseResponse response,
        GetProviderCourseQuery query,
        GetProviderCourseQueryHandler sut)
    {
        apiClientMock
            .Setup(c => c.GetWithResponseCode<GetProviderCourseResponse>(It.Is<GetProviderCourseRequest>(r => r.GetUrl.Equals(new GetProviderCourseRequest(query.Ukprn, query.LarsCode).GetUrl))))
            .ReturnsAsync(new ApiResponse<GetProviderCourseResponse>(response, HttpStatusCode.OK, ""));

        var result = await sut.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().BeEquivalentTo(response);
            apiClientMock.Verify(
                c => c.GetWithResponseCode<GetProviderCourseResponse>(It.Is<GetProviderCourseRequest>(r => r.GetUrl.Equals(new GetProviderCourseRequest(query.Ukprn, query.LarsCode).GetUrl))),
                Times.Once);
        }
    }

    [Test, MoqAutoData]
    public async Task WhenHandlingRequestAndBadRequestIsReturned_ThenReturnsNull(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderCourseQuery query,
        GetProviderCourseQueryHandler sut)
    {
        apiClientMock
            .Setup(c => c.GetWithResponseCode<GetProviderCourseResponse>(It.IsAny<GetProviderCourseRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderCourseResponse>(null!, HttpStatusCode.BadRequest, ""));

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task WhenHandlingRequestAndApiReturnsInvalidResponseCode_ThenThrowsApiResponseException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderCourseQuery query,
        GetProviderCourseQueryHandler sut)
    {
        apiClientMock
            .Setup(c => c.GetWithResponseCode<GetProviderCourseResponse>(It.IsAny<GetProviderCourseRequest>()))
            .ReturnsAsync(new ApiResponse<GetProviderCourseResponse>(new GetProviderCourseResponse(), HttpStatusCode.InternalServerError, ""));

        Func<Task> act = () => sut.Handle(query, CancellationToken.None);

        await act.Should().ThrowAsync<ApiResponseException>();
    }
}
