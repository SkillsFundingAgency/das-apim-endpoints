using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Earnings.Api.Controllers;
using SFA.DAS.Earnings.Application.LearnerData.GetLearnerData;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Earnings.Api.UnitTests.Controllers.LearnerData;

public class WhenGettingLearnerData
{
    [Test, MoqAutoData]
    public async Task Then_Should_Return_Learner_Data_Response(
        [Frozen] Mock<IMediator> mediator,
        [Greedy]LearnerDataController controller)
    {
        // Arrange
        const int ukprn = 1000001;
        const int academicYear = 2425;
        const int page = 1;
        const int pageSize = 10;
    
        const int totalRecords = 30;

        var learnerDataResponse = new GetLearnerDataQueryResult
        {
            TotalRecords = totalRecords
        };
        
        SetUpHttpContext(controller, ukprn, academicYear);

        mediator.Setup(m => m.Send(It.IsAny<GetLearnerDataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(learnerDataResponse);

        // Act
        var result = await controller.Search(ukprn, academicYear, page, pageSize) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<OkObjectResult>();
        result!.Value.Should().BeEquivalentTo(learnerDataResponse);

        // Verify the headers
        controller.Response.Headers.Should().ContainKey("Link");
        var linkHeader = controller.Response.Headers.Link.ToString();
    
        var expectedBaseUrl = $"http://localhost/providers/{ukprn}/academicyears/{academicYear}/apprenticeships";
        var expectedNextPage = $"<{expectedBaseUrl}?page=2&pageSize={pageSize}>; rel=\"next\"";

        linkHeader.Should().Contain(expectedNextPage);
    }
    
    private void SetUpHttpContext(LearnerDataController controller, long ukprn, int academicYear)
    {
        var context = new DefaultHttpContext();
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };
        controller.ControllerContext.HttpContext.Request.Scheme = "http";  // Setting scheme
        controller.ControllerContext.HttpContext.Request.Host = new Microsoft.AspNetCore.Http.HostString("localhost");  // Set Host
        controller.ControllerContext.HttpContext.Request.PathBase = string.Empty;  // Set PathBase (in case you use any path)
        controller.ControllerContext.HttpContext.Request.Path = $"/providers/{ukprn}/academicyears/{academicYear}/apprenticeships";  // Set Path
    }
}