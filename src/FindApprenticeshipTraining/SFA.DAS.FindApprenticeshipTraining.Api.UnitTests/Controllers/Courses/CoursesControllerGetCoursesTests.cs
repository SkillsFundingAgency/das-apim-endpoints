using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Courses;

public sealed class CoursesControllerGetCoursesTests
{
    [Test, MoqAutoData]
    public async Task WhenGetCourses_ThenReturnsOkAndPassesQueryToMediator(
        GetCoursesQuery query,
        GetCoursesQueryResult result,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] CoursesController sut
    )
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetCoursesQuery>(c =>
                    c.Keyword.Equals(query.Keyword) &&
                    c.RouteIds.Equals(query.RouteIds) &&
                    c.Levels.Equals(query.Levels) &&
                    c.OrderBy.Equals(query.OrderBy) &&
                    c.Distance.Equals(query.Distance) &&
                    c.LocationName.Equals(query.LocationName) &&
                    c.Page.Equals(query.Page) &&
                    c.PageSize.Equals(query.PageSize)
                ),
                It.IsAny<CancellationToken>()
            )
        )
        .ReturnsAsync(result);

        var response = await sut.GetCourses(query);

        mockMediator.Verify(mediator => mediator.Send(
                It.Is<GetCoursesQuery>(c =>
                    c.Keyword.Equals(query.Keyword) &&
                    c.RouteIds.Equals(query.RouteIds) &&
                    c.Levels.Equals(query.Levels) &&
                    c.OrderBy.Equals(query.OrderBy) &&
                    c.Distance.Equals(query.Distance) &&
                    c.LocationName.Equals(query.LocationName) &&
                    c.Page.Equals(query.Page) &&
                    c.PageSize.Equals(query.PageSize)
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<OkObjectResult>());
            Assert.That(response, Is.TypeOf<OkObjectResult>());

            var returnedType = (response as OkObjectResult).Value as GetCoursesQueryResult;

            Assert.That(returnedType, Is.Not.Null);
        });
    }
}