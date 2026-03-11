using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;

public class UpsertProviderCourseForecastsCommandHandlerTests
{
    [Test, AutoData]
    public async Task Handle_WhenCalled_ThenShouldPostToApi(
        int ukprn,
        string larsCode,
        IEnumerable<UpsertProviderCourseForecastModel> forecasts,
        CancellationToken cancellationToken)
    {
        // Arrange
        var apiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        var handler = new UpsertProviderCourseForecastsCommandHandler(apiClientMock.Object);
        var command = new UpsertProviderCourseForecastsCommand(ukprn, larsCode, forecasts);
        // Act
        await handler.Handle(command, CancellationToken.None);
        // Assert
        apiClientMock.Verify(client => client.PostWithResponseCode<object>(
            It.Is<UpsertProviderCourseForecastsRequest>(req =>
                req.Ukprn == ukprn &&
                req.LarsCode == larsCode &&
                req.Data == forecasts), false), Times.Once);
    }
}
