using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Courses;

public sealed class WhenCallingGetCourseByLarsCode
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Course_From_Mediator(
        GetCourseByLarsCodeQuery query,
        GetCourseByLarsCodeQueryResult result,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] ILogger<CoursesController> mockLogger,
        [Greedy] CoursesController controller
    )
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetCourseByLarsCodeQuery>(c =>
                    c.LarsCode.Equals(query.LarsCode) &&
                    c.Distance.Equals(query.Distance) &&
                    c.Location.Equals(query.Location)
                ),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var sut = await controller.GetCourseByLarsCode(query.LarsCode, query.Distance, query.Location) as ObjectResult;

        Assert.That(sut, Is.Not.Null);
        Assert.That(sut.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

        var model = sut.Value as GetCourseByLarsCodeQueryResult;
        Assert.That(model, Is.Not.Null);
        model.Should().Be(result);
    }

    [Test, MoqAutoData]
    public async Task And_Null_Course_Returns_Not_Found_Status_Code(
        GetCourseByLarsCodeQuery query,
        [Frozen] Mock<IMediator> mockMediator,
        [Frozen] ILogger<CoursesController> mockLogger,
        [Greedy] CoursesController controller
    )
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetCourseByLarsCodeQuery>(c =>
                    c.LarsCode.Equals(query.LarsCode) &&
                    c.Distance.Equals(query.Distance) &&
                    c.Location.Equals(query.Location)
                ),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetCourseByLarsCodeQueryResult)null);

        var sut = await controller.GetCourseByLarsCode(query.LarsCode, query.Distance, query.Location) as NotFoundResult;

        Assert.That(sut, Is.Not.Null);
        Assert.That(sut.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
    }
}
