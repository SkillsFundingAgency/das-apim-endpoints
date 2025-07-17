using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Controllers;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseLevels;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Controllers.Courses;

public sealed class WhenQueryingCourseLevels
{
    [Test]
    [MoqAutoData]
    public async Task Then_Passes_Query_To_Mediator_And_Returns_Levels(
        GetCourseLevelsListResponse expectedResponse
    )
    {
        Mock<IMediator> mockMediator = new Mock<IMediator>();
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetCourseLevelsQuery>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(expectedResponse);

        var sut = new CoursesController(mockMediator.Object);

        var result = await sut.GetCourseLevels() as ObjectResult;


        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));

            var model = result.Value as GetCourseLevelsListResponse;
            Assert.That(model, Is.Not.Null);
            Assert.That(model, Is.EqualTo(expectedResponse));
        });
    }
}
