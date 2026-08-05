using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfilesByAccountId;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerProfiles;

[TestFixture]
internal class WhenGettingByAccountId
{
    [Test, MoqAutoData]
    public async Task Then_Gets_EmployerProfiles_From_Mediator(
        long accountId,
        GetEmployerProfilesByAccountIdQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerProfileController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetEmployerProfilesByAccountIdQuery>(c => c.AccountId.Equals(accountId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        IResult? controllerResult = await controller.GetMany(accountId);

        Assert.That(controllerResult, Is.Not.Null);
        var payload = (controllerResult as Ok<GetEmployerProfilesByAccountIdQueryResult>)?.Value;

        // assert
        payload.Should().NotBeNull();
        payload.Should().BeEquivalentTo(mediatorResult, options => options.ExcludingMissingMembers());
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Problem_Request(
        long accountId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerProfileController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetEmployerProfilesByAccountIdQuery>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        IResult? controllerResult = await controller.GetMany(accountId);

        controllerResult.Should().NotBeNull();

        var problemResult = controllerResult as ProblemHttpResult;

        problemResult.Should().NotBeNull();
        problemResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
