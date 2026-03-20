using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderCourseForecastsControllerTests;

public class UpsertProviderCourseForecastsTests
{
    [Test, AutoData]
    public async Task UpsertProviderCourseForecasts_Returns_NoContent(
        int ukprn,
        string larsCode,
        IEnumerable<UpsertProviderCourseForecastModel> forecasts,
        CancellationToken cancellationToken)
    {
        Mock<IMediator> mediatorMock = new();
        ProviderCourseForecastsController sut = new(mediatorMock.Object);

        var result = await sut.UpsertProviderCourseForecasts(ukprn, larsCode, forecasts, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<UpsertProviderCourseForecastsCommand>(c => c.Ukprn == ukprn && c.LarsCode == larsCode && c.Forecasts == forecasts), cancellationToken), Times.Once);
        result.Should().BeOfType<NoContentResult>();
    }
}
