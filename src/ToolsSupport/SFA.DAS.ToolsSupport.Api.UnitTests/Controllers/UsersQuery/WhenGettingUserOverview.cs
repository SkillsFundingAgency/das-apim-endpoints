using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Api.Controllers;
using SFA.DAS.ToolsSupport.Application.Queries.GetUserOverview;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Controllers.UsersQuery;

public class WhenGettingUserOverview
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Account_Details_From_Mediator(
         Guid userId,
         GetUserOverviewQueryResult getOverviewResult,
         [Frozen] Mock<IMediator> mockMediator,
         [Greedy] UsersQueryController controller)
    {
        mockMediator
               .Setup(mediator => mediator.Send(
                   It.Is<GetUserOverviewQuery>(x =>
                    x.UserId == userId),
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync(getOverviewResult);

        var controllerResult = await controller.GetUserOverview(userId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        var model = controllerResult.Value as GetUserOverviewQueryResult;

        model.Should().NotBeNull();
        model.Id.Should().Be(getOverviewResult.Id);
        model.FirstName.Should().Be(getOverviewResult.FirstName);
        model.LastName.Should().Be(getOverviewResult.LastName);
        model.Email.Should().Be(getOverviewResult.Email);
        model.IsActive.Should().Be(getOverviewResult.IsActive);
        model.IsLocked.Should().Be(getOverviewResult.IsLocked);
        model.IsSuspended.Should().Be(getOverviewResult.IsSuspended);
        model.AccountSummaries.Should().BeEquivalentTo(getOverviewResult.AccountSummaries);
    }
}
