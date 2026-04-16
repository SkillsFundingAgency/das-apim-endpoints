using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries.GetStandardInformation;

[TestFixture]
public class GetStandardInformationQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_FoundCourseInformation_ReturnsResult(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetStandardInformationQueryHandler sut,
        GetStandardInformationQuery query,
        GetCourseDetailsResponse apiResponse)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetCourseDetailsResponse>(It.Is<GetCourseDetailsRequest>(r => r.LarsCode == query.LarsCode && r.GetUrl == $"standards/{query.LarsCode}"))).ReturnsAsync(new ApiResponse<GetCourseDetailsResponse>(apiResponse, HttpStatusCode.OK, null));

        var result = await sut.Handle(query, new CancellationToken());

        result.Should().BeEquivalentTo(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulApiResponse_ThrowsException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetStandardInformationQueryHandler sut,
        GetStandardInformationQuery query)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetCourseDetailsResponse>(It.IsAny<GetCourseDetailsRequest>())).ReturnsAsync(new ApiResponse<GetCourseDetailsResponse>(null, HttpStatusCode.NotFound, null));

        Func<Task> action = () => sut.Handle(query, new CancellationToken());

        await action.Should().ThrowAsync<ApiResponseException>();
    }
}