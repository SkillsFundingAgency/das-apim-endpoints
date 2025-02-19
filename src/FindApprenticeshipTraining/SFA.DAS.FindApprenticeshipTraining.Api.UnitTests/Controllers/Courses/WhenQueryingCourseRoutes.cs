using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseRoutes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Courses;

public sealed class WhenQueryingCourseRoutes
{
    [Test]
    [MoqAutoData]
    public async Task Then_Passes_Query_To_Mediator_And_Returns_Routes(
        GetRoutesListResponse expectedResponse
    )
    {
        Mock<IMediator> mockMediator = new Mock<IMediator>();
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetCourseRoutesQuery>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(expectedResponse);

        var sut = new CoursesController(mockMediator.Object);

        var result = await sut.GetCourseRoutes() as ObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

            var model = result.Value as GetRoutesListResponse;
            Assert.That(model, Is.Not.Null);
            Assert.That(model, Is.EqualTo(expectedResponse));
        });
    }
}