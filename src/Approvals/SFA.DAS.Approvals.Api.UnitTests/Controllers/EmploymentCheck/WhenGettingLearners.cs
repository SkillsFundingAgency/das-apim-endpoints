using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Approvals.Application.EmploymentChecks.Queries.GetEmploymentChecksQuery;
using SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.EmploymentCheck;

public class WhenGettingLearners
{
    [Test, MoqAutoData]
    public async Task Then_Returns_200_And_EvsCheck_List_When_Valid_Ids(
        List<long> apprenticeshipIds,
        List<EvsCheckResponse> evsChecks,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] EmploymentChecksController controller)
    {
        mediator
            .Setup(x => x.Send(It.IsAny<GetEmploymentChecksQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetEmploymentChecksResult { Checks = evsChecks });

        var result = await controller.Get(apprenticeshipIds);

        result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result;
        ok.Value.Should().BeEquivalentTo(new GetEmploymentChecksResult { Checks = evsChecks });
    }

    [Test, MoqAutoData]
    public async Task And_Null_ApprenticeshipIds_Then_Returns_400(
        [Greedy] EmploymentChecksController controller)
    {
        var result = await controller.Get(null);

        result.Should().BeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.Should().Be("apprenticeshipIds is required and must not be empty.");
    }

    [Test, MoqAutoData]
    public async Task And_Empty_ApprenticeshipIds_Then_Returns_400(
        [Greedy] EmploymentChecksController controller)
    {
        var result = await controller.Get(new List<long>());

        result.Should().BeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.Should().Be("apprenticeshipIds is required and must not be empty.");
    }

    [Test, MoqAutoData]
    public async Task And_More_Than_1000_Ids_Then_Returns_400(
        [Greedy] EmploymentChecksController controller)
    {
        var tooMany = new List<long>();
        for (long i = 0; i < 1001; i++) tooMany.Add(i);

        var result = await controller.Get(tooMany);

        result.Should().BeOfType<BadRequestObjectResult>();
        ((BadRequestObjectResult)result).Value.Should().Be("apprenticeshipIds must not exceed 1000.");
    }

    [Test, MoqAutoData]
    public async Task And_Mediator_Throws_Then_Returns_400(
        List<long> apprenticeshipIds,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] EmploymentChecksController controller)
    {
        mediator
            .Setup(x => x.Send(It.IsAny<GetEmploymentChecksQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Employment check API error"));

        var result = await controller.Get(apprenticeshipIds);

        result.Should().BeOfType<BadRequestResult>();
    }
}
