using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.Controllers;
using SFA.DAS.LearnerData.Application.GetShortCourseLearners;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

public class WhenGettingAllShortCourseLearners
{
    [Test, MoqAutoData]
    public async Task Then_Ok_response_is_returned_with_paged_shortcourse_learners_and_links_set(
    string ukprn,
    List<string> ulns,
    [Frozen] Mock<IMediator> mockMediator,
    [Frozen] Mock<ILogger<ShortCoursesController>> mockLogger,
    [Greedy] ShortCoursesController sut)
    {
        const int academicYear = 2425;
        // Arrange
        var queryResult = CreateShortCourseLearnersQueryResult(ulns);

        mockMediator
            .Setup(x => x.Send(It.IsAny<GetShortCourseLearnersQuery>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(queryResult));

        // Setup fake HttpContext to allow headers to be set
        var context = new DefaultHttpContext();
        sut.ControllerContext = new ControllerContext { HttpContext = context };

        // Act
        var result = await sut.GetShortCourseLearners(ukprn, academicYear, 1, ulns.Count) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var resultBody = result.Value as GetShortCourseLearnersResponse;
        resultBody.Should().NotBeNull();

        foreach (var expectedLearner in queryResult.Items)
        {
            resultBody!.Learners.Should().ContainSingle(x =>
                x.Uln == expectedLearner.Uln &&
                x.Key == expectedLearner.Key);
        }

        context.Response.Headers.Should().ContainKey("links");
    }

    private GetShortCourseLearnersQueryResult CreateShortCourseLearnersQueryResult(List<string> ulns)
    {
        var learners = ulns.Select(uln => new Learner { Uln = uln, Key = Guid.NewGuid() }).ToList();
        return new GetShortCourseLearnersQueryResult
        {
            Items = learners,
            TotalItems = learners.Count,
            Page = 1,
            PageSize = learners.Count
        };
    }  
}