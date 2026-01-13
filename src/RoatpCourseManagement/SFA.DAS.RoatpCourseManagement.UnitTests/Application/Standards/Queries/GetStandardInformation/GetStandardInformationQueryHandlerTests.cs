using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
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
        GetStandardForLarsCodeResponse apiResponse)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetStandardForLarsCodeResponse>(It.Is<GetStandardForLarsCodeRequest>(r => r.LarsCode == query.LarsCode && r.GetUrl == $"standards/{query.LarsCode}"))).ReturnsAsync(new ApiResponse<GetStandardForLarsCodeResponse>(apiResponse, HttpStatusCode.OK, null));

        var result = await sut.Handle(query, new CancellationToken());

        result.Should().BeEquivalentTo(apiResponse, option =>
        {
            option.Excluding(s => s.Version);
            return option;
        });

        result.LarsCode.Should().Be(apiResponse.LarsCode.ToString());
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulApiResponse_ThrowsException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetStandardInformationQueryHandler sut,
        GetStandardInformationQuery query)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetStandardForLarsCodeResponse>(It.IsAny<GetStandardForLarsCodeRequest>())).ReturnsAsync(new ApiResponse<GetStandardForLarsCodeResponse>(null, HttpStatusCode.NotFound, null));

        Func<Task> action = () => sut.Handle(query, new CancellationToken());

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}