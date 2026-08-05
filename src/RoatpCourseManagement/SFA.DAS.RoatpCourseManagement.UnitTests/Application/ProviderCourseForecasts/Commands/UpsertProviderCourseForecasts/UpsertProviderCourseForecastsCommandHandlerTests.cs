using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Models;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;

public class UpsertProviderCourseForecastsCommandHandlerTests
{
    [Test, AutoData]
    public async Task Handle_WhenCalled_ThenShouldPostToApi(
        int ukprn,
        string larsCode,
        IEnumerable<UpsertProviderCourseForecastModel> forecasts)
    {
        // Arrange
        var command = new UpsertProviderCourseForecastsCommand(ukprn, larsCode, forecasts);
        var apiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        apiClientMock.Setup(client => client.PostWithResponseCode<object>(
            It.IsAny<UpsertProviderCourseForecastsRequest>(), false)).ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.OK, string.Empty));
        var handler = new UpsertProviderCourseForecastsCommandHandler(apiClientMock.Object);
        // Act
        await handler.Handle(command, CancellationToken.None);
        // Assert
        apiClientMock.Verify(client => client.PostWithResponseCode<object>(
            It.Is<UpsertProviderCourseForecastsRequest>(req =>
                req.Ukprn == ukprn &&
                req.LarsCode == larsCode &&
                req.Data == forecasts), false), Times.Once);
    }

    [Test, AutoData]
    public async Task Handle_WhenApiError_ThenShouldRaiseApiResponseException(
    UpsertProviderCourseForecastsCommand command,
    IEnumerable<UpsertProviderCourseForecastModel> forecasts)
    {
        // Arrange
        var apiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        var handler = new UpsertProviderCourseForecastsCommandHandler(apiClientMock.Object);
        apiClientMock.Setup(client => client.PostWithResponseCode<object>(
            It.IsAny<UpsertProviderCourseForecastsRequest>(), false)).ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.BadRequest, string.Empty));
        // Act
        var action = () => handler.Handle(command, CancellationToken.None);
        // Assert
        await action.Should().ThrowAsync<ApiResponseException>();
    }
}
