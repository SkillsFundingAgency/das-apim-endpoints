using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Queries.GetProviderCourseForecasts;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderCourseForecastsControllerTests;

public class ProviderCourseForecastsControllerGetProviderCourseForecastsTests
{
    [Test, MoqAutoData]
    public async Task GetProviderCourseForecasts_ReturnsOk(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ProviderCourseForecastsController sut,
        GetProviderCourseForecastsQuery query,
        GetProviderCourseForecastsQueryResult expected,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetProviderCourseForecastsQuery>(q => q.Ukprn == query.Ukprn && q.LarsCode == query.LarsCode), cancellationToken)).ReturnsAsync(expected);

        var actual = await sut.GetProviderCourseForecasts(query.Ukprn, query.LarsCode, cancellationToken);

        actual.As<OkObjectResult>().Value.Should().Be(expected);
    }
}
