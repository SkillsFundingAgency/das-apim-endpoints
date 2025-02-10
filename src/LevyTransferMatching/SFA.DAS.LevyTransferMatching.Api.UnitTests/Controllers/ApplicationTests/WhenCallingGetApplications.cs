using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests;

public class WhenCallingGetApplications
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Applications_From_Mediator(GetApplicationsQueryResult queryResult, [Frozen] Mock<IMediator> mediator,
        [Greedy] ApplicationsController controller)
    {
        mediator.Setup(o => o.Send(It.IsAny<GetApplicationsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var controllerResult = await controller.GetApplications(1) as OkObjectResult;
        var result = controllerResult.Value as GetApplicationsResponse;

        controllerResult.Should().NotBeNull();
        result.Should().NotBeNull();

        var expected = (GetApplicationsResponse)queryResult;
        var expectedApplication = expected.Items.First();
        var actualApplication = result.Items.First();

        result.Items.Count().Should().Be(expected.Items.Count());
        result.PageSize.Should().Be(queryResult.PageSize);
        result.TotalItems.Should().Be(queryResult.TotalItems);
        result.TotalPages.Should().Be(queryResult.TotalPages);

        actualApplication.DasAccountName.Should().Be(expectedApplication.DasAccountName);
        actualApplication.Details.Should().Be(expectedApplication.Details);
        actualApplication.Status.Should().Be(expectedApplication.Status);
        actualApplication.CreatedOn.Should().Be(expectedApplication.CreatedOn);
        actualApplication.NumberOfApprentices.Should().Be(expectedApplication.NumberOfApprentices);
        actualApplication.Id.Should().Be(expectedApplication.Id);
        actualApplication.PledgeId.Should().Be(expectedApplication.PledgeId);
        actualApplication.IsNamePublic.Should().Be(expectedApplication.IsNamePublic);
    }
}
