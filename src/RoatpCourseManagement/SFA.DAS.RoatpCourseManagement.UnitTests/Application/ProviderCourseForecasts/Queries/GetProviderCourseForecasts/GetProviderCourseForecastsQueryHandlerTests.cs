using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Queries.GetProviderCourseForecasts;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.ProviderCourseForecasts.Queries.GetProviderCourseForecasts;

public class GetProviderCourseForecastsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handler_ApiResponseOk_ReturnsExpectedResult(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderCourseForecastsQueryHandler sut,
        GetProviderCourseForecastsQuery query,
        GetProviderCourseForecastsQueryResult expected,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderCourseForecastsQueryResult>(It.Is<GetProviderCourseForecastsRequest>(r => r.Ukprn == query.Ukprn && r.LarsCode == query.LarsCode))).ReturnsAsync(new ApiResponse<GetProviderCourseForecastsQueryResult>(expected, System.Net.HttpStatusCode.OK, string.Empty));

        GetProviderCourseForecastsQueryResult actual = await sut.Handle(query, default);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test, MoqAutoData]
    public void Handler_ApiResponseNotOk_ThrowsException(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        GetProviderCourseForecastsQueryHandler sut,
        GetProviderCourseForecastsQuery query,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetProviderCourseForecastsQueryResult>(It.Is<GetProviderCourseForecastsRequest>(r => r.Ukprn == query.Ukprn && r.LarsCode == query.LarsCode))).ReturnsAsync(new ApiResponse<GetProviderCourseForecastsQueryResult>(null, System.Net.HttpStatusCode.InternalServerError, string.Empty));

        Func<Task> act = async () => await sut.Handle(query, default);

        act.Should().ThrowAsync<ApiResponseException>();
    }
}
